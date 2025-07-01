using System;
using System.Collections.Generic;
using System.Web.UI;

namespace NMU_BookTrade
{
    public partial class SellerDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    if (User.Identity.IsAuthenticated)
            //    {
            //        lblUsername.Text = User.Identity.Name;
            //        LoadSellerStats();
            //        LoadRecentActivity();
            //    }
            //    else
            //    {
            //        Response.Redirect("~/Login.aspx");
            //    }
            //}
        }

        private void LoadSellerStats()
        {
            // TODO: Replace with actual database calls
            lblActiveListings.Text = "12";
            lblSoldBooks.Text = "24";
            lblAvgRating.Text = "4.8";
        }

        private void LoadRecentActivity()
        {
            // TODO: Replace with actual database data
            var recentActivities = new List<dynamic>
            {
                new { Icon = "book", Message = "You listed 'Pure Mathematics 1'", Time = "2 hours ago" },
                new { Icon = "money-bill-wave", Message = "Sale completed for 'Introduction to Economics'", Time = "1 day ago" },
                new { Icon = "star", Message = "Received a 5-star review from buyer123", Time = "2 days ago" },
                new { Icon = "truck", Message = "Driver assigned for 'Law 101' delivery", Time = "3 days ago" }
            };

            rptRecentActivity.DataSource = recentActivities;
            rptRecentActivity.DataBind();
        }
    }
}