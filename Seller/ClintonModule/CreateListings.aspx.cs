using System;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace NMU_BookTrade
{
    public partial class CreateListings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Register client scripts if needed
                RegisterClientScripts();
            }
        }

        private void RegisterClientScripts()
        {
            // Register any necessary startup scripts
            string script = @"
                function setupFileUpload(dropZoneId, fileInputId, previewId) {
                    const dropZone = document.getElementById(dropZoneId);
                    const fileInput = document.getElementById(fileInputId);
                    const preview = document.getElementById(previewId);

                    dropZone.addEventListener('click', () => fileInput.click());

                    dropZone.addEventListener('dragover', (e) => {
                        e.preventDefault();
                        dropZone.classList.add('dragover');
                    });

                    dropZone.addEventListener('dragleave', () => {
                        dropZone.classList.remove('dragover');
                    });

                    dropZone.addEventListener('drop', (e) => {
                        e.preventDefault();
                        dropZone.classList.remove('dragover');
                        if (e.dataTransfer.files.length) {
                            fileInput.files = e.dataTransfer.files;
                            const event = new Event('change');
                            fileInput.dispatchEvent(event);
                        }
                    });

                    fileInput.addEventListener('change', (e) => {
                        if (e.target.files.length) {
                            const file = e.target.files[0];
                            preview.innerHTML = `📄 ${file.name} (${Math.round(file.size/1024)}KB)`;
                        }
                    });
                }

                // Initialize for both upload areas
                document.addEventListener('DOMContentLoaded', function() {
                    setupFileUpload('imageDrop', '" + fuImage.ClientID + @"', '" + imagePreview.ClientID + @"');
                });
            ";

            ScriptManager.RegisterStartupScript(this, GetType(), "FileUploadScript", script, true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    ShowAlert("Please enter a book title");
                    return;
                }

                // Process file uploads
                //string pdfPath = "";
                //if (fuPdf.HasFile)
                //{
                //    if (fuPdf.PostedFile.ContentLength > 10 * 1024 * 1024) // 10MB
                //    {
                //        ShowAlert("PDF file size exceeds 10MB limit");
                //        return;
                //    }

                //    pdfPath = "/Uploads/" + Path.GetFileName(fuPdf.FileName);
                //    fuPdf.SaveAs(Server.MapPath(pdfPath));
                //}

                string imagePath = "";
                if (fuImage.HasFile)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(fuImage.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ShowAlert("Only JPG, PNG, and GIF images are allowed");
                        return;
                    }

                    imagePath = "/Uploads/" + Path.GetFileName(fuImage.FileName);
                    fuImage.SaveAs(Server.MapPath(imagePath));
                }

                // Save to database (pseudo-code)
                // var book = new Book {
                //     Title = txtTitle.Text,
                //     ISBN = txtISBN.Text,
                //     Price = txtPrice.Text,
                //     Condition = ddlCondition.SelectedValue,
                //     PdfPath = pdfPath,
                //     ImagePath = imagePath
                // };
                // db.Books.Add(book);
                // db.SaveChanges();

                ShowAlert("Book listing created successfully!", "success");
            }
            catch (Exception ex)
            {
                ShowAlert("Error creating listing: " + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
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