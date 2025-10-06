﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            
            if (!IsPostBack)
            {

                StorePreviousPage();
                // Default: Anonymous is visible, others are hidden
                MenuAnonymous.Visible = true;
                MenuAdmin.Visible = false;
                MenuBuyer.Visible = false;
                MenuSeller.Visible = false;
                MenuDriver.Visible = false;

                divAnonymous.Visible = true;        // Show login/register by default
                divAuthenticated_Buyer.Visible = false;   // Hide cart/profile/logout by default
                divAuthenticated_driver.Visible = false;
                divAuthenticator_seller.Visible = false;
                divAuthenticator_Admin.Visible = false;
                pnlProfileImage.Visible = false; // hide by default
                if (Session["AccessID"] != null)
                {
                    MenuAnonymous.Visible = false; // Hide Anonymous if user is logged in
                    divAnonymous.Visible = false;  // Hide login/register section
                    divAuthenticated_Buyer.Visible = true; // Show cart/profile/logout section
                    pnlProfileImage.Visible = true; // Show only for authenticated users

                    string accessID = Session["AccessID"].ToString();

                    string userID = Session["UserID"].ToString();

                    string profileUrl = "~/User Management/Login.aspx"; // Default fallback


                    // Load profile image
                    LoadProfileImage(accessID, userID);

                    switch (Session["AccessID"].ToString())
                    {
                        case "1":
                           
                            MenuAdmin.Visible = true;
                            divAuthenticator_Admin.Visible = true;
                            divAnonymous.Visible = false;
                            divAuthenticated_Buyer.Visible = false;
                            divAuthenticated_Buyer.Visible = false;   // Hide cart/profile/logout by default
                            divAuthenticated_driver.Visible = false;
                            divAuthenticator_seller.Visible = false;
                            profileUrl = "~/Admin/GraceModule/AdminProfile.aspx";
                            break;
                        case "2":

                            MenuBuyer.Visible = true;
                            divAuthenticated_driver.Visible = false;
                            divAuthenticator_seller.Visible = false;
                            divAuthenticator_Admin.Visible = false;
                            divAnonymous.Visible = false;
                            profileUrl = "~/Buyer/pabiModule/BuyerProfile.aspx";
                            break;
                        case "3":
                            MenuSeller.Visible = true;
                            divAuthenticator_seller.Visible = true; 
                            divAnonymous.Visible = false;
                            divAuthenticated_Buyer.Visible = false;
                            divAuthenticated_Buyer.Visible = false;   // Hide cart/profile/logout by default
                            divAuthenticated_driver.Visible = false;
                            profileUrl = "~/Seller/ClintonModule/SellerProfile.aspx";
                            break;
                        case "4":
                            MenuDriver.Visible = true;
                            divAuthenticated_driver.Visible = true;
                            divAnonymous.Visible = false;
                            divAuthenticated_Buyer.Visible = false;
                            divAuthenticator_seller.Visible = false;
                            profileUrl = "~/Driver/ClintonModule/DriverProfile.aspx";
                            break;
                        default:
                            MenuAnonymous.Visible = true; // Fallback to Anonymous if AccessID is unexpected
                            divAnonymous.Visible = true;
                            divAuthenticated_Buyer.Visible = false;
                            divAuthenticated_Buyer.Visible = false;
                            divAuthenticated_Buyer.Visible = false;
                            divAuthenticator_seller.Visible = false;
                            MenuAdmin.Visible = false;
                            MenuDriver.Visible = false;
                            MenuBuyer.Visible =false;
                            MenuSeller.Visible=false;
                            break;
                    }
                    hlDynamicProfile.NavigateUrl = ResolveUrl(profileUrl);
                    //hide search on nav when logged in //

                    divSearchBar.Visible = Session["buyerID"] == null;
                }
                
            }
            UpdateCartCount();
          
        }

        public void UpdateCartCount()
        {
            int count = 0;

            if (Session["buyerID"] != null)
            {
                int buyerID = Convert.ToInt32(Session["buyerID"]);
                string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string countQuery = "SELECT SUM(quantity) FROM CartItems ci JOIN Cart c ON ci.cartID = c.cartID WHERE c.buyerID = @buyerID";
                    using (SqlCommand cmd = new SqlCommand(countQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@buyerID", buyerID);
                        object result = cmd.ExecuteScalar();
                        count = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                        UpdatePanel updCartBadge = (UpdatePanel)FindControl("updCartBadge");
                        if (updCartBadge != null)
                        {
                            updCartBadge.Update();
                        }
                    }
                }
            }

            lblCartCount.Text = count.ToString();
            lblCartCount.Visible = count > 0; // hide if cart empty
            hlCart.ToolTip = count > 0 ? $"Cart: {count} item(s)" : "Cart is empty";

            //  Refresh UpdatePanel so badge updates without full reload
            updCartBadge.Update();
        }


        // Loads the profile image based on user role and ID
        private void LoadProfileImage(string accessID, string userID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;
            string query = "";
            string imageColumn = "";
            string imagePath = "~/Images/User.png"; // Default placeholder image

            switch (accessID)
            {
                case "1":
                    query = "SELECT adminProfileImage FROM Admin WHERE adminID = @ID";
                    imageColumn = "adminProfileImage";
                    break;
                case "2":
                    query = "SELECT buyerProfileImage FROM Buyer WHERE buyerID = @ID";
                    imageColumn = "buyerProfileImage";
                    break;
                case "3":
                    query = "SELECT sellerProfileImage FROM Seller WHERE sellerID = @ID";
                    imageColumn = "sellerProfileImage";
                    break;
                case "4":
                    query = "SELECT driverProfileImage FROM Driver WHERE driverID = @ID";
                    imageColumn = "driverProfileImage";
                    break;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ID", userID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string imageName = reader[imageColumn].ToString();
                    if (!string.IsNullOrEmpty(imageName))
                    {
                        imagePath = "~/UploadedImages/" + imageName;
                    }
                }
            }

            // Set the image source to the circular profile image in the MasterPage
            imgProfile.ImageUrl = ResolveUrl(imagePath);
            pnlProfileImage.Visible = true;


        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/User Management/Register.aspx");
        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/User Management/Login.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/User Management/Home.aspx");
        }

        protected void StorePreviousPage()


        {


            if (Request.UrlReferrer != null)


            {


                Uri referrer = Request.UrlReferrer;





                if (referrer.Host == Request.Url.Host)


                {


                    Session["PreviousPage"] = referrer.ToString();


                }


            }


        }


    }
}