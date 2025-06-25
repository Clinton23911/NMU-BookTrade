using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;


namespace NMU_BookTrade
{
    public partial class WebForm16 : System.Web.UI.Page
    {
        // When the page loads this methode will be first to run
        protected void Page_Load(object sender, EventArgs e)
        {
            //but first we check if the page has loaded for  the first time( not from a button click)

            if (!IsPostBack) {

               // LoadAdminProfile();
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

                  //  string imageName = reader[]
                    
                }
                


            }
        }


    }
}