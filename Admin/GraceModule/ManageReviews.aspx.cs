using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NMU_BookTrade.Admin.GraceModule
{
    public partial class ManageReviews : System.Web.UI.Page
    {// Use your existing connection string
        private readonly string _cs = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack) Bind(); // first load
        }

        private void Bind(string search = null)
        {
            // Pull recent reviews (+ buyer name); filter by comment if search provided
            const string sql = @"
                SELECT TOP 15
                       R.reviewID, R.reviewDate, R.reviewRating, R.reviewComment,
                       (B.name + ' ' + B.surname) AS BuyerName
                FROM Review R
                INNER JOIN Sale   S ON R.saleID = S.saleID
                LEFT  JOIN Buyer  B ON S.buyerID = B.buyerID
                WHERE (@q IS NULL OR R.reviewComment LIKE '%' + @q + '%')
                ORDER BY R.reviewDate DESC, R.reviewID DESC;";

            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(sql, cn))
            {
                // NULL means "no filter"; otherwise LIKE '%@q%'
                cmd.Parameters.AddWithValue("@q", string.IsNullOrWhiteSpace(search) ? (object)DBNull.Value : search.Trim());

                var dt = new DataTable();     // table to hold rows
                cn.Open();                    // open connection
                dt.Load(cmd.ExecuteReader()); // execute + load
                gvReviews.DataSource = dt;    // bind to grid
                gvReviews.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e) => Bind(txtSearch.Text);
        protected void btnClear_Click(object sender, EventArgs e) { txtSearch.Text = ""; Bind(); }

        protected void gvReviews_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvReviews.PageIndex = e.NewPageIndex; // move page
            Bind(txtSearch.Text);                 // rebind with current filter
        }

        protected void gvReviews_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Remove") return;

            int reviewId = Convert.ToInt32(e.CommandArgument); // ID from button

            // Hard delete the specific review
            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand("DELETE FROM Review WHERE reviewID=@id", cn))
            {
                cmd.Parameters.AddWithValue("@id", reviewId); // parameterized => safe
                cn.Open();
                cmd.ExecuteNonQuery();
            }

            Bind(txtSearch.Text); // refresh grid after delete
        }
    }
}