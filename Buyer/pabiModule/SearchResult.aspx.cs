using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class SearchResult : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string searchTerm = Request.QueryString["query"].ToString();

            if (!IsPostBack && Request.QueryString["query"] != null)
            {
                string displayText = GetDisplayText(searchTerm);
                lblSearched.Text = Server.HtmlEncode(displayText);
                LoadSearchResults(searchTerm);
                LoadCategories();
            }
            else if (!IsPostBack)
            {
                lblSearched.Text = "nothing";
            }
            
        }

        private string GetDisplayText(string searchTerm)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                string sqlIsbn = "SELECT TOP 1 title FROM Book WHERE bookISBN = @isbn";
                using (SqlCommand cmd = new SqlCommand(sqlIsbn, con))
                {
                    cmd.Parameters.AddWithValue("@isbn", searchTerm.Trim());
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return result.ToString();
                    }
                }
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                string sqlLike = "SELECT TOP 1 title FROM Book WHERE title LIKE @term OR author LIKE @term";
                using (SqlCommand cmd = new SqlCommand(sqlLike, con))
                {
                    cmd.Parameters.AddWithValue("@term", "%" + searchTerm.Trim() + "%");
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return result.ToString();
                    }
                }
            }

            return searchTerm;
        }
        private void LoadSearchResults(string searchTerm)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                string query = @"
            SELECT bookISBN, title, author, price, coverImage, status
            FROM Book 
            WHERE (title LIKE @searchTerm OR author LIKE @searchTerm OR bookISBN LIKE @searchTerm)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    bool allUnavailable = true;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["status"].ToString().ToLower() == "available")
                        {
                            allUnavailable = false;
                            break;
                        }
                    }

                    if (allUnavailable)
                    {
                        lblSearched.Text = "Oops, we are sorry, this book is currently unavailable.";
                        rptBooks.DataSource = null;
                        rptBooks.DataBind();
                    }
                    else
                    {
                        rptBooks.DataSource = dt;
                        rptBooks.DataBind();
                    }
                }
                else
                {
                    lblSearched.Text = "No results found for '" + searchTerm + "'";
                    rptBooks.DataSource = null;
                    rptBooks.DataBind();
                }
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
                Response.Redirect("~/Admin/GraceModule/ManageCategories.aspx?category=" + Server.UrlEncode(faculty));
            }
        }

        protected void rptBooks_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                if (Session["buyerID"] == null)
                {
                    Response.Redirect("~/Login");
                    return;
                }

                int buyerID = Convert.ToInt32(Session["buyerID"]);
                string bookISBN = e.CommandArgument.ToString();

                if (!IsBookAvailable(bookISBN))
                {
                    lblMessage.Text = "Sorry, this book is no longer available.";
                    lblMessage.CssClass = "error-message";
                    return;
                }

                AddToCart(buyerID, bookISBN, 1);

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT title, coverImage FROM Book WHERE bookISBN = @bookISBN", con);
                    cmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblCartBookTitle.Text = reader["title"].ToString();
                        imgCartBook.ImageUrl = reader["coverImage"].ToString();
                    }
                }

                CartPanel.Visible = true;
                CartPanel.CssClass = "slide-panel slide-panel-visible";
                lblMessage.Text = "";

                ((Site1)this.Master).UpdateCartCount();
            }
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


        protected void btnClose_Click(object sender, EventArgs e)
        {
            CartPanel.Visible = false;
            CartPanel.CssClass = "slide-panel";
        }

        private void AddToCart(int buyerID, string bookISBN, int quantity)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
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
        }

        private int CreateCart(SqlConnection con, int buyerID)
        {
            SqlCommand createCartCmd = new SqlCommand("INSERT INTO Cart (buyerID) OUTPUT INSERTED.cartID VALUES (@buyerID)", con);
            createCartCmd.Parameters.AddWithValue("@buyerID", buyerID);
            return (int)createCartCmd.ExecuteScalar();
        }

      

    }
}
