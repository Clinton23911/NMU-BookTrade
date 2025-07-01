using System;
using System.Web.UI;

namespace NMU_BookTrade
{
    public partial class ManageListings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load listing data from database
                LoadListingData();
            }
        }

        private void LoadListingData()
        {
            // TODO: Replace with actual database call
            // var listing = db.Listings.FirstOrDefault(l => l.Id == listingId);

            // Example data - replace with actual data from database
            txtTitle.Text = "Pure Mathematics 1";
            txtISBN.Text = "978-3-16-148410-0";
            txtPrice.Text = "R450";
            ddlCondition.SelectedValue = "good";
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    ShowAlert("Please enter a book title");
                    return;
                }

                // TODO: Update database record
                // var listing = db.Listings.Find(listingId);
                // listing.Title = txtTitle.Text;
                // listing.ISBN = txtISBN.Text;
                // listing.Price = txtPrice.Text;
                // listing.Condition = ddlCondition.SelectedValue;
                // db.SaveChanges();

                ShowAlert("Listing updated successfully!", "success");
            }
            catch (Exception ex)
            {
                ShowAlert("Error updating listing: " + ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: Delete from database
                // var listing = db.Listings.Find(listingId);
                // db.Listings.Remove(listing);
                // db.SaveChanges();

                ShowAlert("Listing deleted successfully!", "success");
                Response.Redirect("~/Listings.aspx"); // Redirect after deletion
            }
            catch (Exception ex)
            {
                ShowAlert("Error deleting listing: " + ex.Message);
            }
        }

        private void ShowAlert(string message, string type = "error")
        {
            string script = $@"alert('{message.Replace("'", @"\'")}');";
            if (type == "success")
            {
                script = $@"swal('Success!', '{message.Replace("'", @"\'")}', 'success');";
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", script, true);
        }
    }
}