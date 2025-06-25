using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                            lblMessage.Text = "Registration successful! Redirecting to login in 4 seconds...";

                            // Redirect after 4 seconds
                            Response.AddHeader("REFRESH", "4;URL=" + ResolveUrl("~/User Management/Login.aspx"));

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