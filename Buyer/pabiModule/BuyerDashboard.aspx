<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="BuyerDashboard.aspx.cs" Inherits="NMU_BookTrade.WebForm10" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
   <div class="search-wrapper">
    <div class="categories-inline">
        <asp:Repeater ID="rptCategory1" runat="server" OnItemCommand="rptCategory_ItemCommand">
            <ItemTemplate>
                <asp:LinkButton runat="server"
                    CommandName="SelectCategory"
                    CommandArgument='<%# Eval("categoryName") %>'
                    CssClass="category-link">
                    <%# Eval("categoryName") %>
                </asp:LinkButton>
            </ItemTemplate>
        </asp:Repeater>

        <div class="search-bar-bd">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-bd" Placeholder="Search textbook title..." />
            <asp:Button ID="btnSearch" runat="server" Text="🔍" OnClick="btnSearch_Click" CssClass="search-btn" />
        </div>

        <asp:Repeater ID="rptCategory2" runat="server" OnItemCommand="rptCategory_ItemCommand">
            <ItemTemplate>
                <asp:LinkButton runat="server"
                    CommandName="SelectCategory"
                    CommandArgument='<%# Eval("categoryName") %>'
                    CssClass="category-link">
                    <%# Eval("categoryName") %>
                </asp:LinkButton>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>


<asp:Label ID="lblSearchResults" runat="server" CssClass="results-label" Font-Bold="true"></asp:Label>

    <div class="results-grid">
        <asp:Repeater ID="rptOutNow" runat="server">
            <ItemTemplate>
                <div class="textbook">
                    <img src='<%# ResolveUrl(Eval("coverImage").ToString()) %>' /><br />
                </div>
            </ItemTemplate>
        </asp:Repeater>
                <div class="out-now">OUT <br />NOW!</div>
    </div>

        <div class="section-title">Recently Added Textbooks!</div>
    <hr class="section-line" />
        <div>
            <asp:Repeater ID="rptRecentlyAdded" runat="server">
                <ItemTemplate>
                    <div class="textbook">
                        <img src='<%# ResolveUrl(Eval("coverImage").ToString()) %>' /><br />
                        <b><%# Eval("title") %></b><br />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

<footer class="footer">
    <div class="logo">
        <img src= '<%# ResolveUrl(Eval("NMUBOOKTRADE").ToString()) %>'/>
        <strong>NMU<br><span>BookTrade</span></strong>
        <p>Thanks for choosing us!</p>
    </div>
    <div class="footer-links">
        <div>
            <h4>Company</h4>
            <a href="#">Home</a><br>
            <a href="#">About</a><br>
            <a href="#">Book Conditions</a><br>
            <a href="#">Contact Support</a>
        </div>
        <div>
            <h4>Documentation</h4>
            <a href="#">FAQ</a><br>
            <a href="#">Privacy Policy</a>
        </div>
        <div>
            <h4>Social</h4>
            <a href="#">WhatsApp</a>
        </div>
    </div>
</footer>
</asp:Content>
