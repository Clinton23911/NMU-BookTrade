using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class WebForm10 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                string query = Request.QueryString["query"];
                if (!string.IsNullOrEmpty(query))
                {
                    txtSearch.Text = query;
                }

                LoadCategories();
                LoadOutNowTextbooks();
                LoadRecentlyAddedTextbooks();
            }
        }

        protected void LoadCategories()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT categoryName FROM Category", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                int splitIndex = dt.Rows.Count / 2;

                DataTable dt1 = dt.Clone();
                DataTable dt2 = dt.Clone();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i < splitIndex)
                        dt1.ImportRow(dt.Rows[i]);
                    else
                        dt2.ImportRow(dt.Rows[i]);
                }

                rptCategory1.DataSource = dt1;
                rptCategory1.DataBind();

                rptCategory2.DataSource = dt2;
                rptCategory2.DataBind();
            }
        }


        private void LoadOutNowTextbooks()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT TOP 8 coverImage FROM Book", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                rptOutNow.DataSource = dt;
                rptOutNow.DataBind();
            }
        }

        private void LoadRecentlyAddedTextbooks()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT TOP 8 title, price, coverImage FROM Book", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                rptRecentlyAdded.DataSource = dt;
                rptRecentlyAdded.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                Response.Redirect("~/Buyer/pabiModule/SearchTextBook.aspx?query=" + Server.UrlEncode(searchTerm));
            }
        }


        protected void lnkViewAllResults_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Buyer/pabiModule/SearchResult.aspx?query=" + Server.UrlEncode(txtSearch.Text.Trim()));
        }

        protected void rptCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectCategory")
            {
                string faculty = e.CommandArgument.ToString();
                Response.Redirect("~/Category.aspx?category=" + Server.UrlEncode(faculty));
            }

            if (e.Item.ItemIndex == 2)
            {
                PlaceHolder phSearchBar = (PlaceHolder)e.Item.FindControl("phSearchBar");
                if (phSearchBar != null)
                {
                    phSearchBar.Visible = true;
                }
            }
        }
    }
}
