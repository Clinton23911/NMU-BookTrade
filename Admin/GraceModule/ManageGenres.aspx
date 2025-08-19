<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostBack="true" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ManageGenres.aspx.cs" Inherits="NMU_BookTrade.WebForm8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/Styles/ManageGenres.css" />
    <script>
        function showCustomModal() {
            document.getElementById('genreDeleteOverlay').style.display = 'block';
        }

        function hideCustomModal() {
            document.getElementById('genreDeleteOverlay').style.display = 'none';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
    <h1 class="admin-heading">Manage Book Genres</h1>
    <hr />
    <br />

    <!-- Left Column -->
    <div class="manage-cat-left">
        <h3 class="h3-categories">&nbsp;&nbsp;Steps to add a genre to the system:</h3>

        <div class="BookImage-category">
            <asp:Image ID="BookImage" CssClass="category-image" ImageUrl="~/Images/BookImage.png" runat="server" />
            <ol>
                <li>Type and add a new genre using the form.</li>
                <li>Select the category that the genre belongs to.</li>
                <li>Click 'Edit' or 'Delete' next to a genre to modify it.</li>
                <li>Changes will reflect immediately in the list below.</li>
            </ol>
        </div>

        <div class="add-wrapper">
            <asp:TextBox ID="txtGenreName" runat="server" CssClass="auto-style3" placeholder="Enter genre name..." Width="156px" />
            <asp:DropDownList ID="ddlCategories" runat="server" CssClass="dropdown-categories"></asp:DropDownList>
            <asp:Button ID="btnAddGenre" runat="server" Text="Add Genre" CssClass="auto-style4" OnClick="BtnAddGenre_Click" Width="192px" />
            <asp:Label ID="lblFeedback" runat="server" CssClass="alert alert-info" Visible="false" />
            <br />
            <br />
        </div>
        <br />
   <h3 class="h3-categories">  &nbsp;&nbsp;Existing genres listed below:</h3>
        <asp:GridView ID="gvGenres" runat="server" AutoGenerateColumns="False" CssClass="manage-cat-table" DataKeyNames="genreID"
                      OnRowEditing="GvGenres_RowEditing"
                      OnRowUpdating="GvGenres_RowUpdating"
                      OnRowCancelingEdit="GvGenres_RowCancelingEdit"
                      OnRowDeleting="GvGenres_RowDeleting">
            <Columns>
                <asp:BoundField DataField="genreName" HeaderText="Genres" ReadOnly="False" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>

        <!-- Modal for Delete Confirmation -->
        <div id="genreDeleteOverlay" class="genre-modal-overlay">
            <div class="genre-modal-box">
                <h5 class="genre-modal-title">Confirm Deletion</h5>
                <asp:Label ID="lblConfirmText" runat="server" Text="" CssClass="form-control-static" />
                <div class="genre-modal-buttons">
                    <asp:Button ID="btnConfirmDelete" runat="server" Text="Delete" CssClass="genre-btn-delete" OnClick="btnConfirmDelete_Click" />
                    <asp:Button ID="btnCancelDelete" runat="server" Text="Cancel" CssClass="genre-btn-cancel" OnClick="btnCancelDelete_Click" CausesValidation="false" />
                </div>
            </div>
        </div>

        <br />
        <br />
    </div>
</asp:Content>