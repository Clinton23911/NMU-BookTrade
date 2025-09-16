<%@ Page Title="Driver Profile" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" 
    CodeBehind="DriverProfile.aspx.cs" Inherits="NMU_BookTrade.Driver.ClintonModule.DriverProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
    <div class="profile-container">
        <h2>Driver Profile</h2>
        
        <!-- Profile Image -->
        <asp:Image ID="imgProfile" runat="server" CssClass="profile-image" AlternateText="Driver Profile Picture" />
        
        <!-- Profile Form -->
        <div class="form-group">
            <label>Username</label>
            <div class="input-icon">
                <i class="fas fa-user"></i>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="input-field" MaxLength="50" />
            </div>
            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                ControlToValidate="txtUsername" ErrorMessage="Username is required." 
                CssClass="form_errormessage" Display="Dynamic" />
        </div>

        <div class="form-group">
            <label>Name</label>
            <div class="input-icon">
                <i class="fas fa-id-card"></i>
                <asp:TextBox ID="txtName" runat="server" CssClass="input-field" MaxLength="50" />
            </div>
            <asp:RequiredFieldValidator ID="rfvName" runat="server" 
                ControlToValidate="txtName" ErrorMessage="Name is required." 
                CssClass="form_errormessage" Display="Dynamic" />
        </div>

        <div class="form-group">
            <label>Surname</label>
            <div class="input-icon">
                <i class="fas fa-id-card"></i>
                <asp:TextBox ID="txtSurname" runat="server" CssClass="input-field" MaxLength="50" />
            </div>
            <asp:RequiredFieldValidator ID="rfvSurname" runat="server" 
                ControlToValidate="txtSurname" ErrorMessage="Surname is required." 
                CssClass="form_errormessage" Display="Dynamic" />
        </div>

        <div class="form-group">
            <label>Email</label>
            <div class="input-icon">
                <i class="fas fa-envelope"></i>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="input-field" TextMode="Email" MaxLength="100" />
            </div>
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                ControlToValidate="txtEmail" ErrorMessage="Email is required." 
                CssClass="form_errormessage" Display="Dynamic" />
            <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                ControlToValidate="txtEmail" ErrorMessage="Invalid email format." 
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                CssClass="form_errormessage" Display="Dynamic" />
        </div>

        <div class="form-group">
            <label>Phone Number</label>
            <div class="input-icon">
                <i class="fas fa-phone"></i>
                <asp:TextBox ID="txtNumber" runat="server" CssClass="input-field" TextMode="Phone" MaxLength="15" />
            </div>
            <asp:RequiredFieldValidator ID="rfvNumber" runat="server" 
                ControlToValidate="txtNumber" ErrorMessage="Phone number is required." 
                CssClass="form_errormessage" Display="Dynamic" />
            <asp:RegularExpressionValidator ID="revNumber" runat="server" 
                ControlToValidate="txtNumber" ErrorMessage="Invalid phone number format." 
                ValidationExpression="^[0-9]{10,15}$" 
                CssClass="form_errormessage" Display="Dynamic" />
        </div>

        <div class="form-group">
            <label>Change Profile Picture</label>
            <asp:FileUpload ID="fuProfileImage" runat="server" CssClass="input-field" />
            <asp:Label ID="lblImageError" runat="server" CssClass="form_errormessage" Visible="false" />
        </div>

        <!-- Action Buttons -->
        <asp:Button ID="btnUpdate" runat="server" Text="Update Profile" CssClass="btn-update" OnClick="btnUpdate_Click" />
        <asp:Button ID="btnDelete" runat="server" Text="Delete Profile" CssClass="btn-update" 
            OnClientClick="return confirm('Are you sure you want to delete your profile? This cannot be undone.');" 
            OnClick="btnDelete_Click" style="background-color: #ff6b6b; margin-left: 10px;" />
        
        <!-- Status Message -->
        <asp:Label ID="lblMessage" runat="server" CssClass="message" />
    </div>
</asp:Content>