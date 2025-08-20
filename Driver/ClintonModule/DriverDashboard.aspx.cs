using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

namespace NMU_BookTrade.Driver.ClintonModule
{
    public partial class DriverDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Validate session before proceeding
                if (!IsDriverAuthenticated())
                {
                    Response.Redirect("~/UserManagement/Login.aspx");
                    return;
                }

                try
                    {
                    LoadDriverData();
                    LoadPendingDeliveries();
                    CheckDatabaseStatus(); // Check database status for debugging
                    SetActiveTab(tabPending);
                }
                catch (SqlException)
                {
                    lblErrorMessage.Text = "Database error occurred. Please try again later.";
                    // Log the error: LogError(sqlEx);
                }
                catch (Exception)
                {
                    lblErrorMessage.Text = "An unexpected error occurred. Please contact support.";
                    // Log the error: LogError(ex);
                }
            }
        }

        private bool IsDriverAuthenticated()
        {
            return Session["AccessID"] != null &&
                   Session["AccessID"].ToString() == "4" &&
                   Session["DriverID"] != null;
        }

        private void LoadDriverData()
        {
            string driverId = Session["DriverID"].ToString();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
                {
                    connection.Open();

                    // Get driver info
                    string driverQuery = "SELECT driverName, driverSurname FROM Driver WHERE driverID = @DriverID";
                    using (SqlCommand cmd = new SqlCommand(driverQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@DriverID", driverId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblDriverName.Text = $"{reader["driverName"]} {reader["driverSurname"]}";
                            }
                            else
                            {
                                lblErrorMessage.Text = "Driver information not found.";
                                lblErrorMessage.Visible = true;
                            }
                        }
                    }

                    // Get current date for today's calculations
                    DateTime today = DateTime.Today;
                    DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
                    DateTime endOfWeek = startOfWeek.AddDays(7);

                    // Total deliveries (all time)
                    string totalQuery = "SELECT COUNT(*) FROM Delivery WHERE driverID = @DriverID";
                    using (SqlCommand cmd = new SqlCommand(totalQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@DriverID", driverId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        lblTotalDeliveries.Text = count.ToString();
                    }

                    // Assigned deliveries (status = 1)
                    string assignedQuery = "SELECT COUNT(*) FROM Delivery WHERE driverID = @DriverID AND status = 1";
                    using (SqlCommand cmd = new SqlCommand(assignedQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@DriverID", driverId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        lblAssignedDeliveries.Text = count.ToString();
                    }

                    // Pending deliveries (status = 0)
                    string pendingQuery = "SELECT COUNT(*) FROM Delivery WHERE driverID = @DriverID AND status = 0";
                    using (SqlCommand cmd = new SqlCommand(pendingQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@DriverID", driverId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        lblPendingDeliveries.Text = count.ToString();
                    }

                    // In Transit deliveries (status = 2)
                    string transitQuery = "SELECT COUNT(*) FROM Delivery WHERE driverID = @DriverID AND status = 2";
                    using (SqlCommand cmd = new SqlCommand(transitQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@DriverID", driverId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        lblInTransitDeliveries.Text = count.ToString();
                    }

                    // Completed deliveries today
                    string completedTodayQuery = @"SELECT COUNT(*) FROM Delivery 
                                                WHERE driverID = @DriverID AND status = 3
                                                AND CONVERT(DATE, deliveryDate) = @Today";
                    using (SqlCommand cmd = new SqlCommand(completedTodayQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@DriverID", driverId);
                        cmd.Parameters.AddWithValue("@Today", today);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        lblCompletedDeliveries.Text = count.ToString();
                    }

                    // Completed deliveries this week
                    string completedWeekQuery = @"SELECT COUNT(*) FROM Delivery 
                                                WHERE driverID = @DriverID AND status = 3
                                                AND deliveryDate >= @StartOfWeek AND deliveryDate < @EndOfWeek";
                    using (SqlCommand cmd = new SqlCommand(completedWeekQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@DriverID", driverId);
                        cmd.Parameters.AddWithValue("@StartOfWeek", startOfWeek);
                        cmd.Parameters.AddWithValue("@EndOfWeek", endOfWeek);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        lblCompletedThisWeek.Text = count.ToString();
                    }

                    // Failed deliveries (status = 4)
                    string failedQuery = "SELECT COUNT(*) FROM Delivery WHERE driverID = @DriverID AND status = 4";
                    using (SqlCommand cmd = new SqlCommand(failedQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@DriverID", driverId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        lblFailedDeliveries.Text = count.ToString();
                    }

                    // Cancelled deliveries (status = 5)
                    string cancelledQuery = "SELECT COUNT(*) FROM Delivery WHERE driverID = @DriverID AND status = 5";
                    using (SqlCommand cmd = new SqlCommand(cancelledQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@DriverID", driverId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        lblCancelledDeliveries.Text = count.ToString();
                    }

                    // Calculate average rating
                    string ratingQuery = @"SELECT AVG(CAST(r.reviewRating AS FLOAT)) 
                                         FROM Review r
                                         JOIN Sale s ON r.saleID = s.saleID
                                         JOIN Delivery d ON s.saleID = d.deliveryID
                                         WHERE d.driverID = @DriverID";
                    using (SqlCommand cmd = new SqlCommand(ratingQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@DriverID", driverId);
                        object result = cmd.ExecuteScalar();
                        double avgRating = result != DBNull.Value ? Convert.ToDouble(result) : 0.0;
                        lblDriverRating.Text = avgRating.ToString("0.0");
                    }

                    // Clear any previous error messages
                    lblErrorMessage.Text = "";
                    lblErrorMessage.Visible = false;
                }
            }
            catch (SqlException)
            {
                lblErrorMessage.Text = $"Database error";
                lblErrorMessage.Visible = true;
                // Log the error: LogError(sqlEx);
            }
            catch (Exception)
            {
                lblErrorMessage.Text = $"Error loading driver data";
                lblErrorMessage.Visible = true;
                // Log the error: LogError(ex);
            }
        }

        private void LoadPendingDeliveries()
        {
            string driverId = Session["DriverID"].ToString();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                connection.Open();

                string query = @"SELECT d.deliveryID, b.title AS BookTitle, 
                                seller.sellerName AS SellerName, buyer.buyerName AS BuyerName,
                                seller.sellerAddress AS PickupAddress, buyer.buyerAddress AS DeliveryAddress,
                                d.deliveryDate
                                FROM Delivery d
                                JOIN Sale s ON d.deliveryID = s.saleID
                                JOIN Book b ON s.bookISBN = b.bookISBN
                                JOIN Seller seller ON b.sellerID = seller.sellerID
                                JOIN Buyer buyer ON s.buyerID = buyer.buyerID
                                WHERE d.driverID = @DriverID AND d.status = 1
                                ORDER BY d.deliveryDate";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@DriverID", driverId);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            rptPendingDeliveries.DataSource = dt;
                            rptPendingDeliveries.DataBind();
                            lblNoPending.Visible = false;
                        }
                        else
                        {
                            rptPendingDeliveries.DataSource = null;
                            rptPendingDeliveries.DataBind();
                            lblNoPending.Visible = true;
                        }
                    }
                }
            }
        }

        private void LoadCompletedDeliveries()
        {
            string driverId = Session["DriverID"].ToString();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                connection.Open();

                string query = @"SELECT d.deliveryID, b.title AS BookTitle, 
                                seller.sellerName AS SellerName, buyer.buyerName AS BuyerName,
                                d.deliveryDate AS CompletedDate,
                                DATEDIFF(MINUTE, d.startTime, d.endTime) AS DeliveryMinutes,
                                r.reviewRating, r.reviewComment
                                FROM Delivery d
                                JOIN Sale s ON d.deliveryID = s.saleID
                                JOIN Book b ON s.bookISBN = b.bookISBN
                                JOIN Seller seller ON b.sellerID = seller.sellerID
                                JOIN Buyer buyer ON s.buyerID = buyer.buyerID
                                LEFT JOIN Review r ON s.saleID = r.saleID
                                WHERE d.driverID = @DriverID AND d.status = 3
                                ORDER BY d.deliveryDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@DriverID", driverId);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Add formatted duration column
                        dt.Columns.Add("DeliveryDuration", typeof(string));
                        foreach (DataRow row in dt.Rows)
                        {
                            int minutes = row["DeliveryMinutes"] != DBNull.Value ? Convert.ToInt32(row["DeliveryMinutes"]) : 0;
                            row["DeliveryDuration"] = FormatDuration(minutes);
                        }

                        if (dt.Rows.Count > 0)
                        {
                            rptCompletedDeliveries.DataSource = dt;
                            rptCompletedDeliveries.DataBind();
                            lblNoCompleted.Visible = false;
                        }
                        else
                        {
                            rptCompletedDeliveries.DataSource = null;
                            rptCompletedDeliveries.DataBind();
                            lblNoCompleted.Visible = true;
                        }
                    }
                }
            }
        }

        // Fix for the CS1061 error
        private void LoadWeeklySchedule()
        {
            string driverId = Session["DriverID"].ToString();
            DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(7);

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                connection.Open();

                string query = @"SELECT d.deliveryID, b.title AS BookTitle, 
                                buyer.buyerAddress AS Location, d.deliveryDate
                                FROM Delivery d
                                JOIN Sale s ON d.deliveryID = s.saleID
                                JOIN Book b ON s.bookISBN = b.bookISBN
                                JOIN Buyer buyer ON s.buyerID = buyer.buyerID
                                WHERE d.driverID = @DriverID
                                AND d.deliveryDate >= @StartDate
                                AND d.deliveryDate < @EndDate
                                ORDER BY d.deliveryDate";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@DriverID", driverId);
                    cmd.Parameters.AddWithValue("@StartDate", startOfWeek);
                    cmd.Parameters.AddWithValue("@EndDate", endOfWeek);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Convert DataTable to a list of dynamic objects
                        var scheduleByDay = dt.Rows.Cast<DataRow>()
                            .GroupBy(r => ((DateTime)r["deliveryDate"]).DayOfWeek)
                            .Select(g => new
                            {
                                Day = g.Key.ToString(),
                                Deliveries = g.Select(r => new
                                {
                                    Time = ((DateTime)r["deliveryDate"]).ToString("HH:mm"),
                                    BookTitle = r["BookTitle"].ToString(),
                                    Location = r["Location"].ToString()
                                }).ToList()
                            }).ToList();

                        if (scheduleByDay.Count > 0)
                        {
                            rptWeeklySchedule.DataSource = scheduleByDay;
                            rptWeeklySchedule.DataBind();
                            lblNoSchedule.Visible = false;
                        }
                        else
                        {
                            rptWeeklySchedule.DataSource = null;
                            rptWeeklySchedule.DataBind();
                            lblNoSchedule.Visible = true;
                        }
                    }
                }
            }
        }

        private string FormatDuration(int minutes)
        {
            if (minutes >= 60)
                return $"{(minutes / 60)}h {(minutes % 60)}m";
            return $"{minutes}m";
        }

        protected void tabPending_Click(object sender, EventArgs e)
        {
            mvDriverContent.ActiveViewIndex = 0;
            LoadPendingDeliveries();
            LoadDriverData(); // Refresh summary data
            SetActiveTab(tabPending);
        }

        protected void tabCompleted_Click(object sender, EventArgs e)
        {
            mvDriverContent.ActiveViewIndex = 1;
            LoadCompletedDeliveries();
            LoadDriverData(); // Refresh summary data
            SetActiveTab(tabCompleted);
        }

        protected void tabSchedule_Click(object sender, EventArgs e)
        {
            mvDriverContent.ActiveViewIndex = 2;
            LoadWeeklySchedule();
            LoadDriverData(); // Refresh summary data
            SetActiveTab(tabSchedule);
        }

        private void SetActiveTab(LinkButton activeTab)
        {
            tabPending.CssClass = "dd-tab" + (tabPending == activeTab ? " active" : "");
            tabCompleted.CssClass = "dd-tab" + (tabCompleted == activeTab ? " active" : "");
            tabSchedule.CssClass = "dd-tab" + (tabSchedule == activeTab ? " active" : "");
        }

        protected void btnStartDelivery_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = (Button)sender;
                if (!int.TryParse(btn.CommandArgument, out int deliveryId))
                {
                    lblErrorMessage.Text = "Invalid delivery ID.";
                    return;
                }

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
                {
                    connection.Open();

                    string updateQuery = @"UPDATE Delivery 
                                          SET status = 2, startTime = @StartTime 
                                          WHERE deliveryID = @DeliveryID AND driverID = @DriverID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@DeliveryID", deliveryId);
                        cmd.Parameters.AddWithValue("@StartTime", DateTime.Now);
                        cmd.Parameters.AddWithValue("@DriverID", Session["DriverID"].ToString());

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Refresh the summary data after status change
                            LoadDriverData();
                            LoadPendingDeliveries();
                            Response.Redirect($"~/Driver/ClintonModule/DeliveryTracking.aspx?id={deliveryId}");
                        }
                        else
                        {
                            lblErrorMessage.Text = "Delivery could not be started. It may have been assigned to another driver.";
                        }
                    }
                }
            }
            catch (Exception)
            {
                lblErrorMessage.Text = "Error starting delivery. Please try again.";
                // Log the error: LogError(ex);
            }
        }

        protected void btnViewDetails_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            string deliveryId = btn.CommandArgument;
            Response.Redirect($"~/Driver/ClintonModule/DeliveryDetails.aspx?id={deliveryId}");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/UserManagement/Login.aspx");
        }

        protected void btnRefreshSummary_Click(object sender, EventArgs e)
        {
            try
            {
                LoadDriverData();
                LoadPendingDeliveries();
                // Clear any error messages
                lblErrorMessage.Text = "";
                lblErrorMessage.Visible = false;
            }
            catch (Exception)
            {
                lblErrorMessage.Text = "Error refreshing summary data. Please try again.";
                lblErrorMessage.Visible = true;
                // Log the error: LogError(ex);
            }
        }

        protected void rptPendingDeliveries_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // Additional formatting if needed
        }

        protected void rptCompletedDeliveries_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                Label lblRating = (Label)e.Item.FindControl("lblRating");

                if (row["reviewRating"] != DBNull.Value)
                {
                    lblRating.Text = GetStarRating(row["reviewRating"]);
                }
                else
                {
                    lblRating.Text = "Not rated";
                }
            }
        }

        protected void rptWeeklySchedule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var dayData = (dynamic)e.Item.DataItem;
                var dayDeliveries = (Repeater)e.Item.FindControl("rptDayDeliveries");
                dayDeliveries.DataSource = dayData.Deliveries;
                dayDeliveries.DataBind();
            }
        }

        public string GetStarRating(object rating)
        {
            if (rating == null || Convert.IsDBNull(rating))
                return "Not rated";

            int stars = Convert.ToInt32(rating);
            return new string('★', stars) + new string('☆', 5 - stars);
        }

        private void RefreshSummaryData()
        {
            LoadDriverData();
            LoadPendingDeliveries();
        }

        private void CheckDatabaseStatus()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
                {
                    connection.Open();
                    
                    // Check if Delivery table exists and has data
                    string checkDeliveryQuery = "SELECT COUNT(*) FROM Delivery";
                    using (SqlCommand cmd = new SqlCommand(checkDeliveryQuery, connection))
                    {
                        int deliveryCount = Convert.ToInt32(cmd.ExecuteScalar());
                        if (deliveryCount == 0)
                        {
                            lblErrorMessage.Text = "No deliveries found in database. Dashboard summary will show 0 for all counts.";
                            lblErrorMessage.Visible = true;
                        }
                    }

                    // Check if Driver table exists and has the current driver
                    string checkDriverQuery = "SELECT COUNT(*) FROM Driver WHERE driverID = @DriverID";
                    using (SqlCommand cmd = new SqlCommand(checkDriverQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@DriverID", Session["DriverID"]);
                        int driverCount = Convert.ToInt32(cmd.ExecuteScalar());
                        if (driverCount == 0)
                        {
                            lblErrorMessage.Text = "Driver not found in database. Please contact administrator.";
                            lblErrorMessage.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = $"Database connection test failed: {ex.Message}";
                lblErrorMessage.Visible = true;
            }
        }
    }
}