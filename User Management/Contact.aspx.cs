using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Web.UI;

namespace NMU_BookTrade
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string message = txtMessage.Text.Trim();

            // Determine role from session
            string role = "Anonymous";
            if (Session["AccessID"] != null)
            {
                role = GetUserRole(Session["AccessID"].ToString());
            }

            // Compose full message content
            string fullMessage = $"From: {firstName} {lastName} ({role})\nEmail: {email}\n\n{message}";

            // Save to database with senderEmail
            SaveMessageToDatabase(fullMessage, email);

            // Optionally send a notification email
            SendEmail(fullMessage);

            // Clear form inputs
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtMessage.Text = "";

            // Show success alert
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Your message has been sent successfully.');", true);
        }

        private string GetUserRole(string accessID)
        {
            switch (accessID)
            {
                case "1": return "Admin";
                case "2": return "Buyer";
                case "3": return "Seller";
                case "4": return "Driver";
                default: return "Anonymous";
            }
        }

        private void SaveMessageToDatabase(string content, string email)
        {
            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "INSERT INTO SupportMessages (dateSent, messageContent, senderEmail) VALUES (@dateSent, @messageContent, @senderEmail)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@dateSent", DateTime.Now);
                cmd.Parameters.AddWithValue("@messageContent", content);
                cmd.Parameters.AddWithValue("@senderEmail", email);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void SendEmail(string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("gracamanyonganise@gmail.com");
            mail.To.Add("gracamanyonganise@gmail.com");
            mail.Subject = "New Support Message - NMU BookTrade";
            mail.Body = body;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("gracamanyonganise@gmail.com", "sdijpajgbcnwuizi");
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }
    }
}
