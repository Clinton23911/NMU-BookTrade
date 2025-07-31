using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;

namespace NMU_BookTrade
{
    public partial class WebForm9 : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["sent"] == "true")
                    LoadSentMessages();
                else
                    LoadMessages();
                int messageCount = GetMessageCount();
                inboxCountSpan.InnerText = messageCount.ToString();
            }
        }

        private void LoadMessages()
        {
            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;
            string query = "SELECT messageID, dateSent, messageContent, senderEmail, isRead FROM SupportMessages WHERE senderEmail IS NOT NULL AND senderEmail != @adminEmail ORDER BY dateSent DESC";
            // we are going to display the messages that are in the SupportMessages Table from newest to oldest 

            StringBuilder htmlBuilder = new StringBuilder(); // we use it to build the long string in html for our message 

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@adminEmail", "gracamanyonganise@gmail.com");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) // we are looping through each and every row in the SupportMessages table
                {
                    // here we are now accessing the data from the column read e.g. reader['column'].
                    string id = reader["messageID"].ToString();
                    DateTime dateSent = Convert.ToDateTime(reader["dateSent"]);
                    string content = reader["messageContent"].ToString();
                    string email = reader["senderEmail"].ToString();
                    bool isRead = Convert.ToBoolean(reader["isRead"]);

                    string[] parts = content.Split(new[] { '\n' }, 2);
                    string senderInfo = parts.Length > 0 ? parts[0] : "Unknown Sender";
                    string preview = parts.Length > 1 ? parts[1] : "";

                    string role = "anonymous";
                    if (senderInfo.ToLower().Contains("(seller)")) role = "seller";
                    else if (senderInfo.ToLower().Contains("(buyer)")) role = "buyer";
                    else if (senderInfo.ToLower().Contains("(driver)")) role = "driver";
                    else if (senderInfo.ToLower().Contains("(admin)")) role = "admin";

                    string bodySafe = preview.Replace("\"", "&quot;").Replace("'", "&apos;").Replace("\n", "<br>"); // Cleans the html and replaces double quotes and single quotes with safe HTML entities to prevent breaking HTML attributes.
                    string readClass = isRead ? "read" : "unread"; // this sets the readclass to read or unread. 

                    htmlBuilder.Append($@"
<div class='inbox-message-row {readClass}' onclick='selectMessage(this)'
     data-read='{(isRead ? "1" : "0")}' 

     data-sender='{senderInfo}'
     data-email='{email}'
     data-role='{role}'
     data-time='{dateSent}'
     data-body='{bodySafe}'
     data-id='{id}'> 
    <div class='inbox-message-content'>
       
        <div class='inbox-message-details'>
            <div class='inbox-message-header'>
                <div class='inbox-sender-info'>
                    <span class='inbox-sender-name'>{senderInfo}</span>
                    <span class='inbox-user-badge {role}'>{role}</span>
                </div>
               <span class='inbox-message-time'>{dateSent.ToString("MMM dd, yyyy hh:mm tt")}</span>


            </div>
            
            <div class='inbox-message-preview'>{(preview.Length > 80 ? preview.Substring(0, 80) + "..." : preview)}</div>
        </div>
       
    </div>
</div>");
                }
            }

            inboxMessagesContainer.InnerHtml = htmlBuilder.ToString();
        }

        [WebMethod]
        public static string LoadSentMessages()
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;
                string query = "SELECT messageID, dateSent, messageContent, senderEmail FROM SupportMessages WHERE senderEmail = @senderEmail ORDER BY dateSent DESC";

                StringBuilder htmlBuilder = new StringBuilder();

                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@senderEmail", "gracamanyonganise@gmail.com");  // Use admin's email here

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string id = reader["messageID"].ToString();
                        DateTime dateSent = Convert.ToDateTime(reader["dateSent"]);
                        string content = reader["messageContent"].ToString();
                        string email = reader["senderEmail"].ToString();

                        string[] parts = content.Split(new[] { '\n' }, 2);
                        string header = parts.Length > 0 ? parts[0] : "";
                        string body = parts.Length > 1 ? parts[1] : "";

                        htmlBuilder.Append($@"
                <div class='inbox-message-row sent' onclick='selectMessage(this)'
                     data-sender='Me'
                     data-email='{email}'
                     data-role='admin'
                     data-time='{dateSent}'
                     data-body='{body.Replace("\"", "&quot;").Replace("'", "&apos;").Replace("\n", "<br>")}'
                     data-id='{id}'> 
                    <div class='inbox-message-content'>
                        
                        <div class='inbox-message-details'>
                            <div class='inbox-message-header'>
                                <div class='inbox-sender-info'>
                                    <span class='inbox-sender-name'>Me</span>
                                    <span class='inbox-user-badge admin'>admin</span>
                                </div>
                               <span class='inbox-message-time'>{dateSent.ToString("MMM dd, yyyy hh:mm tt")}</span>
                            </div>
                           
                            <div class='inbox-message-preview'>{(body.Length > 80 ? body.Substring(0, 80) + "..." : body)}</div>
                        </div>
                       
                    </div>
                </div>");
                    }
                }

                return htmlBuilder.ToString();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }


        private int GetMessageCount()
        {
            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;
            string query = "SELECT COUNT(*) FROM SupportMessages WHERE isRead = 0";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        [System.Web.Services.WebMethod]
        public static string DeleteMessage(int messageID)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand("DELETE FROM SupportMessages WHERE messageID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", messageID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return "Message deleted.";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }


        [WebMethod]
        public static string MarkAsRead(int[] ids)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    foreach (int id in ids)
                    {
                        using (SqlCommand cmd = new SqlCommand("UPDATE SupportMessages SET isRead = 1 WHERE messageID = @id", conn))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    return "Messages marked as read.";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }


        [WebMethod]
        public static string MarkMessageAsRead(int messageID)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand("UPDATE SupportMessages SET isRead = 1 WHERE messageID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", messageID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return "Marked as read";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        [WebMethod]
        public static int GetUnreadMessageCount()
        {
            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;
            string query = "SELECT COUNT(*) FROM SupportMessages WHERE isRead = 0";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }


        [WebMethod(EnableSession = true)]
        public static string SendMessage(string to, string subject, string body)
        {
            try
            {
                string senderEmail = "gracamanyonganise@gmail.com"; // Hardcoded for now


                string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand("INSERT INTO SupportMessages (messageContent, senderEmail, dateSent, isRead) VALUES (@content, @senderEmail, @date, 1)", conn))
                {
                    string messageContent = $"To: {to}\nSubject: {subject}\n{body}";
                    cmd.Parameters.AddWithValue("@content", messageContent);
                    cmd.Parameters.AddWithValue("@senderEmail", senderEmail);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return "Message Sent";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        public static string SendEmail(string to, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("gracamanyonganise@gmail.com");
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("gracamanyonganise@gmail.com", "sdijpajgbcnwuizi");
                smtp.EnableSsl = true;

                smtp.Send(mail);
                return "Message sent successfully!";
            }
            catch (Exception ex)
            {
                return "Error sending message: " + ex.Message;
            }
        }
    }
}
