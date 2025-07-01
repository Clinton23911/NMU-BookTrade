<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="NMU_BookTrade.User_Management.ResetPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">

    <div class ="register-container">
     <div class="left-side">
         <asp:Image 
         ID="imgPersonReading" 
         runat="server" 
         CssClass="person-reading"
         ImageUrl="~/Images/Person reading.png" 
         AlternateText="Person Reading"/>

       </div> 
            <div class="left-side">
                    <div class="form-container">
                        <h2 class="LR-formheadings">Reset Your Password</h2>
                        <p class="p-Passwordpages">Now you can reset your password below. Remember to save it so that you do not forget it again.</p>

            <div class="password-wrapper">
                            <asp:TextBox ID="txtNewPassword" ClientIDMode="Static" runat="server" TextMode="Password" placeholder="Enter your new password" CssClass="input-field-forgotpassword " />
                            <span class="toggle-password-reset" onclick="toggleVisibility('txtNewPassword', this)">
                                <asp:RequiredFieldValidator 
                                    ID="rfvNewPassword" 
                                    runat="server" 
                                    ControlToValidate="txtNewPassword" 
                                    ErrorMessage="New password is required." 
                                    CssClass="form_errormessage" 
                                    Display="Dynamic" 
                                    ForeColor="Red" />

                                <i class="fas fa-eye"></i>
                            </span>
             </div>

<br />

<div class="password-wrapper">
            <asp:TextBox ID="txtConfirmPassword" ClientIDMode="Static" runat="server" TextMode="Password" placeholder="Confirm your new password" CssClass="input-field-forgotpassword " />
                <asp:RequiredFieldValidator 
                    ID="rfvConfirmPassword" 
                    runat="server" 
                    ControlToValidate="txtConfirmPassword" 
                    ErrorMessage="Please confirm your password." 
                    CssClass="form_errormessage" 
                    Display="Dynamic" 
                    ForeColor="Red" />
            <span class="toggle-password-reset2" onclick="toggleVisibility('txtConfirmPassword', this)">
                <i class="fas fa-eye"></i>
            </span>
       </div>
                        <br />  
                        <br />
                        <asp:Button ID="btnReset" runat="server" Text="Reset Password" OnClick="btnReset_Click" CssClass="form-button" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btnClear_forgetPasswordpage" OnClick="btnClear4_Click" CausesValidation="false" Width="89px" />
                        <br />
                        <asp:Label ID="lblMessage" runat="server" CssClass="form_errormessage" ForeColor="Red"/>


                    </div>

            </div>
   </div >


<script type="text/javascript">

                  // Toggle password visibility for input fields
              function toggleVisibility(inputId, iconSpan) {
                  var input = document.getElementById(inputId);

                  if (input.type === "password") {
                      input.type = "text"; // Show password
                      iconSpan.innerHTML = '<i class="fas fa-eye-slash"></i>'; // Switch to slashed eye
                  } else {
                      input.type = "password"; // Hide password
                      iconSpan.innerHTML = '<i class="fas fa-eye"></i>'; // Switch back to eye
                  }
              }
 </script>








   

</asp:Content>
