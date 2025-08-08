using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class WebForm8 : System.Web.UI.Page
    {// Connection string from Web.config
        private readonly string connStr =
            ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGenres(); // Load all genres when the page loads
            }
        }

        // Load genre records from database
        private void LoadGenres()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Genre", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvGenres.DataSource = dt;
                gvGenres.DataBind();
            }
        }

        // Add new genre
        protected void BtnAddGenre_Click(object sender, EventArgs e)
        {
            string name = txtGenreName.Text.Trim();

            if (string.IsNullOrEmpty(name)) return;

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("INSERT INTO Genre (genreName) VALUES (@name)", con))
            {
                cmd.Parameters.AddWithValue("@name", name);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            txtGenreName.Text = "";  // Clear input field
            LoadGenres();            // Refresh the GridView
        }

        // Edit genre (start edit mode)
        protected void GvGenres_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvGenres.EditIndex = e.NewEditIndex;
            LoadGenres();
        }

        // Save updated genre
        protected void GvGenres_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvGenres.DataKeys[e.RowIndex].Value);
            string name = ((TextBox)gvGenres.Rows[e.RowIndex].Cells[0].Controls[0]).Text.Trim();

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE Genre SET genreName = @name WHERE genreID = @id", con))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvGenres.EditIndex = -1;
            LoadGenres();
        }

        // Cancel edit mode
        
        protected void GvGenres_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)

        {
            gvGenres.EditIndex = -1;
            LoadGenres();
        }

        // Delete genre
        protected void GvGenres_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvGenres.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("DELETE FROM Genre WHERE genreID = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadGenres();
        }
    }
}