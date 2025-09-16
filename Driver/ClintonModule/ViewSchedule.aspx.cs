using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace NMU_BookTrade.Driver.ClintonModule
{
    public partial class ViewSchedule : System.Web.UI.Page
    {
        // Properties
        private DateTime CurrentWeekStart
        {
            get => ViewState["CurrentWeekStart"] as DateTime? ?? GetMondayOfCurrentWeek();
            set => ViewState["CurrentWeekStart"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!IsDriverAuthenticated())
                {
                    Response.Redirect("~/User Management/Login.aspx");
                    return;
                }

                ClearErrorMessage();
                CurrentWeekStart = GetMondayOfCurrentWeek();
                LoadWeekDates();
                BindScheduleData();
            }
        }

        private bool IsDriverAuthenticated()
        {
            return Session["AccessID"] != null &&
                   Session["AccessID"].ToString() == "4" &&
                   Session["DriverID"] != null;
        }

        private DateTime GetMondayOfCurrentWeek()
        {
            DateTime today = DateTime.Today;
            int daysToSubtract = (int)today.DayOfWeek - (int)DayOfWeek.Monday;
            return today.AddDays(-daysToSubtract);
        }

        private void ClearErrorMessage()
        {
            lblErrorMessage.Visible = false;
            lblErrorMessage.Text = string.Empty;
        }

        private void LoadWeekDates()
        {
            DateTime monday = CurrentWeekStart;
            lblWeekRange.Text = $"{monday:MMM dd, yyyy} - {monday.AddDays(6):MMM dd, yyyy}";
        }

        private void BindScheduleData()
        {
            try
            {
                DataTable deliveries = GetWeekDeliveries();

                if (deliveries.Rows.Count == 0)
                {
                    ShowErrorMessage("No deliveries scheduled for this week.");
                    return;
                }

                var scheduleData = deliveries.AsEnumerable()
                    .GroupBy(r => new {
                        Day = r.Field<string>("Day"),
                        Date = r.Field<DateTime>("deliveryDate").Date
                    })
                    .Select(g => new {
                        DayName = g.Key.Day,
                        Date = g.Key.Date,
                        TimeSlots = g.GroupBy(r => r.Field<string>("TimeSlot"))
                                    .Select(ts => new {
                                        TimeSlot = ts.Key,
                                        Deliveries = ts.CopyToDataTable()
                                    })
                    }).ToList();

                rptDays.DataSource = scheduleData;
                rptDays.DataBind();
            }
            catch (Exception ex)
            {
                LogError($"Error binding schedule: {ex.Message}");
                ShowErrorMessage("Unable to load schedule data. Please try again later.");
            }
        }

        private DataTable GetWeekDeliveries()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("GetDeliveriesForSchedule", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", CurrentWeekStart);
                        cmd.Parameters.AddWithValue("@EndDate", CurrentWeekStart.AddDays(7));
                        
                        // Always filter by the logged-in driver
                        cmd.Parameters.AddWithValue("@DriverID", Session["DriverID"]);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error loading deliveries: {ex.Message}");
            }

            return dt;
        }

        protected void rptDays_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var rptTimeSlots = (Repeater)e.Item.FindControl("rptTimeSlots");
                var timeSlots = DataBinder.Eval(e.Item.DataItem, "TimeSlots");

                rptTimeSlots.DataSource = timeSlots;
                rptTimeSlots.DataBind();
            }
        }

        protected void rptTimeSlots_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var rptDeliveries = (Repeater)e.Item.FindControl("rptDeliveries");
                var deliveries = DataBinder.Eval(e.Item.DataItem, "Deliveries");

                rptDeliveries.DataSource = deliveries;
                rptDeliveries.DataBind();
            }
        }

        protected string GetDeliveryCardStyle(object status)
        {
            if (status == null) return "background-color: #F5F5F5; border-left: 4px solid #9E9E9E;";

            string statusText = status.ToString().ToLower();

            switch (statusText)
            {
                case "pending": return "background-color: #FFF3E0; border-left: 4px solid #FFA000;";
                case "assigned": return "background-color: #E3F2FD; border-left: 4px solid #1976D2;";
                case "in transit": return "background-color: #E8F5E9; border-left: 4px solid #388E3C;";
                case "completed": return "background-color: #F1F8E9; border-left: 4px solid #689F38;";
                case "failed": return "background-color: #FFEBEE; border-left: 4px solid #D32F2F;";
                case "cancelled": return "background-color: #F5F5F5; border-left: 4px solid #757575;";
                default: return "background-color: #F5F5F5; border-left: 4px solid #9E9E9E;";
            }
        }

        protected void lnkPrevWeek_Click(object sender, EventArgs e)
        {
            CurrentWeekStart = CurrentWeekStart.AddDays(-7);
            LoadWeekDates();
            BindScheduleData();
        }

        protected void lnkNextWeek_Click(object sender, EventArgs e)
        {
            CurrentWeekStart = CurrentWeekStart.AddDays(7);
            LoadWeekDates();
            BindScheduleData();
        }

        protected void btnPrintSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable deliveries = GetWeekDeliveries();
                if (deliveries.Rows.Count == 0)
                {
                    ShowErrorMessage("No deliveries to include in PDF");
                    return;
                }

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", $"attachment; filename=schedule_{CurrentWeekStart:yyyyMMdd}.pdf");

                using (Document doc = new Document(PageSize.A4, 36f, 36f, 54f, 36f))
                {
                    PdfWriter.GetInstance(doc, Response.OutputStream);
                    doc.Open();

                    // Fonts/colors
                    BaseColor primary = new BaseColor(2, 53, 60);
                    var fontTitle = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, primary);
                    var fontSub = FontFactory.GetFont(FontFactory.HELVETICA, 11, BaseColor.BLACK);
                    var fontHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
                    var fontCell = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.BLACK);

                    Paragraph title = new Paragraph("My Delivery Schedule", fontTitle);
                    title.Alignment = Element.ALIGN_LEFT;
                    doc.Add(title);
                    doc.Add(new Paragraph($"Week: {CurrentWeekStart:MMM dd, yyyy} - {CurrentWeekStart.AddDays(6):MMM dd, yyyy}", fontSub));
                    doc.Add(new Paragraph(" "));

                    PdfPTable table = new PdfPTable(7) { WidthPercentage = 100 };
                    table.SetWidths(new float[] { 1.1f, 1.1f, 0.8f, 2.0f, 2.0f, 1.2f, 1.4f });

                    void AddHeader(string text)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(text, fontHeader))
                        {
                            BackgroundColor = primary,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 6f
                        };
                        table.AddCell(cell);
                    }

                    AddHeader("Day");
                    AddHeader("Time Slot");
                    AddHeader("ID");
                    AddHeader("Book");
                    AddHeader("Location");
                    AddHeader("Status");
                    AddHeader("Date/Time");

                    foreach (DataRow row in deliveries.AsEnumerable().OrderBy(r => r.Field<DateTime>("deliveryDate")))
                    {
                        void AddCell(string text)
                        {
                            PdfPCell c = new PdfPCell(new Phrase(text ?? string.Empty, fontCell))
                            {
                                Padding = 5f,
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                VerticalAlignment = Element.ALIGN_MIDDLE
                            };
                            table.AddCell(c);
                        }

                        AddCell(row["Day"].ToString());
                        AddCell(row["TimeSlot"].ToString());
                        AddCell(row["DeliveryID"].ToString());
                        AddCell(row["BookTitle"].ToString());
                        AddCell(row["Location"].ToString());
                        AddCell(row["Status"].ToString());
                        AddCell(((DateTime)row["deliveryDate"]).ToString("yyyy-MM-dd HH:mm"));
                    }

                    doc.Add(table);
                    doc.Close();
                }

                Response.End();
            }
            catch (Exception ex)
            {
                LogError($"Error generating PDF: {ex.Message}");
                ShowErrorMessage("Failed to generate PDF. Please try again.");
            }
        }

        protected void btnExportCSV_Click(object sender, EventArgs e)
        {
            DataTable deliveries = GetWeekDeliveries();

            if (deliveries.Rows.Count == 0)
            {
                ShowErrorMessage("No deliveries to export");
                return;
            }

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", $"attachment;filename=my_deliveries_{CurrentWeekStart:yyyyMMdd}.csv");

            // Write headers
            Response.Write("DeliveryID,BookTitle,Location,Status,Day,TimeSlot,DeliveryDate\n");

            // Write data
            foreach (DataRow row in deliveries.Rows)
            {
                Response.Write(
                    $"{EscapeCsv(row["DeliveryID"].ToString())}," +
                    $"{EscapeCsv(row["BookTitle"].ToString())}," +
                    $"{EscapeCsv(row["Location"].ToString())}," +
                    $"{EscapeCsv(row["Status"].ToString())}," +
                    $"{EscapeCsv(row["Day"].ToString())}," +
                    $"{EscapeCsv(row["TimeSlot"].ToString())}," +
                    $"{EscapeCsv(((DateTime)row["deliveryDate"]).ToString("yyyy-MM-dd HH:mm"))}\n"
                );
            }

            Response.End();
        }

        private string EscapeCsv(string value)
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        private void ShowErrorMessage(string message)
        {
            lblErrorMessage.Text = message;
            lblErrorMessage.Visible = true;
        }

        private void LogError(string message)
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}] ERROR: {message}");
        }
    }
}