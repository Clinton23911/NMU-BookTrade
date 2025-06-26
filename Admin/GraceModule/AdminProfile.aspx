<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AdminProfile.aspx.cs" Inherits="NMU_BookTrade.WebForm16" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
<<<<<<< HEAD
    <div class="profile-container">
        <div class="profile-form">
            <h2>Update Your Profile</h2>

            <!-- Display current profile picture -->
            <asp:Image ID="imgProfile" runat="server" CssClass="profile-img" />

            <!-- Upload new profile picture -->
            <div class="form-group">
                <label for="fuProfileImage">Upload New Picture</label><br />
                <asp:FileUpload ID="fuProfileImage" runat="server" />
            </div>

            <!-- Email -->
            <div class="form-group">
                <label for="txtEmail">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="input-field" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="Email is required" ForeColor="Red" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                    ControlToValidate="txtEmail" ValidationExpression="\w+@\w+\.\w+"
                    ErrorMessage="Invalid email format" ForeColor="Red" Display="Dynamic" />
            </div>

            <!-- Username -->
            <div class="form-group">
                <label for="txtUsername">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="input-field" />
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                    ErrorMessage="Username is required" ForeColor="Red" Display="Dynamic" />
            </div>

            <!-- Update button -->
            <asp:Button ID="btnUpdate" runat="server" Text="Update Profile" CssClass="register-btn" OnClick="btnUpdate_Click" />

            <!-- Message -->
            <asp:Label ID="lblMessage" runat="server" CssClass="message-label" />
        </div>
    </div>
=======

      <div class="profile-container">
        <h2>Admin Profile</h2>

        <!-- Profile Picture Display -->
        <asp:Image ID="imgProfile" runat="server" CssClass="profile-image" />

        <!-- Email Field -->
        <div class="form-group">
            <label for="txtEmail">Email Address</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="input-field" TextMode="Email" />
        </div>

        <!-- Username Field -->
        <div class="form-group">
            <label for="txtUsername">Username</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="input-field" />
        </div>

        <!-- Profile Image Upload -->
        <div class="form-group">
            <label for="fuProfileImage">Change Profile Picture</label>
            <asp:FileUpload ID="fuProfileImage" runat="server" />
        </div>

        <!-- Update Button -->
        <asp:Button ID="btnUpdate" runat="server" Text="Update Profile" CssClass="btn-update" OnClick="btnUpdate_Click" />

        <!-- Feedback Label -->
        <asp:Label ID="lblMessage" runat="server" CssClass="message" />
    </div>

>>>>>>> f95f0fc (worked on Admin page, sitemaster and stylesheet.css, and fixed the LogOut page)
</asp:Content>
