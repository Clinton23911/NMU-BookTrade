//using System;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;
//using System.Web.UI.WebControls;

//namespace NMU_BookTrade
//{
//    public partial class SearchTextBook : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (!IsPostBack)
//            {
//                LoadCategories();
//                LoadOutNowTextbooks();
//                LoadRecentlyAddedTextbooks();
//            }
//        }

//        private void LoadCategories()
//        {
//            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand("SELECT DISTINCT categoryName FROM Category", con);
//                con.Open();
//                SqlDataReader reader = cmd.ExecuteReader();
//                DataTable dt = new DataTable();
//                dt.Load(reader);
//                rptCategory.DataSource = dt;
//                rptCategory.DataBind();
//            }
//        }

//        private void LoadOutNowTextbooks()
//        {
//            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand("SELECT CategoryID, CategoryName FROM Category", con);
//                con.Open();
//                SqlDataReader reader = cmd.ExecuteReader();
//                DataTable dt = new DataTable();
//                dt.Load(reader);
//                rptOutNow.DataSource = dt;
//                rptOutNow.DataBind();
//            }
//        }

//        private void LoadRecentlyAddedTextbooks()
//        {
//            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand("SELECT CategoryID, CategoryName FROM Category", con);
//                con.Open();
//                SqlDataReader reader = cmd.ExecuteReader();
//                DataTable dt = new DataTable();
//                dt.Load(reader);
//                rptRecentlyAdded.DataSource = dt;
//                rptRecentlyAdded.DataBind();
//            }
//        }

//        protected void btnSearch_Click(object sender, EventArgs e)
//        {
//            string searchTerm = txtSearch.Text.Trim();
//            lblSearchResults.Text = $"Search results for \"{searchTerm}\"";

//            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand("SELECT * FROM Book WHERE title LIKE @SearchTerm", con);
//                cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
//                con.Open();
//                SqlDataReader reader = cmd.ExecuteReader();
//                DataTable dt = new DataTable();
//                dt.Load(reader);
//                rptOutNow.DataSource = dt;  
//                rptOutNow.DataBind();
//            }
//        }

//        protected void lnkViewAllResults_Click(object sender, EventArgs e)
//        {
//            Response.Redirect("~/SearchResult.aspx?query=" + Server.UrlEncode(txtSearch.Text.Trim()));
//        }

//        protected void rptCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
//        {
//            if (e.CommandName == "SelectCategory")
//            {
//                string faculty = e.CommandArgument.ToString();
//                Response.Redirect("~/Category.aspx?category=" + Server.UrlEncode(faculty));
//            }
//        }
//    }
//}
