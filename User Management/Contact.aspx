<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="NMU_BookTrade.WebForm4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server"> 
    <div class="contact-wrapper">

        <!-- LEFT: Image -->
        <div class="left-side-contact">
            <asp:Image 
                ID="imgPersonReading" 
                runat="server" 
                CssClass="person-reading"
                ImageUrl="~/Images/getintouchperson.png" 
                AlternateText="Person Reading"/>
        </div>  

        <!-- RIGHT: Form and Info -->
        <div class="contact-right">
            <h2 class="contact-h2">Get in touch with us</h2>
            <p>Our Admin is happy to help with whatever issue or question you have. Just pop us a message, or call us directly.</p>

            <!-- First Name -->
            <asp:Label AssociatedControlID="txtFirstName" runat="server" CssClass="contact-lable" Text="First Name:" />
            <asp:TextBox ID="txtFirstName" runat="server" />
            <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="First Name is required." ForeColor="Red" Display="Dynamic" />
            <br />

            <!-- Last Name -->
            <asp:Label AssociatedControlID="txtLastName" runat="server" CssClass="contact-lable" Text="Last Name:" />
            <asp:TextBox ID="txtLastName" runat="server" />
            <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" ErrorMessage="Last Name is required." ForeColor="Red" Display="Dynamic" />
            <br />

            <!-- Email -->
            <asp:Label AssociatedControlID="txtEmail" runat="server" CssClass="contact-lable" Text="Your Email:" />
            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="contact-input" />
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." ForeColor="Red" Display="Dynamic" />
            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" 
                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" 
                ErrorMessage="Invalid email format." ForeColor="Red" Display="Dynamic" />
            <br /><br />

            <!-- Message -->
            <asp:Label AssociatedControlID="txtMessage" runat="server" CssClass="contact-lable" Text="Your Message:" /> <br />
            <asp:TextBox ID="txtMessage" TextMode="MultiLine" CssClass="textarea-contact" runat="server" />
            <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage" ErrorMessage="Message is required." ForeColor="Red" Display="Dynamic" />
            <br /><br />
            
            <!-- Submit -->
            <asp:Button ID="btnSubmit" runat="server" Text="Send Message" CssClass="btn-Cosubmit" OnClick="btnSubmit_Click" />

            <!-- Info -->
            <div class="contact-info">
                <p><strong>Email:</strong> gracamanyonganise@gmail.com</p>
                <p><strong>Phone/Message us:</strong> +27 808 69557</p>
                <p>If you're already registered, please check your inbox for an email from admin.</p>
            </div>
        </div>
    </div>
</asp:Content>
