using System;
using System.Collections.Generic;
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

                if (Session["AccessID"] != null)
                {
                    MenuAnonymous.Visible = false; // Hide Anonymous if user is logged in
                    divAnonymous.Visible = false;  // Hide login/register section
                    divAuthenticated_Buyer.Visible = true; // Show cart/profile/logout section


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
                            break;
                        case "2":

                            MenuBuyer.Visible = true;
                            divAuthenticated_driver.Visible = false;
                            divAuthenticator_seller.Visible = false;
                            divAuthenticator_Admin.Visible = false;
                            divAnonymous.Visible = false;
                            
                            break;
                        case "3":
                            MenuSeller.Visible = true;
                            divAuthenticator_seller.Visible = true; 
                            divAnonymous.Visible = false;
                            divAuthenticated_Buyer.Visible = false;
                            divAuthenticated_Buyer.Visible = false;   // Hide cart/profile/logout by default
                            divAuthenticated_driver.Visible = false;

                            break;
                        case "4":
                            MenuDriver.Visible = true;
                            divAuthenticated_driver.Visible = true;
                            divAnonymous.Visible = false;
                            divAuthenticated_Buyer.Visible = false;
                            divAuthenticator_seller.Visible = false;

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
                }
            }
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

    }
}