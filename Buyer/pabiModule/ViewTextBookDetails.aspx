<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ViewTextBookDetails.aspx.cs" Inherits="NMU_BookTrade.ViewTextBookDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
     <div class="categories">
    <asp:Repeater ID="rptCategory" runat="server" OnItemCommand="rptCategory_ItemCommand">
         <ItemTemplate>
             <asp:LinkButton runat="server" CommandName="SelectFaculty" CommandArgument='<%# Eval("categoryName") %>' CssClass="category-link">
                 <%# Eval("categoryName") %>
             </asp:LinkButton>
         </ItemTemplate>
     </asp:Repeater>
 </div>
<div class="container">
  <div class="book-details">
    <asp:Image ID="imgCover" runat="server" CssClass="book-cover" />

      <div class="info">
        <asp:Label CssClass="detail-label" runat="server" Text="Title"></asp:Label>
        <asp:Label ID="lblTitle" runat="server" CssClass="label-title"></asp:Label><br /><br />
                    
        <asp:Label CssClass="detail-label" runat="server" Text="Author"></asp:Label>
        <asp:Label ID="lblAuthor" runat="server"></asp:Label><br /><br />
                    
        <asp:Label CssClass="detail-label" runat="server" Text="Price"></asp:Label>
        <asp:Label ID="lblPrice" runat="server"></asp:Label><br /><br />
                    
        <asp:Label CssClass="detail-label" runat="server" Text="Condition"></asp:Label>
        <asp:Label ID="lblCondition" runat="server"></asp:Label><br /><br />
        
        <asp:Label CssClass="detail-label" runat="server" Text="Category"></asp:Label>
        <asp:Label ID="lblCategory" runat="server"></asp:Label><br /><br />
        <asp:Label CssClass="detail-label" runat="server" Text="Genre"></asp:Label>
        <asp:Label ID="lblGenre" runat="server"></asp:Label><br /><br />
                    
       <div class="buttons">
          <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart ➤" OnClick="btnAddToCart_Click" />
          <%--<asp:Button ID="btnAddToFavourites" runat="server" Text="Purchase ➤" OnClick="btnAddToFavourites_Click" />--%>
<%--           <asp:Button ID="btnCancel" runat="server" Text="Cancel ➤" CssClass="form-button" OnClick="btnCancel" CausesValidation="false" />--%>
             </div>
           <asp:Label ID="lblMessage" runat="server"></asp:Label>
          </div>
      <asp:Panel ID="CartPanel" runat="server" CssClass="slide-panel" Visible="false">
    <div class="panel-header">
        <asp:Label ID="lblHeader" runat="server" Text="Added to Cart"></asp:Label>
        <asp:Button ID="btnClose" runat="server" Text="X" CssClass="close-btn" OnClick="btnClose_Click" />
    </div>
    <div class="panel-content">
        <asp:Label ID="lblCartMessage" runat="server" Text=""></asp:Label>
        <br /><br />
        <asp:HyperLink ID="lnkGoToCart" runat="server" NavigateUrl="Cart.aspx" CssClass="go-to-cart">Go to Cart</asp:HyperLink>
    </div>
</asp:Panel>

      
     </div>
   </div>
</asp:Content>
