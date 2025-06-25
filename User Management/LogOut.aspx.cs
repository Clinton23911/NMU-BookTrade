
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
            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    string userId = Session["UserID"].ToString();
                    string customerName = GetBuyerName(userId).ToUpper();
                    lblCustomerName.Text = customerName;
                }
                if (!IsPostBack && Request.UrlReferrer != null)
                {
                    Session["PrevPage"] = Request.UrlReferrer.ToString();
                }
            }
        }
            public string GetBuyerName(string userId)
        {
            string name = "Buyer";
            string connectionString = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT buyerName FROM Buyer WHERE BuyerID = @UserId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    name = reader["buyerName"].ToString();
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
