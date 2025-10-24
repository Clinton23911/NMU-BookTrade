<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SearchResult.aspx.cs" Inherits="NMU_BookTrade.SearchResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="~/Styles/Stylesheet1.css" rel="stylesheet" type="text/css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
            <div class="categories-inline">
        <asp:Repeater ID="rptCategory1" runat="server" OnItemCommand="rptCategory_ItemCommand">
            <ItemTemplate>
                <asp:LinkButton runat="server"
                    CommandName="SelectCategory"
                    CommandArgument='<%# Eval("categoryID") + "|" + Eval("categoryName") %>'
                    CssClass="category-link">
                    <%# Eval("categoryName") %>
                </asp:LinkButton>
            </ItemTemplate>
        </asp:Repeater>
        <div class="search-bar-bd">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-bd" Placeholder="Search by Title or Author..." />
            <br />
            <asp:ListBox ID="lstSuggestions" runat="server" Visible="false" Width="300px"></asp:ListBox>
            <asp:Button ID="btnSearch" runat="server" Text="🔍" OnClick="btnSearch_Click" CssClass="search-btn" />
        </div>

        <asp:Repeater ID="rptCategory2" runat="server" OnItemCommand="rptCategory_ItemCommand">
            <ItemTemplate>
                <asp:LinkButton runat="server"
                    CommandName="SelectCategory"
                    CommandArgument='<%# Eval("categoryID") + "|" + Eval("categoryName")%>'
                    CssClass="category-link">
                    <%# Eval("categoryName") %>
                </asp:LinkButton>
            </ItemTemplate>
        </asp:Repeater>
    </div>


    <div class="results-header">
        <asp:Label ID="lblSearched" runat="server" CssClass="search-term"></asp:Label>
        <hr class="section-line" />
    </div>

              <asp:Panel ID="pnlSearchResults" runat="server" Visible="false">
          
        <asp:Label ID="lblSearchResults" runat="server" CssClass="results-label" Font-Bold="false"></asp:Label>

         <asp:Repeater ID="rptSearchResults" runat="server">
     <ItemTemplate>
         <div class="textbook">
              <asp:LinkButton ID="lnkBookCover" runat="server" 
                CommandName="ViewBook"
                CommandArgument='<%# Eval("bookISBN") %>'>
                <img src='<%# ResolveUrl(Eval("coverImage").ToString()) %>' 
                     alt='<%# Eval("title") %>' 
                     style="cursor:pointer;" />
            </asp:LinkButton>
            <br />
            <b><%# Eval("title") %></b><br />
            R<%# Eval("price") %>
        </div>
     </ItemTemplate>
 </asp:Repeater>
</asp:Panel> 

    <asp:Panel ID="pnlCategoryResults" runat="server" Visible="false">
    <asp:Repeater ID="rptCategoryBooks" runat="server">
        <ItemTemplate>
            <div class="textbook">
                ,<asp:LinkButton ID="lnkCover" runat="server" 
CommandName="ViewBook" 
CommandArgument='<%# Eval("bookISBN") %>'>
                <asp:Image runat="server"
                    ImageUrl='<%# ResolveUrl(Eval("coverImage").ToString()) %>'
                    CssClass="book-img" />
                    </asp:LinkButton>

                <div class="book-info">
                    <h4><%# Eval("title") %></h4>
                    <p class="book-price">R<%# Eval("price") %></p>
                </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>

        <div class="book-grid">
    <asp:Repeater ID="rptBooks" runat="server" OnItemCommand="rptBooks_ItemCommand">
        <ItemTemplate>
            <div class="book-card">
                <asp:Image ID="imgCover" runat="server" ImageUrl='<%# Eval("coverImage") %>' Width="120" CssClass="book-image-sr" />
                <div class="book-info">
    <div class="book-title"><%# Eval("title") %></div>
    <div class="book-author">by <%# Eval("author") %></div>

 <div class="book-rating">
    <%# GetStarIcons(Convert.ToDouble(Eval("AvgRating"))) %>
    <asp:LinkButton ID="lnkReviews" runat="server"
        Text='<%# Convert.ToInt32(Eval("ReviewCount")) > 0 
            ? "(" + Eval("ReviewCount") + " reviews)" 
            : "(No reviews yet)" %>'
        CommandName="ViewReviews"
        CommandArgument='<%# Eval("bookISBN") %>'
        CssClass="review-link"
        CausesValidation="false"
        UseSubmitBehavior="false" />
</div>


    <div class="book-price-sr">R <%# Eval("price", "{0:N2}") %></div>
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
