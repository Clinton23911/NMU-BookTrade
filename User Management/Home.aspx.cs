using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBooks(); // Load books for the slider
            }
        }

        private void LoadBooks()
        {
            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;
            string query = "SELECT TOP 6 bookISBN, title, price, coverImage FROM Book ORDER BY bookISBN DESC";

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                rptBooks.DataSource = reader;
                rptBooks.DataBind();
            }
        }

        protected void Interested_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string bookISBN = btn.CommandArgument;

            // Redirect to book details page
            Response.Redirect("BookDetails.aspx?bookISBN=" + bookISBN);
        }


        private void LoadReviews()
        {
            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;
            string query = @"
                SELECT TOP 4 B.name AS BuyerName, B.surname AS BuyerSurname, B.profileImage, 
                             R.reviewRating, R.reviewComment
                FROM Review R
                INNER JOIN Sale S ON R.saleID = S.saleID
                INNER JOIN Buyer B ON S.buyerID = B.buyerID
                ORDER BY R.reviewID DESC";

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
               // rptTestimonials.DataSource = reader;
               // rptTestimonials.DataBind();
            }
        }

        public static string GetStarHtml(int rating)
        {
            string stars = "";
            for (int i = 1; i <= 5; i++)
            {
                if (i <= rating)
                    stars += "<span style='color: gold;'>&#9733;</span>"; // filled star
                else
                    stars += "<span style='color: lightgray;'>&#9734;</span>"; // empty star
            }
            return stars;
        }

        protected void btnReadStories_Click(object sender, EventArgs e)
        {
            // You can redirect or show more reviews here
            Response.Redirect("AllReviews.aspx");
        }
    }
}
