using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.Runtime.Remoting.Messaging;

namespace NMU_BookTrade
{
    public partial class SearchTextBook : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.QueryString["q"] != null)
            {
                string query = Request.QueryString["q"];
                txtSearch.Text = query;
                SearchBooks(query);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            SearchBooks(keyword);
        }

        private void SearchBooks(string keyword)
        {
            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT Title, Author, Price FROM Book WHERE Title LIKE @keyword";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

            }

        }
        


        public class Book
        {
            public long bookISBN { get; set; }
            public string title { get; set; }
            public string author { get; set; }
            public int genreID { get; set; }

            public double price { get; set; }

            public string condition { get; set; }

            public int driverID { get; set; }

            public int sellerID { get; set; }

            public string coverImg { get; set; }

            public string pdfFile { get; set; }

            public int reviewsCount { get; set; }

        }
    }
}