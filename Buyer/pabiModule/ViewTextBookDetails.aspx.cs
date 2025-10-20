using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class ViewTextBookDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string isbn = Request.QueryString["isbn"];
                if (!string.IsNullOrEmpty(isbn))
                {
                    LoadBookDetails(isbn);
                    LoadBookReviews(isbn);
                }
            }
        }



        private void LoadBookDetails(string isbn)
        {
            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT b.title, b.author, b.condition, b.price, 
                                 b.coverImage, s.Name + ' ' + s.SurnameName AS sellerName,
                                 s.email, b.isAvailable
                                 FROM Books b
                                 INNER JOIN Sellers s ON b.sellerID = s.sellerID
                                 WHERE b.bookISBN = @isbn";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@isbn", isbn);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblTitle.Text = dr["title"].ToString();
                    lblAuthor.Text = dr["author"].ToString();
                    lblEdition.Text = dr["edition"].ToString();
                    lblCondition.Text = dr["condition"].ToString();
                    lblPrice.Text = string.Format("{0:N2}", dr["price"]);
                    lblSellerName.Text = dr["sellerName"].ToString();
                    lblSellerEmail.Text = dr["email"].ToString();
                    lblISBN.Text = isbn;
                    imgBookCover.ImageUrl = dr["coverImage"].ToString();

                    bool available = Convert.ToBoolean(dr["isAvailable"]);
                    lblAvailability.Text = available ? "In Stock" : "Sold Out";
                    lblAvailability.ForeColor = available ? System.Drawing.ColorTranslator.FromHtml("#388e3c") : System.Drawing.Color.Red;
                }
                con.Close();
            }
        }

        private void LoadBookReviews(string isbn)
        {
            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT buyerName, reviewComment, reviewRating, reviewDate 
                                 FROM Reviews WHERE bookISBN = @isbn ORDER BY reviewDate DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@isbn", isbn);
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptBookReviews.DataSource = dt;
                rptBookReviews.DataBind();

                if (dt.Rows.Count > 0)
                {
                    double avg = Convert.ToDouble(dt.Compute("AVG(reviewRating)", string.Empty));
                    lblAverageRating.Text = avg.ToString("0.0");
                    lblTotalReviews.Text = dt.Rows.Count.ToString();
                }
                else
                {
                    lblAverageRating.Text = "No ratings yet";
                    lblTotalReviews.Text = "0";
                }
                con.Close();
            }
        }

        protected void btnWriteReview_Click(object sender, EventArgs e)
        {
            string isbn = Request.QueryString["isbn"];
            Response.Redirect("~/Buyer/pabiModule/Reviews.aspx?isbn={isbn}");
        }
    }
}