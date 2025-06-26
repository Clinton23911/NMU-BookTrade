using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
<<<<<<< HEAD
using System.Data.SqlClient;
using System.IO;

=======
using System.Data.SqlClient;          // For SQL Server operations
using System.IO;
using System.Configuration;
>>>>>>> f95f0fc (worked on Admin page, sitemaster and stylesheet.css, and fixed the LogOut page)

namespace NMU_BookTrade
{
    public partial class WebForm16 : System.Web.UI.Page
    {
        // When the page loads this methode will be first to run
        protected void Page_Load(object sender, EventArgs e)
        {
<<<<<<< HEAD
            //but first we check if the page has loaded for  the first time( not from a button click)

            if (!IsPostBack) {

                //LoadAdminProfile();
            }
=======
            // Only run the profile loading code once (not every button click)
            if (!IsPostBack)
            {
                LoadAdminProfile();  // Load current admin's data
            }
        }
>>>>>>> f95f0fc (worked on Admin page, sitemaster and stylesheet.css, and fixed the LogOut page)

        // Loads the current admin's email, username, and profile image
        private void LoadAdminProfile()
        {
            // Get admin ID from Session["UserID"] (set during login)
            int adminID = Convert.ToInt32(Session["UserID"]);

            // Grab the connection string from Web.config
            string connectionString = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

            // Connect to the database
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Query the Admin table for the current user's details
                string query = "SELECT adminEmail, adminUsername, adminProfileImage FROM Admin WHERE adminID = @adminID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@adminID", adminID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Fill the input fields with data from the database
                    txtEmail.Text = reader["adminEmail"].ToString();
                    txtUsername.Text = reader["adminUsername"].ToString();

                    // Load the profile picture, or show default image if not set
                    string imageName = reader["adminProfileImage"].ToString();
                    imgProfile.ImageUrl = string.IsNullOrEmpty(imageName)
                        ? "~/Images/default.png"
                        : "~/UploadedImages/" + imageName;
                }
            }
        }

        // Called when the user clicks the "Update Profile" button
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // Get admin ID again from the session
            int adminID = Convert.ToInt32(Session["UserID"]);

            // Get updated input values from the form
            string email = txtEmail.Text.Trim();
            string username = txtUsername.Text.Trim();

            string newImageName = ""; // Holds the new image file name if one is uploaded

            // Handle profile image upload
            if (fuProfileImage.HasFile)
            {
                string ext = Path.GetExtension(fuProfileImage.FileName).ToLower();

                // Only allow JPG, JPEG, PNG
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    lblMessage.Text = "Only JPG, JPEG, or PNG images are allowed.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                // Generate a unique file name to avoid overwriting
                newImageName = Guid.NewGuid().ToString() + ext;

                // Save the file to the server
                string path = Server.MapPath("~/UploadedImages/" + newImageName);
                fuProfileImage.SaveAs(path);
            }

            // Connect to SQL Server
            string connectionString = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query;

                // Choose SQL query depending on whether a new image is uploaded
                if (string.IsNullOrEmpty(newImageName))
                {
                    query = "UPDATE Admin SET adminEmail = @Email, adminUsername = @Username WHERE adminID = @AdminID";
                }
                else
                {
                    query = "UPDATE Admin SET adminEmail = @Email, adminUsername = @Username, adminProfileImage = @Image WHERE adminID = @AdminID";
                }

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@AdminID", adminID);

                // Only add image if one was uploaded
                if (!string.IsNullOrEmpty(newImageName))
                {
                    cmd.Parameters.AddWithValue("@Image", newImageName);
                }

                con.Open();
                cmd.ExecuteNonQuery();

                // Show success feedback
                lblMessage.Text = "Profile updated successfully!";
                lblMessage.ForeColor = System.Drawing.Color.Lime;

                // Reload the updated image and fields
                LoadAdminProfile();
            }
        }

        protected void LoadAdminProfile(object sender, EventArgs e)
        {
            // we want to get the session value and convert it into int
            int adminID = Convert.ToInt32(Session["UserID"]);  

            // now we grab the connection string from web.config

            string connectionString = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

            using (SqlConnection constring = new SqlConnection(connectionString))
            {
                // Query the admin details to check the current user's details in the database
                string query = "SELECT adminEmail, adminUsername, adminProfileImage FROM Admin WHERE adminID = @adminID";
                SqlCommand cmd = new SqlCommand(query, constring);

                cmd.Parameters.AddWithValue("@adminID",adminID);

                constring.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                // now we have the data from the database
                if (reader.Read())
                {
                    // we have to fill that into the input fields with data from the database
                    txtEmail.Text = reader["adminEmail"].ToString();
                    txtUsername.Text = reader["adminUsername"].ToString();


                    //Here we now load the picture, or show the default image if not set

                    //string imageName = reader[];
                }
                    
                
                


            }
        }


    }
}