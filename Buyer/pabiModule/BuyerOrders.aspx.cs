using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NMU_BookTrade.Buyer.pabiModule
{
    public partial class BuyerOrders : System.Web.UI.Page
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrderFilters();
                BindOrders();
            }
        }

        private int GetBuyerId()
        {
            if (Session["buyerID"] != null && int.TryParse(Session["buyerID"].ToString(), out int id))
                return id;

            // fallback (for testing)
            return 1;
        }

        private void LoadOrderFilters()
        {
            orderDate.Items.Clear();
            orderDate.Items.Add(new ListItem("Last 3 months", "3m"));
            orderDate.Items.Add(new ListItem("Last 6 months", "6m"));

            // Load distinct years from DB
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT DISTINCT YEAR(saleDate) AS OrderYear 
                                    FROM Sale 
                                    WHERE buyerID = @buyerID
                                    ORDER BY OrderYear DESC";
                cmd.Parameters.AddWithValue("@buyerID", GetBuyerId());

                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string year = dr["OrderYear"].ToString();
                        orderDate.Items.Add(new ListItem(year, year));
                    }
                }
            }

            // Add "All" option
            if (orderDate.Items.FindByValue("0") == null)
                orderDate.Items.Insert(0, new ListItem("All", "0"));

            orderDate.SelectedIndex = 0;
        }

        private void BindOrders()
        {
            string filter = orderDate.SelectedValue;
            DateTime fromDate = DateTime.MinValue, toDate = DateTime.Now;
            bool useDateFilter = true;

            if (filter == "3m")
            {
                fromDate = DateTime.Now.AddMonths(-3);
            }
            else if (filter == "6m")
            {
                fromDate = DateTime.Now.AddMonths(-6);
            }
            else if (filter != "0" && int.TryParse(filter, out int year))
            {
                fromDate = new DateTime(year, 1, 1);
                toDate = new DateTime(year, 12, 31, 23, 59, 59);
            }
            else
            {
                // "All" selected
                useDateFilter = false;
            }

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
            SELECT 
                sl.saleID,
                sl.saleDate,
                b.buyerAddress,
                d.deliveryDate,
                bk.coverImage,
                bk.title,
                bk.price,
                ci.quantity,
                s.sellerName,
                s.sellerSurname
            FROM Sale sl
           INNER JOIN Buyer b ON sl.buyerID = b.buyerID
           LEFT JOIN Delivery d ON sl.saleID = d.saleID
           INNER JOIN Book bk ON sl.bookISBN = bk.bookISBN
           LEFT JOIN CartItems ci ON sl.saleID = ci.saleID
           INNER JOIN Seller s ON sl.sellerID = s.sellerID
            WHERE sl.buyerID = @buyerID
              " + (useDateFilter ? "AND sl.saleDate BETWEEN @fromDate AND @toDate" : "") + @"
            ORDER BY sl.saleDate DESC;
        ";

                cmd.Parameters.AddWithValue("@buyerID", GetBuyerId());

                if (useDateFilter)
                {
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                }

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            rptOrders.DataSource = dt;
            rptOrders.DataBind();
        }


        protected void dateSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOrders();
        }

        protected void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var drv = e.Item.DataItem as DataRowView;
                if (drv == null) return;

                var btnTrack = e.Item.FindControl("btnTrack") as Button;
                if (btnTrack != null)
                {
                    bool hasDeliveryDate = drv["deliveryDate"] != DBNull.Value;
                    btnTrack.Visible = !hasDeliveryDate;
                }

                var pnl = e.Item.FindControl("pnlDetails") as Panel;
                if (pnl != null)
                {
                    string currentSaleId = drv["saleID"].ToString();
                    if (ViewState["ExpandedSaleID"] != null && ViewState["ExpandedSaleID"].ToString() == currentSaleId)
                        pnl.CssClass = "order-details";
                    else
                        pnl.CssClass = "order-details hidden-panel";
                }
            }
        }

        protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ToggleDetails")
            {
                string saleId = e.CommandArgument.ToString();
                if (ViewState["ExpandedSaleID"] != null && ViewState["ExpandedSaleID"].ToString() == saleId)
                    ViewState["ExpandedSaleID"] = null;
                else
                    ViewState["ExpandedSaleID"] = saleId;

                BindOrders();
            }
            else if (e.CommandName == "Track")
            {
                string saleId = e.CommandArgument?.ToString();
                if (!string.IsNullOrEmpty(saleId))
                    Response.Redirect("~/TrackDelivery.aspx?saleID=" + saleId);
            }
        }
    }
}