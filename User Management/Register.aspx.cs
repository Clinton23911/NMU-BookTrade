﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace NMU_BookTrade
{
    public partial class WebForm6 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {

            if (!Page.IsValid)
            {
                return; // Stop here if any validators (like ConfirmPassword) failed
            }

            string role = ddlRole.SelectedValue;

            if (string.IsNullOrEmpty(role))
            {
                lblMessage.Text = "Please select a role.";
                return;
            }

            // Collect input values
            string username = txtUsername.Text.Trim();
            //string password = HashPassword(txtPassword.Text.Trim()); // hashing password before saving it in the database
            string password = txtPassword.Text.Trim();
            string name = txtName.Text.Trim();
            string surname = txtSurname.Text.Trim();
            string email = txtEmail.Text.Trim();
            string number = txtPhoneNumber.Text.Trim();
            string address = txtAddress.Text.Trim();

            string query = "";


            if (role == "2")
            {
                query = "INSERT INTO Buyer (buyerUsername, buyerPassword, buyerName, buyerSurname, buyerEmail, buyerNumber, buyerAddress, accessID) " +
                        "VALUES (@Username, @Password, @Name, @Surname, @Email, @Number, @Address,@AccessID)";
            }
            else if (role == "3")
            {
                query = "INSERT INTO Seller (sellerUsername, sellerPassword, sellerName, sellerSurname, sellerEmail, sellerNumber, sellerAddress, accessID) " +
                        "VALUES (@Username, @Password, @Name, @Surname, @Email, @Number, @Address, @AccessID)";
            }
           /* else if (role == "4")
            {
                query = "INSERT INTO Driver (driverUsername, driverPassword, driverName, driverSurname, driverEmail, driverNumber, driverAddress, accessID) " +
                        "VALUES (@Username, @Password, @Name, @Surname, @Email, @Number, @Address, @AccessID)";
            }*/
            else
            {
                lblMessage.Text = $"Invalid role selected: {ddlRole.SelectedItem.Text} ({ddlRole.SelectedValue})";
                return;
            }

            // Get connection string from Web.config
            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameter values
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Surname", surname);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Number", number);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@AccessID", role);

                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                            lblMessage.Text = "Registration successful! Check Confirmation message at the Top";

                            // Get role name and full name
                            string roleText = ddlRole.SelectedItem.Text;
                            string fullName = name + " " + surname;

                            // Send confirmation email
                            SendConfirmationEmail(email, fullName, roleText,username);

                            //  Trigger modal popup after successful registration
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", $"showConfirmation('{roleText}');", true);



                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Registration failed.";
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Error: " + ex.Message;
                    }
                }
            }
        }

        private void SendConfirmationEmail(string toEmail, string fullName, string role, string username)
        {
            // The email account sending the message (admin/support)
            string fromEmail = "gracamanyonganise@gmail.com";

            // An App Password from that Outlook account — must be generated
            string fromPassword = "sdijpajgbcnwuizi ";

            // The subject line of the email
            string subject = "NMU BookTrade: Welcome!";

            // The body of the email message (sent to students)
            string body = $@"


                    Hi {fullName},

                    Welcome to NMU BookTrade!
                        Welcome to NMU BookTrade!

                        Your registration as a {role} has been completed successfully.

                        Your username: {username}

                        If you forget your password, please use the 'Forgot Password' option on the login page to reset it.

                        If you have any questions, contact our support team: gracamanyonganise@gmail.com

                    Happy trading!
                    NMU BookTrade Team";

            // Create the message to send
            MailMessage mail = new MailMessage();

            // Who it's from (support email + name label)
            mail.From = new MailAddress(fromEmail, "NMU BookTrade");

            // Who it’s going to (student’s email)
            mail.To.Add(toEmail);

            // Add subject and message
            mail.Subject = subject;
            mail.Body = body;

            // Plain text (change to true if using HTML formatting)
            mail.IsBodyHtml = false;

            // Setup the email server (SMTP)
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);


            // Credentials to log in to the sending email
            smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
            smtp.EnableSsl = true;
                        

            // Send the email
            try
            {
                smtp.Send(mail);  // Actually sends the message
            }
            catch (Exception ex)
            {
                // Optional: show or log error
                System.Diagnostics.Debug.WriteLine("Email failed: " + ex.Message);
            }

        }




        // This function hashes a plain-text password using SHA256 encryption
       // public string HashPassword(string password)
       // {
            // Create a SHA256 object that will handle the hashing
            //using (SHA256 sha256 = SHA256.Create())
           // {
                // Convert the input string (password) into a byte array using UTF-8 encoding
               // byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Create a StringBuilder to build the hashed string
               // StringBuilder builder = new StringBuilder();

                // Loop through each byte in the byte array
               // foreach (byte b in bytes)
              //  {
                    // Convert each byte to a hexadecimal string (2 characters) and append to the builder
               //     builder.Append(b.ToString("x2"));
               // }

                // Return the final hashed string (e.g., "a3c5b4d6...")
               // return builder.ToString();
          //  }
       // }


        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtName.Text = "";
            txtSurname.Text = "";
            txtEmail.Text = "";
            txtPhoneNumber.Text = "";
            txtAddress.Text = "";
            ddlRole.SelectedIndex = 0;
            lblMessage.Text = "";
        }

    }
}