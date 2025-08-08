<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ManageCategories.aspx.cs" Inherits="NMU_BookTrade.WebForm17" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">

    <h1 class="admin-heading">Manage Categories</h1>
    <hr />
    <br />
    <h3 class="h3-categories">&nbsp;&nbsp; Steps to adding a faculty into the categories section:</h3>
    <div class="BookImage-category">
        <asp:Image ID="BookImage" CssClass="category-image" ImageUrl="~/Images/BookImage.png" runat="server" /> 

        <ol>
            <li>Type the name of the faculty you want below.</li>
            <li>Click add to save the name in the database.</li>
            <li>The faculty name added will be saved in the categories table.</li>
        </ol>


    </div>  
    
     <!-- ── Add-new form ─────────────────────────────────────────── -->
    <div class="add-wrapper">
        <!-- TextBox: faculty / category name -->
        <asp:TextBox ID="txtCategoryName" runat="server"
                     CssClass="auto-style2"
                     placeholder="Enter faculty name..." Width="152px" />

        <asp:Button ID="btnAddCategory" runat="server"
                    Text="Add Category"
                    CssClass="auto-style1"
                    OnClick="btnAddCategory_Click" Width="196px" />
    </div>
    <h3 class="h3-categories"> &nbsp;&nbsp; After adding the faculty it will appear in the table below. </h3>
    <br />
    <!-- ── GridView: list + inline edit/delete ─────────────────── -->
    <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False"
                  CssClass="manage-cat-table"
                  DataKeyNames="categoryID"
                  OnRowEditing="gvCategories_RowEditing"
                  OnRowUpdating="gvCategories_RowUpdating"
                  OnRowCancelingEdit="gvCategories_RowCancelingEdit"
                  OnRowDeleting="gvCategories_RowDeleting">

       
        <Columns>
            <asp:BoundField DataField="categoryName"
                            HeaderText="Categories"
                            ReadOnly="False" />

           
            <asp:CommandField ShowEditButton="True"
                              ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>

    <br />
    <br />
</asp:Content>
