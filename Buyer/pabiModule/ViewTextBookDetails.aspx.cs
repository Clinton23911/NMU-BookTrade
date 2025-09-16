using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class ViewTextBookDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.QueryString["id"] != null)
            {
                string bookISBN = Request.QueryString["id"];
                LoadBookDetails(bookISBN);
                LoadCategories();
            }
        }

        private void LoadBookDetails(string bookISBN)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT title, author, price, condition, coverImage, bookISBN FROM Book WHERE bookISBN = @bookISBN AND status = 'available'", con);
                cmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                con.Open();

                string title = "";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblTitle.Text = reader["title"].ToString();
                    lblAuthor.Text = reader["author"].ToString();
                    lblPrice.Text = reader["price"].ToString();
                    lblCondition.Text = reader["condition"].ToString();
                    //lblStock.Text = reader["stock"].ToString();
                    imgCover.ImageUrl = reader["coverImage"].ToString();

                    ViewState["bookISBN"] = bookISBN;
                    ViewState["title"] = title;
                }

                reader.Close();

            }
        }

        private void LoadCategories()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT categoryName FROM Category", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                rptCategory.DataSource = dt;
                rptCategory.DataBind();
            }
        }

        protected void rptCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectCategory")
            {
                string faculty = e.CommandArgument.ToString();
                Response.Redirect("~/Category.aspx?category=" + Server.UrlEncode(faculty));

                if (e.Item.ItemIndex == 2)
                {
                    PlaceHolder phSearchBar = (PlaceHolder)e.Item.FindControl("phSearchBar");
                    if (phSearchBar != null)
                    {
                        phSearchBar.Visible = true;
                    }
                }
            }
        }
        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (Session["buyerID"] == null)
            {
                Response.Redirect("~/Login");
                return;
            }

            int buyerID = Convert.ToInt32(Session["buyerID"]);
            string bookISBN = ViewState["bookISBN"].ToString();

            if (!IsBookAvailable(bookISBN))
            {
                lblMessage.Text = "Sorry, this book is no longer available.";
                lblMessage.CssClass = "error";
                return;
            }

            AddToCart(buyerID, bookISBN, 1);

            lblCartMessage.Text = "<strong>Book added to cart!</strong>";
            CartPanel.Visible = true;
            CartPanel.CssClass = "slide-panel slide-panel-visible";



        }


        protected void btnClose_Click(object sender, EventArgs e)
        {
            CartPanel.Visible = false;
            CartPanel.CssClass = "slide-panel";
        }


        private bool IsBookAvailable(string bookISBN)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Book WHERE bookISBN = @bookISBN AND status = 'available'", con);
                cmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                con.Open();

                int count = (int)cmd.ExecuteScalar();
                return count > 0;

            }
        }


        private void AddToCart(int buyerID, string bookISBN, int quantity)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                con.Open();

                SqlCommand getCartCmd = new SqlCommand("SELECT cartID FROM Cart WHERE buyerID = @buyerID", con);
                getCartCmd.Parameters.AddWithValue("@buyerID", buyerID);

                object result = getCartCmd.ExecuteScalar();
                int cartID;

                if (result != null)
                {
                    cartID = Convert.ToInt32(result);
                }
                else
                {
                    SqlCommand createCartCmd = new SqlCommand("INSERT INTO Cart (buyerID) OUTPUT INSERTED.cartID VALUES (@buyerID)", con);
                    createCartCmd.Parameters.AddWithValue("@buyerID", buyerID);
                    cartID = (int)createCartCmd.ExecuteScalar();
                }

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
        }

    }
}
