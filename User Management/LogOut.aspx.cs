
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class WebForm18 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.UrlReferrer != null)
            {
                Session["PrevPage"] = Request.UrlReferrer.ToString();
            }
        }

        private string GetCustomerName(string userId)
        {
            string name = "User";
            string connectionString = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT FullName FROM Buyer WHERE BuyerID = buyerID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    name = reader["FullName"].ToString();
                }
                con.Close();
            }

            return name;
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            if (Session["PreviousPage"] != null)
            {
                Response.Redirect(Session["PreviousPage"].ToString());
            }
            else
            {
                Response.Redirect("Home.aspx");
            }
        }
    }
}
