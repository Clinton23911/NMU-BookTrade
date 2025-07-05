<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ManageListings.aspx.cs" Inherits="NMU_BookTrade.ManageListings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/ManageListings.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
    <div class="ml-container">
        <div class="ml-header">
            <h1 class="ml-title">Manage Listings</h1>
            <div class="ml-search-results">
                <p>Search results for "Mathematics"</p>
                <asp:LinkButton ID="lnkViewAll" runat="server" CssClass="ml-view-all">View all results</asp:LinkButton>
            </div>
        </div>

        <div class="ml-book-card">
            <div class="ml-book-header">
                <h2 class="ml-book-title">Pure Mathematics 1 <span class="ml-book-price">R 450.00</span></h2>
                <div class="ml-book-reviews">3 reviews</div>
            </div>

            <div class="ml-instructions">
                <h3>How to manage your listing</h3>
                <ol class="ml-steps">
                    <li>Search for your book title which you have listed</li>
                    <li>Click on the book and update the form</li>
                </ol>
            </div>

            <div class="ml-form">
                <div class="ml-form-row">
                    <label class="ml-label">Title:</label>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="ml-input" Text="Pure Mathematics 1"></asp:TextBox>
                </div>

                <div class="ml-form-row">
                    <label class="ml-label">Book ISBN:</label>
                    <asp:TextBox ID="txtISBN" runat="server" CssClass="ml-input" Text="978-3-16-148410-0"></asp:TextBox>
                </div>

                <div class="ml-form-row">
                    <label class="ml-label">Price:</label>
                    <asp:TextBox ID="txtPrice" runat="server" CssClass="ml-input" Text="R450"></asp:TextBox>
                </div>

                <div class="ml-form-row">
                    <label class="ml-label">Condition:</label>
                    <asp:DropDownList ID="ddlCondition" runat="server" CssClass="ml-select">
                        <asp:ListItem Text="Excellent" Value="excellent"></asp:ListItem>
                        <asp:ListItem Text="Very Good" Value="very-good"></asp:ListItem>
                        <asp:ListItem Text="Good" Value="good" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Fair" Value="fair"></asp:ListItem>
                        <asp:ListItem Text="Poor" Value="poor"></asp:ListItem>
                    </asp:DropDownList>
                    <p class="ml-condition-desc">Select the option that best describes the state of your book</p>
                </div>

                <div class="ml-form-actions">
                    <asp:Button ID="btnUpdate" runat="server" Text="UPDATE ▼" CssClass="ml-btn ml-btn-update" OnClick="btnUpdate_Click" />
                    <asp:Button ID="btnDelete" runat="server" Text="DELETE ▼" CssClass="ml-btn ml-btn-delete" OnClick="btnDelete_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>