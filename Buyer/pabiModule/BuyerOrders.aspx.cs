using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class BuyerOrders : System.Web.UI.Page
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrderYears();
                BindOrders();

            }

        }

        private int GetBuyerId()
        {
            // adjust this to how you store logged-in buyer id in your app (Session, Claims, etc.)
            if (Session["buyerID"] != null && int.TryParse(Session["buyerID"].ToString(), out int id))
                return id;

            // fallback for development — replace or remove in production
            return 1;
        }

        private void LoadOrderYears()
        {
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
                    orderDate.DataSource = dr;
                    orderDate.DataTextField = "OrderYear";
                    orderDate.DataValueField = "OrderYear";
                    orderDate.DataBind();
                }
            }

            if (orderDate.Items.FindByValue("0") == null)
                orderDate.Items.Insert(0, new ListItem("All", "0"));
        }

        private void BindOrders()
        {
            int yearFilter = 0;
            if (!string.IsNullOrEmpty(orderDate.SelectedValue))
                int.TryParse(orderDate.SelectedValue, out yearFilter);

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
                      AND (@YearFilter = 0 OR YEAR(sl.saleDate) = @YearFilter)
                    ORDER BY sl.saleDate DESC;
                ";

                cmd.Parameters.AddWithValue("@buyerID", GetBuyerId());
                cmd.Parameters.AddWithValue("@YearFilter", yearFilter);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            // Bind to Repeater
            rptOrders.DataSource = dt;
            rptOrders.DataBind();
        }

        protected void dateSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            // re-bind with selected year filter
            BindOrders();
        }

        protected void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (drv == null) return;

                // Handle Track button visibility
                var btnTrack = e.Item.FindControl("btnTrack") as Button;
                if (btnTrack != null)
                {
                    bool hasDeliveryDate = drv["deliveryDate"] != DBNull.Value;
                    btnTrack.Visible = !hasDeliveryDate;
                }

                // Handle expanded/collapsed panel state
                var pnl = e.Item.FindControl("pnlDetails") as Panel;
                if (pnl != null)
                {
                    string currentSaleId = drv["saleID"].ToString();
                    if (ViewState["ExpandedSaleID"] != null && ViewState["ExpandedSaleID"].ToString() == currentSaleId)
                    {
                        pnl.CssClass = "order-details"; // Remove hidden-panel to show
                    }
                    else
                    {
                        pnl.CssClass = "order-details hidden-panel"; // Hide others
                    }
                }
            }
        }

        protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ToggleDetails")
            {
                string saleId = e.CommandArgument.ToString();
                if (ViewState["ExpandedSaleID"] != null && ViewState["ExpandedSaleID"].ToString() == saleId)
                {
                    ViewState["ExpandedSaleID"] = null; // Collapse if same order clicked again
                }
                else
                {
                    ViewState["ExpandedSaleID"] = saleId; // Expand this order
                }
                BindOrders(); // Rebind to refresh visibility
            }
            else if (e.CommandName == "Track")
            {
                string saleId = e.CommandArgument?.ToString();
                if (!string.IsNullOrEmpty(saleId))
                {
                    Response.Redirect("~/TrackDelivery.aspx?saleID=" + saleId);
                }
            }
        }

    }
}
