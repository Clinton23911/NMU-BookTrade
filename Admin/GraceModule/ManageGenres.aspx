<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ManageGenres.aspx.cs" Inherits="NMU_BookTrade.WebForm8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
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
                     <li>Click 'Edit' or 'Delete' next to a genre to modify it.</li>
                     <li>Changes will reflect immediately in the list below.</li>
                 </ol>

            </div>
           

            <div class="add-wrapper">
                <asp:TextBox ID="txtGenreName" runat="server" CssClass="auto-style3" placeholder="Enter genre name..." Width="156px" />

                <asp:Button ID="btnAddGenre" runat="server" Text="Add Genre" CssClass="auto-style4" OnClick="BtnAddGenre_Click" Width="192px" />
            </div>
            <h3 class="h3-categories">Existing genres listed below:</h3>
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
            <br />
            <br />
        </div>

      
           
    
</asp:Content>
