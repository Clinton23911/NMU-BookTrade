using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NMU_BookTrade.User_Management
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            // Step 1: Get values
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (newPassword != confirmPassword)
            {
                lblMessage.Text = "Passwords do not match.";
                return;
            }

            // Step 2: Retrieve session values
            string userId = Session["ResetUserID"] as string;
            string role = Session["ResetRole"] as string;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
            {
                lblMessage.Text = "Session expired or invalid access.";
                return;
            }

            // Optional: Re-enable this if using hashing again
            // newPassword = HashPassword(newPassword);

            // Step 3: Build update query
            string query = $"UPDATE {role} SET {role.ToLower()}Password = @Password WHERE {role.ToLower()}ID = @ID";

            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ToString();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Password", newPassword);
                cmd.Parameters.AddWithValue("@ID", userId);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.GhostWhite;
                    lblMessage.Text = "Password successfully reset. You may now log in.";
                }
                else
                {
                    lblMessage.Text = "Failed to reset password. Try again.";
                }
            }
        }


        protected void btnClear4_Click(object sender, EventArgs e)
        {
            txtNewPassword.Text= " ";
            txtConfirmPassword.Text= "";
            
        }

    }
}