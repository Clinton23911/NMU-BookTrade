<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AdminProfile.aspx.cs" Inherits="NMU_BookTrade.WebForm16" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
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


</asp:Content>
