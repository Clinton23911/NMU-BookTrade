<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SearchResult.aspx.cs" Inherits="NMU_BookTrade.SearchResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="~/Styles/Stylesheet1.css" rel="stylesheet" type="text/css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
              
    <div class="categories-inline">
        <asp:Repeater ID="rptCategory" runat="server" OnItemCommand="rptCategory_ItemCommand">
            <ItemTemplate>
                <asp:LinkButton runat="server" CommandName="SelectCategory" CommandArgument='<%# Eval("categoryName") %>' CssClass="category-link">
                    <%# Eval("categoryName") %>
                </asp:LinkButton>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <div class="results-header">
        <h2>Search Results for: <asp:Label ID="lblSearched" runat="server" CssClass="search-term"></asp:Label></h2>
        <hr class="section-line" />
    </div>

    <div class="book-grid">
        <asp:Repeater ID="rptBooks" runat="server" OnItemCommand="rptBooks_ItemCommand">
            <ItemTemplate>
                <div class="book-card">
                    <asp:Image ID="imgCover" runat="server" ImageUrl='<%# Eval("coverImage") %>' Width="120" CssClass="book-image" />
                    <div class="book-info">
                        <div class="book-title"><%# Eval("title") %></div>
                        <div class="book-author">by <%# Eval("author") %></div>
                        <div class="book-price">R <%# Eval("price", "{0:N2}") %></div>
                    </div>
                    <div class="book-actions">
                        <asp:Button runat="server" Text="Add to Cart ➤"
                            CommandName="AddToCart"
                            CommandArgument='<%# Eval("bookISBN") %>'
                            CssClass="add-cart-btn" />
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

               <asp:Label ID="lblMessage" runat="server"></asp:Label>
      <asp:Panel ID="CartPanel" runat="server" CssClass="slide-panel" Visible="false">
    <div class="panel-header">
        <asp:Label ID="lblHeader" runat="server" Text="Added to Cart"></asp:Label>
        <asp:Button ID="btnClose" runat="server" Text="x" CssClass="close-btn" OnClick="btnClose_Click" />
    </div>
    <div class="panel-content">
        <asp:Label ID="lblCartMessage" runat="server" Text=""></asp:Label>
        <asp:Image ID="imgCartBook" runat="server" CssClass="cart-book-image" Width="80" />
        <asp:Label ID="lblCartBookTitle" runat="server" CssClass="cart-book-title"></asp:Label>
        <br /><br />
        <asp:HyperLink ID="lnkGoToCart" runat="server" NavigateUrl="Cart.aspx" CssClass="go-to-cart">Go to Cart ➤</asp:HyperLink>
    </div>
</asp:Panel>
</asp:Content>
