<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AddDriver.aspx.cs" Inherits="NMU_BookTrade.Admin.GraceModule.DeleteDriver" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
  

    <div class="register-container">
        <div class="right-side">
            <h2>Add New Driver</h2>
            <p>Add your drivers details here to create their account.  </p>

            <asp:Label ID="lblMessage" runat="server" CssClass="form_errormessage" />

            <table class="register-table">
                <tr>
                    <td>
                        <div class="input-icon">
                            <i class="fas fa-user"></i>
                            <asp:TextBox ID="txtName" runat="server" CssClass="input-field" placeholder="Name" />
                        </div>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="Name is required" ControlToValidate="txtName" ForeColor="Red" Display="Dynamic" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input-icon">
                            <i class="fas fa-user"></i>
                            <asp:TextBox ID="txtSurname" runat="server" CssClass="input-field" placeholder="Surname" />
                        </div>
                        <asp:RequiredFieldValidator ID="rfvSurname" runat="server" ErrorMessage="Surname is required" ControlToValidate="txtSurname" ForeColor="Red" Display="Dynamic" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input-icon">
                            <i class="fas fa-envelope"></i>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="input-field" TextMode="Email" placeholder="Email" />
                        </div>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email is required" ControlToValidate="txtEmail" ForeColor="Red" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid email format" ValidationExpression="^\S+@\S+\.\S+$" ForeColor="Red" Display="Dynamic" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input-icon">
                            <i class="fas fa-phone"></i>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="input-field" placeholder="Phone Number" />
                        </div>
                        <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ErrorMessage="Phone number is required" ControlToValidate="txtPhone" ForeColor="Red" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone" ErrorMessage="Invalid phone number format" ValidationExpression="^\+?\d{10,15}$" ForeColor="Red" Display="Dynamic" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input-icon">
                            <i class="fas fa-user-circle"></i>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="input-field" placeholder="Username" />
                        </div>
                        <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ErrorMessage="Username is required" ControlToValidate="txtUsername" ForeColor="Red" Display="Dynamic" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input-icon">
                            <i class="fas fa-lock"></i>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="input-field" TextMode="Password" placeholder="Password" />
                        </div>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password is required" ControlToValidate="txtPassword" ForeColor="Red" Display="Dynamic" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label style="color:#c1f6ed;">Upload Profile Image:</label><br />
                        <asp:FileUpload ID="fuImage" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvImage" runat="server" ErrorMessage="Profile image is required" ControlToValidate="fuImage" ForeColor="Red" Display="Dynamic" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnAddDriver" runat="server" Text="Add Driver" CssClass="register-btn-AddDriver" OnClick="btnAddDriver_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <br />
    <br />


</asp:Content>
