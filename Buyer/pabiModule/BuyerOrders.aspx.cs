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
                LoadOrderFilters();
                BindOrders();
            }
        }


        protected void btnStartShopping_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Buyer/pabiModule/SearchTextBook.aspx");
        }
        private int GetBuyerId()
        {
            if (Session["buyerID"] != null && int.TryParse(Session["buyerID"].ToString(), out int id))
                return id;

            return 1; // Fallback for testing
        }

        private void LoadOrderFilters()
        {
            orderDate.Items.Clear();
            orderDate.Items.Add(new ListItem("Last 3 months", "3m"));
            orderDate.Items.Add(new ListItem("Last 6 months", "6m"));

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
                useDateFilter = false;
            }

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        SELECT 
                            MIN(sl.saleID) AS saleID,         
                            sl.orderGroupId,
                            MIN(sl.saleDate) AS saleDate,
                            MAX(d.deliveryDate) AS deliveryDate,
                            b.buyerAddress
                        FROM Sale sl
                        INNER JOIN Buyer b ON sl.buyerID = b.buyerID
                        LEFT JOIN Delivery d ON sl.saleID = d.saleID
                        WHERE sl.buyerID = @buyerID
                          " + (useDateFilter ? "AND sl.saleDate BETWEEN @fromDate AND @toDate" : "") + @"
                        GROUP BY sl.orderGroupId, b.buyerAddress
                        ORDER BY MIN(sl.saleDate) DESC;
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

            if (dt.Rows.Count == 0)
            {
                pnlNoOrders.Visible = true;
                rptOrders.Visible = false;

                string filterText = orderDate.SelectedItem != null ? orderDate.SelectedItem.Text : "this period";
                lblNoOrdersMessage.Text = $"You have no orders in {filterText}";
            }

            else
            {
                pnlNoOrders.Visible = false;
                rptOrders.Visible = true;

                rptOrders.DataSource = dt;
                rptOrders.DataBind();

            }
        }

        protected void dateSelected_SelectedIndexChanged(object sender, EventArgs e)
        {

            ViewState["ExpandedGroupId"] = null;
            BindOrders();
        }

        protected void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var drv = e.Item.DataItem as DataRowView;
                if (drv == null)
                {
                    return;
                }

                // Handle Panel Visibility
                var pnl = e.Item.FindControl("pnlDetails") as Panel;
                if (pnl != null)
                {
                    string expandedId = ViewState["ExpandedGroupId"] as string;
                    string currentId = drv["orderGroupId"].ToString();
                    pnl.Visible = (expandedId == currentId);
                }

                // Bind Nested Repeaters
                var rptOrderItems = e.Item.FindControl("rptOrderItems") as Repeater;
                var rptBookCovers = e.Item.FindControl("rptBookCovers") as Repeater;

                if (rptOrderItems != null || rptBookCovers != null)
                {
                    Guid orderGroupId = (Guid)drv["orderGroupId"];
                    DataTable dtItems = new DataTable();

                    using (SqlConnection con = new SqlConnection(connString))
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = @"
                                SELECT 
                                    bk.bookISBN,
                                    bk.coverImage, 
                                    bk.title, 
                                    bk.author,
                                    bk.price, 
                                    CAST(sl.amount / bk.price AS INT) AS quantity,
                                    s.sellerName, 
                                    s.sellerSurname
                                FROM Sale sl
                                INNER JOIN Book bk ON sl.bookISBN = bk.bookISBN
                                INNER JOIN Seller s ON sl.sellerID = s.sellerID
                                WHERE sl.orderGroupId = @orderGroupId";
                        cmd.Parameters.AddWithValue("@orderGroupId", orderGroupId);

                        con.Open();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtItems);
                        }
                    }

                    if (dtItems.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtItems.Rows)
                        {
                            if (string.IsNullOrEmpty(row["coverImage"]?.ToString()))
                            {
                                row["coverImage"] = "~/Images/no-image.png";
                            }
                        }

                        if (rptBookCovers != null)
                        {
                            rptBookCovers.DataSource = dtItems;
                            rptBookCovers.DataBind();
                        }

                        if (rptOrderItems != null)
                        {
                            rptOrderItems.DataSource = dtItems;
                            rptOrderItems.DataBind();
                        }
                    }
                }
            }
        }




        protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ToggleDetails")
            {
                string orderGroupIdStr = e.CommandArgument.ToString();

                if (ViewState["ExpandedGroupId"] as string == orderGroupIdStr)
                {
                    ViewState["ExpandedGroupId"] = null;
                }
                else
                {
                    ViewState["ExpandedGroupId"] = orderGroupIdStr;
                }
                BindOrders();
            }

        }
    }
}