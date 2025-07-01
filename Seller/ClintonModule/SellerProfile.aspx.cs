using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class SellerProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                LoadSellerProfile();
            }
        }

        protected void LoadSellerProfile() {

            int sellerID = Convert.ToInt32(Session["UserID"]);

            string connectionString = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT sellerUsername, sellerName, sellerSurname, sellerEmail, sellerNumber, sellerAddress, sellerProfileImage FROM Seller WHERE sellerID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", sellerID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {

                    txtUsername.Text = reader["sellerUsername"].ToString();
                    
                    txtName.Text = reader["sellerName"].ToString();
                    txtSurname.Text = reader["sellerSurname"].ToString();
                    txtEmail.Text = reader["sellerEmail"].ToString();
                    txtNumber.Text = reader["sellerNumber"].ToString();
                    txtAddress.Text = reader["sellerAddress"].ToString();

                    string imageName = reader["sellerProfileImage"].ToString();

                    imgProfile.ImageUrl = string.IsNullOrEmpty(imageName) ? "~/Images/default.png" : "~/UploadedImages/" + imageName;


                }

            }
        }



        // Now we want to update and store everything When the button update is clicked this event happens
        protected void btnUpdate_Click(object sender, EventArgs e) {

            int sellerID = Convert.ToInt32(Session["UserID"]);
            string username = txtUsername.Text.Trim();
            
            string name = txtName.Text.Trim();
            string surname = txtSurname.Text.Trim();
            string email = txtEmail.Text.Trim();
            string number = txtNumber.Text.Trim();
            string address = txtAddress.Text.Trim();
            string newImageName = "";

            if (fuProfileImage.HasFile)
            {
                string ext = Path.GetExtension(fuProfileImage.FileName).ToLower();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    lblMessage.Text = "Only JPG, JPEG, or PNG images are allowed.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                newImageName = Guid.NewGuid().ToString() + ext;
                string path = Server.MapPath("~/UploadedImages/" + newImageName);
                fuProfileImage.SaveAs(path);
            }

            string connectionString = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = string.IsNullOrEmpty(newImageName)
                    ? "UPDATE Seller SET sellerUsername=@Username, sellerName=@Name, sellerSurname=@Surname, sellerEmail=@Email, sellerNumber=@Number, sellerAddress=@Address WHERE sellerID=@ID"
                    : "UPDATE Seller SET sellerUsername=@Username, sellerName=@Name, sellerSurname=@Surname, sellerEmail=@Email, sellerNumber=@Number, sellerAddress=@Address, sellerProfileImage=@Image WHERE sellerID=@ID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
               
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Surname", surname);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Number", number);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@ID", sellerID);

                if (!string.IsNullOrEmpty(newImageName))
                    cmd.Parameters.AddWithValue("@Image", newImageName);

                con.Open();
                cmd.ExecuteNonQuery();

                lblMessage.Text = "Profile updated successfully!";
                lblMessage.ForeColor = System.Drawing.Color.Lime;
                LoadSellerProfile();

            }






        }

        // Deleting the profile/ Account from existing.   
        protected void btnDeleteProfile_Click(object sender, EventArgs e)
        {
            string sellerID = Session["sellerID"] as string;

            if (string.IsNullOrEmpty(sellerID))
            {
                lblMessage.Text = "Session expired. Please log in again.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ToString();
            string query = "DELETE FROM Seller WHERE sellerID = @SellerID";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SellerID", sellerID);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    // Clear session and redirect
                    Session.Clear();
                    Response.Redirect("~/User Management/Home.aspx");
                }
                else
                {
                    lblMessage.Text = "Failed to delete account.";
                }
            }

        }

    }
}