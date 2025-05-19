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

                if (Session["AccessID"] != null)
                {
                    MenuAnonymous.Visible = false; // Hide Anonymous if user is logged in

                    switch (Session["AccessID"].ToString())
                    {
                        case "1":
                            MenuAdmin.Visible = true;
                            break;
                        case "2":
                            MenuBuyer.Visible = true;
                            break;
                        case "3":
                            MenuSeller.Visible = true;
                            break;
                        case "4":
                            MenuDriver.Visible = true;
                            break;
                        default:
                            MenuAnonymous.Visible = true; // Fallback to Anonymous if AccessID is unexpected
                            break;
                    }
                }
            }
        }



        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Register.aspx");
        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Log In.aspx");
        }


    }
}