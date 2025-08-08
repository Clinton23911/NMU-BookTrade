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
    public partial class WebForm17 : System.Web.UI.Page
    {
        /* Convenience: connection string pulled once. */
        private readonly string connStr =
            ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();   // Populate GridView on first load only.
            }
        }


        // 2. READ – Fill GridView with current categories
      
        private void LoadCategories()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Category", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCategories.DataSource = dt;
                gvCategories.DataBind();
            }
        }

       
        // 3. CREATE – Add a new category
       
        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txtCategoryName.Text.Trim();

            if (string.IsNullOrEmpty(name)) return;         // Guard clause

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(
                "INSERT INTO Category (categoryName) VALUES (@name)", con))
            {
                cmd.Parameters.AddWithValue("@name", name);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            txtCategoryName.Text = "";  // Clear input
            LoadCategories();           // Refresh table
        }


        
        // 4. UPDATE – Begin inline edit
        
        protected void gvCategories_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCategories.EditIndex = e.NewEditIndex;   // Put row into edit mode
            LoadCategories();                          // Re-bind to show TextBox
        }

        // Save edited value
        protected void gvCategories_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = (int)gvCategories.DataKeys[e.RowIndex].Value;
            string name =
                ((TextBox)gvCategories.Rows[e.RowIndex].Cells[0].Controls[0]).Text.Trim();

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE Category SET categoryName = @name WHERE categoryID = @id", con))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvCategories.EditIndex = -1;  // Exit edit mode
            LoadCategories();
        }

        // User clicks “Cancel”
        protected void gvCategories_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCategories.EditIndex = -1;  // Exit edit mode without saving
            LoadCategories();
        }


        // 5. DELETE – Remove a category

        protected void gvCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            object key = gvCategories.DataKeys[e.RowIndex].Value;

            if (key == null || key == DBNull.Value)
            {
                // Optionally log error or show message
                return;
            }

            int id = Convert.ToInt32(key);

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("DELETE FROM Category WHERE categoryID = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadCategories();
        }

    }
}