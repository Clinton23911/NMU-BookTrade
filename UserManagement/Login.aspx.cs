﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Web.Security;

namespace NMU_BookTrade
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // here we are grabbing the users input from the textboxes 
            string username = txtUsername.Text.Trim();// trim removes the white spaces users make
            string password = txtPassword.Text.Trim();

            using (SqlConnection constr = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                constr.Open();

                //Now we are checking ADMIN TABLE
                SqlCommand cmd = new SqlCommand("SELECT * FROM Admin WHERE adminUsername =@Username and adminPassword = @Password", constr); // building an sqlcommand so that we check the admin table for matching username and passwords  

                cmd.Parameters.AddWithValue("@Username", username); 
                cmd.Parameters.AddWithValue("@Password", password);

                // now we need to execute the SQL and get the result using the data reader
                SqlDataReader reader = cmd .ExecuteReader();

                //checking admin table
                if (reader.Read())
                {
                    Session["AccessID"] = "1";
                    Session["UserID"] = reader["adminID"].ToString();
                    FormsAuthentication.SetAuthCookie(username, false);
                    Response.Redirect("~/Admin/GraceModule/AdminDashboard.aspx");
                    return;

                }

                reader.Close();

                // Clear parameters before reuse
                cmd.Parameters.Clear();

                // Checking Buyer Table 
                cmd = new SqlCommand("SELECT * FROM Buyer WHERE buyerUsername=@Username AND buyerPassword=@Password", constr);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Session["AccessID"] = "2";
                    Session["UserID"] = reader["buyerID"].ToString();// used for general user tracking 
                    Session["BuyerID"] = reader["buyerID"].ToString();// used for deletion
                    FormsAuthentication.SetAuthCookie(username, false);
                    Response.Redirect("~/Buyer/pabiModule/BuyerDashboard.aspx");
                    return;

                }
                reader.Close();

                // Clear parameters before reuse
                cmd.Parameters.Clear();

                // Check Seller table
                cmd = new SqlCommand("SELECT * FROM Seller WHERE sellerUsername=@Username AND sellerPassword=@Password", constr);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Session["AccessID"] = "3";
                    Session["UserID"] = reader["sellerID"].ToString();// used for general user tracking 
                    Session["SellerID"] = reader["sellerID"].ToString();// used for deletion
                    FormsAuthentication.SetAuthCookie(username, false);
                    Response.Redirect("~/Seller/ClintonModule/SellerDashboard.aspx");
                    return;

                }
                reader.Close();

                // Clear parameters before reuse
                cmd.Parameters.Clear();

                // Check Driver table
                cmd = new SqlCommand("SELECT * FROM Driver WHERE driverUsername=@Username AND driverPassword=@Password", constr);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Session["AccessID"] = "4";
                    Session["UserID"] = reader["driverID"].ToString();// used for general user tracking 
                    Session["DriverID"] = reader["driverID"].ToString();// used for deletion
                    FormsAuthentication.SetAuthCookie(username, false);
                    Response.Redirect("~/Driver/ClintonModule/DriverDashboard.aspx");
                    return;

                }
                reader.Close();

                // If no matches found
                lblMessage.Text = "Invalid login. Please try again.";
           
            }
         
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            // Clear all input fields
            txtUsername.Text = "";
            txtPassword.Text = "";
            lblMessage.Text = "";
        }
    }
}