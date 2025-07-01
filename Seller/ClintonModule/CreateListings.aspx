<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CreateListings.aspx.cs" Inherits="NMU_BookTrade.CreateListings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/CreateListings.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
    <div class="cl-container">
        <h1 class="cl-title">Create Listing</h1>
        
        <p class="cl-subtitle">Ready to share your book with others?</p>
        <p class="cl-description">List your book by filling in the details below — it's quick, easy, and helps fellow readers discover their next great read!</p>

        <div class="cl-form-row">
            <label class="cl-label">Title</label>
            <asp:TextBox ID="txtTitle" runat="server" CssClass="cl-input" placeholder="Book title"></asp:TextBox>
        </div>

        <div class="cl-form-row">
            <label class="cl-label">Book ISBN</label>
            <asp:TextBox ID="txtISBN" runat="server" CssClass="cl-input" placeholder="ISBN number"></asp:TextBox>
        </div>

        <div class="cl-form-row">
            <label class="cl-label">Price</label>
            <asp:TextBox ID="txtPrice" runat="server" CssClass="cl-input" placeholder="Price"></asp:TextBox>
        </div>

        <div class="cl-form-row">
            <label class="cl-label">Condition</label>
            <asp:DropDownList ID="ddlCondition" runat="server" CssClass="cl-select">
                <asp:ListItem Text="Excellent" Value="excellent"></asp:ListItem>
                <asp:ListItem Text="Very Good" Value="very-good"></asp:ListItem>
                <asp:ListItem Text="Good" Value="good" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Fair" Value="fair"></asp:ListItem>
                <asp:ListItem Text="Poor" Value="poor"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="cl-condition-desc">
            Select the option that best describes the state of your book
        </div>

        <div class="cl-upload-section">
           
               

            <div class="cl-upload-box">
                <div class="cl-upload-icon">?</div>
                <div class="cl-upload-title">If the book is hard copy,</div>
                <div class="cl-upload-subtitle">add the image of the book here and tick hard copy ✓</div>
                <div class="cl-drag-area" id="imageDrop">
                    <div>Drag and drop your file here</div>
                    <asp:FileUpload ID="fuImage" runat="server" style="display: none;" />
                </div>
                <div class="cl-file-info">Supported formats: JPG, PNG, GIF</div>
                <div id="imagePreview" runat="server" class="cl-file-preview"></div>
            </div>
        </div>

        <div class="cl-button-group">
            <asp:Button ID="btnSave" runat="server" Text="SAVE ▶" CssClass="cl-btn cl-btn-save" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel ▶" CssClass="cl-btn cl-btn-cancel" OnClick="btnCancel_Click" />
        </div>
    </div>

    <script src="js/CreateListings.js"></script>
</asp:Content>