using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class WebForm7 : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["buyerID"] == null)
            {
                Response.Redirect("~/User Management/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                int buyerId = Convert.ToInt32(Session["buyerID"]);

                LoadPurchases(buyerId);

                string qsIsbn = Request.QueryString["isbn"];
                if (!string.IsNullOrEmpty(qsIsbn))
                {
                    string bookISBN = qsIsbn;
                    LoadProductSummary(bookISBN);
                }

                lblFirstName.Text = GetBuyerFirstName(buyerId);

                LoadFilterOptions();
                LoadReviewHistory(buyerId);

                pnlPurchasesTab.Visible = true;
                pnlHistoryTab.Visible = false;
                pnlReviewPanel.Visible = false;

                btnShowPurchases.CssClass = "tab-btn active";
                btnShowHistory.CssClass = "tab-btn";
            }
        }

        private string GetBuyerFirstName(int buyerId)
        {
            using (var con = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("SELECT buyerName FROM Buyer WHERE buyerID = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", buyerId);
                con.Open();
                object o = cmd.ExecuteScalar();
                return o == null ? "" : o.ToString();
            }
        }

        private void LoadPurchases(int buyerId)
        {
            var items = new List<dynamic>();
            using (var con = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(@"
                SELECT s.bookISBN, MAX(s.saleDate) AS lastPurchased,
                       b.title, b.coverImage
                FROM Sale s
                JOIN Book b ON b.bookISBN = s.bookISBN
                WHERE s.buyerID = @buyerID
                GROUP BY s.bookISBN, b.title, b.coverImage
                ORDER BY lastPurchased DESC;", con))
            {
                cmd.Parameters.AddWithValue("@buyerID", buyerId);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        items.Add(new
                        {
                            bookISBN = r["bookISBN"].ToString(),
                            title = r["title"].ToString(),
                            coverImage = r["coverImage"].ToString()
                        });
                    }
                }
            }

            rptPurchases.DataSource = items;
            rptPurchases.DataBind();
        }

        private void LoadProductSummary(string bookISBN)
        {
            pnlSummary.Visible = true;
            LoadAverageRating(bookISBN);
            LoadRatingBreakdown(bookISBN);
            LoadReviews(bookISBN);
        }
        private void LoadReviews(string bookISBN)
        {
            var reviews = new List<dynamic>();
            using (var con = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(@"
        SELECT R.reviewID, B.buyerName, R.reviewRating, R.reviewComment, R.reviewDate
        FROM Review R
        JOIN Sale S ON R.saleID = S.saleID
        JOIN Buyer B ON S.buyerID = B.buyerID
        WHERE S.bookISBN = @bookISBN
        ORDER BY R.reviewDate DESC;", con))
            {
                cmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        reviews.Add(new
                        {
                            reviewID = Convert.ToInt32(r["reviewID"]),
                            buyerName = r["buyerName"].ToString(),
                            reviewRating = Convert.ToInt32(r["reviewRating"]),
                            reviewComment = r["reviewComment"].ToString(),
                            reviewDate = Convert.ToDateTime(r["reviewDate"]),
                            Verified = true
                        });
                    }
                }
            }

            rptReviews.DataSource = reviews;
            rptReviews.DataBind();

            lblTotalReviews.Text = $"{reviews.Count} {(reviews.Count == 1 ? "Review" : "Reviews")}";
        }

        private void LoadFilterOptions()
        {
            ddlReviewFilter.Items.Clear();
            ddlReviewFilter.Items.Add(new ListItem("Last 3 months", "3m"));
            ddlReviewFilter.Items.Add(new ListItem("Last 6 months", "6m"));

            int currentYear = DateTime.Now.Year;
            for (int i = 0; i < 5; i++)
            {
                ddlReviewFilter.Items.Add(new ListItem((currentYear - i).ToString(), (currentYear - i).ToString()));
            }

            ddlReviewFilter.SelectedIndex = 0;
        }


        private void LoadReviewHistory(int buyerId)
        {
            string filter = ddlReviewFilter.SelectedValue;
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.Now;

            if (filter == "3m")
            {
                fromDate = DateTime.Now.AddMonths(-3);
            }
            else if (filter == "6m")
            {
                fromDate = DateTime.Now.AddMonths(-6);
            }
            else if (int.TryParse(filter, out int year))
            {
                fromDate = new DateTime(year, 1, 1);
                toDate = new DateTime(year, 12, 31, 23, 59, 59);
            }

            var reviews = new List<dynamic>();
            using (var con = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(@"
        SELECT R.reviewID, R.reviewRating, R.reviewComment, R.reviewDate, 
               B.title
        FROM Review R
        JOIN Sale S ON R.saleID = S.saleID
        JOIN Book B ON S.bookISBN = B.bookISBN
        WHERE S.buyerID = @buyerID
          AND R.reviewDate BETWEEN @fromDate AND @toDate
        ORDER BY R.reviewDate DESC;", con))
            {
                cmd.Parameters.AddWithValue("@buyerID", buyerId);
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                con.Open();

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        reviews.Add(new
                        {
                            reviewID = Convert.ToInt32(r["reviewID"]),
                            title = r["title"].ToString(),
                            reviewRating = Convert.ToInt32(r["reviewRating"]),
                            reviewComment = r["reviewComment"].ToString(),
                            reviewDate = Convert.ToDateTime(r["reviewDate"])
                        });
                    }
                }
            }

            rptReviewHistory.DataSource = reviews;
            rptReviewHistory.DataBind();
        }

        protected void ddlReviewFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            int buyerId = Convert.ToInt32(Session["buyerID"]);
            LoadReviewHistory(buyerId);
        }


        private void LoadAverageRating(string bookISBN)
        {
            using (var con = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(@"
                SELECT AVG(CAST(R.reviewRating AS DECIMAL(3,2))) 
                FROM Review R
                JOIN Sale S ON R.saleID = S.saleID
                WHERE S.bookISBN = @bookISBN;", con))
            {
                cmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                con.Open();
                var result = cmd.ExecuteScalar();
                lblAverageRating.Text = (result != DBNull.Value && result != null)
                    ? $"⭐ {Convert.ToDecimal(result):0.0} / 5"
                    : "No Ratings Yet";
            }
        }

        private void LoadRatingBreakdown(string bookISBN)
        {
            var dt = new DataTable();
            using (var con = new SqlConnection(connStr))
            using (var da = new SqlDataAdapter(@"
                SELECT R.reviewRating, COUNT(*) AS CountReviews
                FROM Review R
                JOIN Sale S ON R.saleID = S.saleID
                WHERE S.bookISBN = @bookISBN
                GROUP BY R.reviewRating
                ORDER BY R.reviewRating DESC;", con))
            {
                da.SelectCommand.Parameters.AddWithValue("@bookISBN", bookISBN);
                da.Fill(dt);
            }

            int total = dt.AsEnumerable().Sum(r => r.Field<int>("CountReviews"));
            var breakdown = dt.AsEnumerable().Select(r => new
            {
                reviewRating = r.Field<int>("reviewRating"),
                CountReviews = r.Field<int>("CountReviews"),
                percentage = total > 0 ? (int)(r.Field<int>("CountReviews") * 100.0 / total) : 0
            });

            rptBreakdown.DataSource = breakdown;
            rptBreakdown.DataBind();
        }

        protected void btnWriteReview_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string bookISBN = btn.CommandArgument;

            hfBookISBN.Value = bookISBN;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                con.Open();
                string query = "SELECT title, coverImage FROM Book WHERE bookISBN = @ISBN";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ISBN", bookISBN);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblProductName.Text = reader["title"].ToString();
                        imgProduct.ImageUrl = reader["coverImage"].ToString();
                    }
                }
            }

            int buyerId = Convert.ToInt32(Session["buyerID"]);
            lblFirstName.Text = GetBuyerFirstName(buyerId);

            pnlReviewPanel.Visible = true;
            pnlReviewPanel.CssClass = "slide panel slide-panel-visible";
            pnlPurchasesTab.Visible = false;
            pnlHistoryTab.Visible = false;
        }


        protected void btnSubmitReview_Click(object sender, EventArgs e)
        {
            if (Session["buyerID"] == null || string.IsNullOrEmpty(hfBookISBN.Value))
                return;

            int buyerId = Convert.ToInt32(Session["buyerID"]);
            string bookISBN = hfBookISBN.Value;
            int rating = int.Parse(ddlRating.SelectedValue);
            string comment = txtReviewComment.Text?.Trim();

            using (var con = new SqlConnection(connStr))
            {
                con.Open();

                using (var tx = con.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        int saleID;
                        using (var findSale = new SqlCommand(@"
                    SELECT TOP 1 saleID
                    FROM Sale
                    WHERE buyerID = @buyer AND bookISBN = @bookISBN
                    ORDER BY saleDate DESC;", con, tx))
                        {
                            findSale.Parameters.Add("@buyer", SqlDbType.Int).Value = buyerId;
                            findSale.Parameters.Add("@bookISBN", SqlDbType.VarChar, 32).Value = bookISBN;
                            object o = findSale.ExecuteScalar();
                            if (o == null) throw new Exception("No matching purchase found for this product.");
                            saleID = Convert.ToInt32(o);
                        }

                        int nextReviewID;
                        using (var getNext = new SqlCommand(@"
                    SELECT ISNULL(MAX(reviewID), 0) + 1
                    FROM Review WITH (UPDLOCK, HOLDLOCK);", con, tx))
                        {
                            nextReviewID = Convert.ToInt32(getNext.ExecuteScalar());
                        }

                        using (var insert = new SqlCommand(@"
                    INSERT INTO Review (reviewID, reviewRating, reviewComment, saleID, reviewDate)
                    VALUES (@id, @rating, @comment, @saleID, GETDATE());", con, tx))
                        {
                            insert.Parameters.Add("@id", SqlDbType.Int).Value = nextReviewID;
                            insert.Parameters.Add("@rating", SqlDbType.Int).Value = rating;
                            insert.Parameters.Add("@comment", SqlDbType.NVarChar, -1).Value = (object)comment ?? DBNull.Value;
                            insert.Parameters.Add("@saleID", SqlDbType.Int).Value = saleID;
                            insert.ExecuteNonQuery();
                        }

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }

            LoadProductSummary(bookISBN);
            LoadReviewHistory(buyerId);

            txtReviewComment.Text = string.Empty;
            ddlRating.SelectedValue = "5";
        }

        protected void btnDeleteReview_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int reviewID = Convert.ToInt32(btn.CommandArgument);

            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "DELETE FROM Review WHERE reviewID = @reviewID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@reviewID", reviewID);
                    cmd.ExecuteNonQuery();
                }
            }

            int buyerId = Convert.ToInt32(Session["buyerID"]);
            LoadReviewHistory(buyerId);
        }

        protected void btnShowPurchases_Click(object sender, EventArgs e)
        {
            pnlPurchasesTab.Visible = true;
            pnlHistoryTab.Visible = false;
            pnlReviewPanel.Visible = false;

            btnShowPurchases.CssClass = "tab-btn active";
            btnShowHistory.CssClass = "tab-btn";
        }

        protected void btnShowHistory_Click(object sender, EventArgs e)
        {
            pnlPurchasesTab.Visible = false;
            pnlHistoryTab.Visible = true;
            pnlReviewPanel.Visible = false;

            btnShowPurchases.CssClass = "tab-btn";
            btnShowHistory.CssClass = "tab-btn active";

            int buyerId = Convert.ToInt32(Session["buyerID"]);
            LoadReviewHistory(buyerId);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            // Clear all input fields
            txtReviewComment.Text = "";
        }

    }
}
