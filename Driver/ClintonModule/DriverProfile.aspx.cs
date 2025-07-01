using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class DriverProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBuyerProfile();
            }
        }

        protected void LoadBuyerProfile()
        {

            int driverID = Convert.ToInt32(Session["UserID"]);

            string connectionString = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT driverUsername, driverName, driverSurname, driverEmail, driverPhoneNumber, driverProfileImage FROM Driver WHERE driverID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", driverID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {

                    txtUsername.Text = reader["driverUsername"].ToString();

                    txtName.Text = reader["driverName"].ToString();
                    txtSurname.Text = reader["driverSurname"].ToString();
                    txtEmail.Text = reader["driverEmail"].ToString();
                    txtNumber.Text = reader["driverPhoneNumber"].ToString();
                    

                    string imageName = reader["driverProfileImage"].ToString();

                    imgProfile.ImageUrl = string.IsNullOrEmpty(imageName) ? "~/Images/default.png" : "~/UploadedImages/" + imageName;


                }

            }
        }



        // Now we want to update and store everything When the button update is clicked this event happens
        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            int buyerID = Convert.ToInt32(Session["UserID"]);
            string username = txtUsername.Text.Trim();

            string name = txtName.Text.Trim();
            string surname = txtSurname.Text.Trim();
            string email = txtEmail.Text.Trim();
            string number = txtNumber.Text.Trim();
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
                    ? "UPDATE Driver SET driverUsername=@Username, driverName=@Name, driverSurname=@Surname, driverEmail=@Email, driverPhoneNumber=@Number, WHERE driverID=@ID"
                    : "UPDATE Driver SET driverUsername=@Username, driverName=@Name, driverSurname=@Surname, driverEmail=@Email, driverPhoneNumber=@Number, driverProfileImage=@Image WHERE driverID=@ID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);

                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Surname", surname);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Number", number);
                cmd.Parameters.AddWithValue("@ID", buyerID);

                if (!string.IsNullOrEmpty(newImageName))
                    cmd.Parameters.AddWithValue("@Image", newImageName);

                con.Open();
                cmd.ExecuteNonQuery();

                lblMessage.Text = "Profile updated successfully!";
                lblMessage.ForeColor = System.Drawing.Color.Lime;
                LoadBuyerProfile();

            }


        }
    }
}