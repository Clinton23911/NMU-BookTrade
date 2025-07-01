using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace NMU_BookTrade.Admin.GraceModule
{
    public partial class DeleteDriver : System.Web.UI.Page
    {
        protected void btnAddDriver_Click(object sender, EventArgs e)
        {
            // Validate the page to make sure all fields are filled
            if (!Page.IsValid)
            {
                return; // Stop if validation fails
            }

            // Collect input from the form
            string name = txtName.Text.Trim();
            string surname = txtSurname.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim(); // Consider hashing password in production
            string fileName = "";

            // Handle image upload
            if (fuImage.HasFile)
            {
                try
                {
                    fileName = Path.GetFileName(fuImage.FileName);
                    string filePath = Server.MapPath("~/UploadedImages/" + fileName);
                    fuImage.SaveAs(filePath);
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Image upload failed: " + ex.Message;
                    return;
                }
            }

            // Insert data into the database
            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ToString();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO Driver (driverName, driverSurname, driverEmail, driverPhoneNumber, driverUsername, driverPassword, driverProfileImage) " +
                               "VALUES (@Name, @Surname, @Email, @Phone, @Username, @Password, @Image)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Surname", surname);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Image", fileName);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                if (rows > 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Driver added successfully.";
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Failed to add driver.";
                }
            }
        }


    }
}