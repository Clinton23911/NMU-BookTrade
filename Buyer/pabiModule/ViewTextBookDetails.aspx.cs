using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class ViewTextBookDetails : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;
        string bookISBN;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["bookISBN"] != null)
                {
                    bookISBN = Request.QueryString["bookISBN"];
                    LoadBookDetails(bookISBN);
                    LoadReviews();
                }
                else
                {
                    Response.Redirect("~/BuyerDashboard.aspx");
                }
                ((Site1)this.Master).UpdateCartCount();
            }
        }

        private void LoadBookDetails(string bookISBN)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                   SELECT 
                        b.title,
                        b.author,
                        b.bookISBN,
                        c.categoryName,
                        g.genreName,
                        b.condition,
                        b.price,
                        s.sellerName,
                        b.coverImage
                   FROM Book b
                   INNER JOIN Category c ON b.categoryID = c.categoryID
                   INNER JOIN Genre g ON b.genreID = g.genreID
                   INNER JOIN Seller s ON b.sellerID = s.sellerID
                   WHERE b.bookISBN = @bookISBN";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@bookISBN", bookISBN);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblTitle.Text = reader["title"].ToString();
                    lblAuthor.Text = reader["author"].ToString();
                    lblISBN.Text = reader["bookISBN"].ToString();
                    lblCategory.Text = reader["categoryName"].ToString();
                    lblGenre.Text = reader["genreName"].ToString();
                    lblCondition.Text = reader["condition"].ToString();
                    lblPrice.Text = reader["price"].ToString();
                    lblSeller.Text = reader["sellerName"].ToString();
                    string coverImage = reader["coverImage"].ToString();
                    imgBookCover.ImageUrl = ResolveUrl("~/Images/" + coverImage);
                }
            }
        }

        private void LoadReviews()
        {
            string query = @"
                SELECT 
                    r.reviewID,
                    r.reviewRating,
                    r.reviewComment,
                    r.reviewDate,
                    b.buyerName,
                    b.buyerSurname,
                    b.buyerProfileImage
                FROM Review r
                INNER JOIN Sale s ON r.saleID = s.saleID
                INNER JOIN Buyer b ON s.buyerID = b.buyerID
                INNER JOIN Book bk ON s.bookISBN = bk.bookISBN
                WHERE bk.bookISBN = @bookISBN
                ORDER BY r.reviewDate DESC";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@bookISBN", Request.QueryString["bookISBN"]);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);
                rptTestimonials.DataSource = dt;
                rptTestimonials.DataBind();

                if (dt.Rows.Count == 0)
                {
                    RepeaterItem footer = rptTestimonials.Controls[rptTestimonials.Controls.Count - 1] as RepeaterItem;
                    if (footer != null)
                    {
                        Panel pnlNoReviews = (Panel)footer.FindControl("pnlNoReviews");
                        if (pnlNoReviews != null)
                            pnlNoReviews.Visible = true;
                    }
                }
            }
        }

        public static string GetStarHtml(int rating)
        {
            string stars = "";
            for (int i = 1; i <= 5; i++)
            {
                stars += (i <= rating)
                    ? "<span style='color: gold;'>&#9733;</span>"
                    : "<span style='color: lightgray;'>&#9734;</span>";
            }
            return stars;
        }

        private void AddToCart(int buyerID, string bookISBN, int quantity)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand getCartCmd = new SqlCommand("SELECT cartID FROM Cart WHERE buyerID = @buyerID", con);
                getCartCmd.Parameters.AddWithValue("@buyerID", buyerID);
                object result = getCartCmd.ExecuteScalar();
                int cartID = result != null ? Convert.ToInt32(result) : CreateCart(con, buyerID);

                SqlCommand checkCmd = new SqlCommand("SELECT quantity FROM CartItems WHERE cartID = @cartID AND bookISBN = @bookISBN", con);
                checkCmd.Parameters.AddWithValue("@cartID", cartID);
                checkCmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                object qtyResult = checkCmd.ExecuteScalar();

                if (qtyResult != null)
                {
                    SqlCommand updateCmd = new SqlCommand("UPDATE CartItems SET quantity = quantity + @qty WHERE cartID = @cartID AND bookISBN = @bookISBN", con);
                    updateCmd.Parameters.AddWithValue("@qty", quantity);
                    updateCmd.Parameters.AddWithValue("@cartID", cartID);
                    updateCmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand insertCmd = new SqlCommand("INSERT INTO CartItems (cartID, bookISBN, quantity) VALUES (@cartID, @bookISBN, @qty)", con);
                    insertCmd.Parameters.AddWithValue("@cartID", cartID);
                    insertCmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                    insertCmd.Parameters.AddWithValue("@qty", quantity);
                    insertCmd.ExecuteNonQuery();
                }
            }

            CartPanel.Visible = true;
            CartPanel.CssClass = "slide-panel slide-panel-visible";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "PanelOpen",
                "document.body.classList.add('panel-open');", true);
            ((Site1)this.Master).UpdateCartCount();
        }

        private int CreateCart(SqlConnection con, int buyerID)
        {
            SqlCommand createCartCmd = new SqlCommand("INSERT INTO Cart (buyerID) OUTPUT INSERTED.cartID VALUES (@buyerID)", con);
            createCartCmd.Parameters.AddWithValue("@buyerID", buyerID);
            return (int)createCartCmd.ExecuteScalar();
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (Session["buyerID"] == null)
            {
                Response.Redirect("~/Login");
                return;
            }

            string bookISBN = Request.QueryString["bookISBN"];
            int buyerID = Convert.ToInt32(Session["buyerID"]);

            if (!IsBookAvailable(bookISBN))
            {
                lblMessage.Text = "Sorry, this book is no longer available.";
                lblMessage.CssClass = "error-message";
                return;
            }

            AddToCart(buyerID, bookISBN, 1);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT title, coverImage FROM Book WHERE bookISBN = @bookISBN", con);
                cmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblCartBookTitle.Text = reader["title"].ToString();
                    imgCartBook.ImageUrl = "~/Images/" + reader["coverImage"].ToString();
                }
            }

            CartPanel.Visible = true;
            CartPanel.CssClass = "slide-panel slide-panel-visible";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "PanelOpen",
                "document.body.classList.add('panel-open');", true);
        }

        private bool IsBookAvailable(string bookISBN)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Book WHERE bookISBN = @bookISBN AND status = 'available'", con);
                cmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            CartPanel.Visible = false;
            CartPanel.CssClass = "slide-panel";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "PanelClose",
                "document.body.classList.remove('panel-open');", true);
        }
    }
}
