using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadCart();
            LoadCategories();
        }

        private void LoadCart()
        {
            if (Session["buyerID"] == null) return;

            int buyerID = Convert.ToInt32(Session["buyerID"]);
            decimal total = 0;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT b.bookISBN AS bookISBN, b.title AS Title, b.price AS Price, ci.quantity AS Quantity, b.coverImage
                    FROM Cart c
                    JOIN CartItems ci ON c.cartID = ci.cartID
                    JOIN Book b ON b.bookISBN = ci.bookISBN
                    WHERE c.buyerID = @buyerID", con);
                cmd.Parameters.AddWithValue("@buyerID", buyerID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    decimal price = Convert.ToDecimal(row["Price"]);
                    int qty = Convert.ToInt32(row["Quantity"]);
                    total += price * qty;
                }

                rptCartItems.DataSource = dt;
                rptCartItems.DataBind();

                lblTotal.Text = total.ToString("0.00");
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
            }
        }

        protected void btnShowPayment_Click(object sender, EventArgs e)
        {
            pnlPayment.Visible = true;
        }

        protected void btnPurchase_Click(object sender, EventArgs e)
        {
            if (Session["buyerID"] == null) return;

            int buyerID = Convert.ToInt32(Session["buyerID"]);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                con.Open();

                SqlCommand getCartIDCmd = new SqlCommand("SELECT cartID FROM Cart WHERE buyerID = @buyerID", con);
                getCartIDCmd.Parameters.AddWithValue("@buyerID", buyerID);
                int cartID = (int)getCartIDCmd.ExecuteScalar();

                SqlCommand clearCartCmd = new SqlCommand("DELETE FROM CartItems WHERE cartID = @cartID", con);
                clearCartCmd.Parameters.AddWithValue("@cartID", cartID);
                clearCartCmd.ExecuteNonQuery();


                lblConfirmation.Text = "Purchase successful!";
                lblConfirmation.Visible = true;
                pnlPayment.Visible = false;

                LoadCart();
            }
        }
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string bookISBN = btn.CommandArgument;
            int buyerID = Convert.ToInt32(Session["buyerID"]);

            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                int cartID = 0;
                string getCartIDQuery = "SELECT cartID FROM Cart WHERE buyerID = @buyerID";
                using (SqlCommand getCartCmd = new SqlCommand(getCartIDQuery, conn))
                {
                    getCartCmd.Parameters.AddWithValue("@buyerID", buyerID);
                    object result = getCartCmd.ExecuteScalar();
                    if (result != null)
                    {
                        cartID = Convert.ToInt32(result);
                    }
                    else
                    {
                        return;
                    }
                }

                string deleteQuery = "DELETE FROM CartItems WHERE cartID = @cartID AND bookISBN = @bookISBN";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@cartID", cartID);
                    deleteCmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                    deleteCmd.ExecuteNonQuery();
                }

                conn.Close();
            }

            LoadCart();
        }


    }
}
