<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="NMU_BookTrade.WebForm6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">

    <div class="register-container">
        <div class="left-side">
            <asp:Image 
            ID="imgPersonReading" 
            runat="server" 
            CssClass="person-reading"
            ImageUrl="~/Images/Person reading.png" 
            AlternateText="Person Reading"/>

        </div>  

        <div class="right-side">
            <h2 class=" LR-formheadings" > Create an account </h2>
            <h3 class=" LR-formheadings">Lets get started </h3>

            <table class="register-table">
                <tr>
                    <td>
                        <div class="input-wrapper">
                            <div class="input-icon">
                                <i class="fas fa-user-tag"></i>
                                <asp:DropDownList ID="ddlRole" runat="server" CssClass="input-field">
                                    <asp:ListItem Text="-- Select Role --" Value="" />
                                    <asp:ListItem Text="Buyer" Value="2" />
                                    <asp:ListItem Text="Seller" Value="3" />
                                    </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvRole" runat="server" 
                                ErrorMessage="Please select a role" 
                                ControlToValidate="ddlRole"
                                InitialValue="" 
                                CssClass="form_errormessage" 
                                ForeColor="Red" 
                                Display="Dynamic" />
                        </div>
                    </td>
               </tr>



                <tr>
                    <td>
                        <div class="input-wrapper">
                            <div class="input-icon">
                                <i class="fas fa-user"></i>
                                <asp:TextBox ID="txtName" runat="server" CssClass="input-field" ToolTip="Name" placeholder="Name"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="Please enter your Name" ControlToValidate="txtName" CssClass="form_errormessage" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </td>  
                </tr>  

                <tr>
                    <td>
                        <div class="input-wrapper">
                            <div class="input-icon">
                                <i class="fas fa-user"></i>
                                <asp:TextBox ID="txtSurname" runat="server" CssClass="input-field" ToolTip="Surname" placeholder ="Surname"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvSurname" runat="server" ErrorMessage="Please enter your Surname" ControlToValidate="txtSurname" CssClass="form_errormessage" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </td>  
                </tr>  

                <tr>
                    <td>
                        <div class="input-wrapper">
                            <div class="input-icon">
                                <i class="fas fa-envelope"></i>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="input-field" ToolTip="Email" placeholder="Email"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Please enter your email address" ControlToValidate="txtEmail" CssClass="form_errormessage" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Invalid email address" ForeColor="Red" ControlToValidate="txtEmail" ValidationExpression="^\S+@\S+\.\S+$" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>  
                </tr>  

                <tr>
                    <td>
                        <div class="input-wrapper">
                            <div class="input-icon">
                                <i class="fas fa-phone"></i>
                                <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="input-field" ToolTip="Phone Number" placeholder="Phone Number"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvPhoneNumber" runat="server" ErrorMessage="Your phone number is required here" ControlToValidate="txtPhoneNumber" CssClass="form_errormessage" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber"
                                ErrorMessage="Invalid phone number" ValidationExpression="^\+?\d{10,15}$"
                                ForeColor="Red" Display="Dynamic" />
                        </div>
                    </td>  
                </tr>

                <tr>
                    <td>
                        <div class="input-wrapper">
                            <div class="input-icon">
                                <i class="fas fa-location-dot"></i>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="input-field" ToolTip="Home Address" placeholder ="Home Adress"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ErrorMessage="Your home address is required here" ControlToValidate="txtAddress" CssClass="form_errormessage" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </td>  
                </tr>

                <tr>
                    <td>
                        <div class="input-wrapper">
                            <div class="input-icon">
                                <i class="fas fa-user"></i>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="input-field" ToolTip="Student Number (Without the s)" placeholder="Student Number (Without the s)"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ErrorMessage="Enter your student number, without the s at the beginning" ControlToValidate="txtUsername" CssClass="form_errormessage" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </td>  
                </tr>

                <tr>
                    <td>
                        <div class="input-wrapper">
                            <div class="input-icon">
                                <i class="fas fa-lock"></i>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="input-field" ToolTip="Password" placeholder="Password"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Enter your password, it's required" ControlToValidate="txtPassword" CssClass="form_errormessage"  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </td>  
                </tr>

                <tr>
                    <td>
                        <div class="input-wrapper">
                            <label>Confirm Password:</label>
                            <div class="input-icon">
                                <i class="fas fa-lock"></i>
                                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="input-field" placeholder="Re-enter your password" />
                            </div>
                            <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
                                ErrorMessage="Please confirm your password" ForeColor="Red" Display="Dynamic" />
                            <asp:CompareValidator ID="cvPasswords" runat="server"
                                ControlToCompare="txtPassword"
                                ControlToValidate="txtConfirmPassword"
                                ErrorMessage="Passwords do not match"
                                Operator="Equal"
                                Type="String"
                                ForeColor="Red"
                                Display="Dynamic" />
                        </div>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="form-button" OnClick="btnRegister_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="form-button" OnClick="btnClear_Click" CausesValidation="false" />
                    </td>

                </tr>
               
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" CssClass="form_errormessage" ForeColor="Red" />
                    </td>
                </tr>

            </table>
        </div>
    </div>

</asp:Content>
