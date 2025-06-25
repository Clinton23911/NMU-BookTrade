<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SearchTextBook.aspx.cs" Inherits="NMU_BookTrade.SearchTextBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
   <%-- <div class="search-results-box">
            <div class="result-item">
                <img src="~/Images/accounting.jpg" class="result-thumbnail" />
                <div>
                    <div class="result-title">Pure Mathematics 1</div>
                    <div class="result-price">R 450.00</div>
                    <div class="result-rating">★ ★ ★ ☆ ☆ (3 reviews)</div>
                </div>
            </div>
        </div>

        <div class="out-now">OUT NOW!</div>

        <div class="section-title">Recently Added Textbooks!</div>
        <div class="book-row">
            <div class="book-card"><img src="~/Images/econ1.jpg" alt="Principles of Economics" /></div>
            <div class="book-card"><img src="~/Images/.jpg" alt="Python Workshop" /></div>
            <div class="book-card"><img src="~/Images/physics.jpg" alt="Physics" /></div>
            <div class="book-card"><img src="~/Images/management.jpg" alt="Management" /></div>
            <div class="book-card"><img src="~/Images/financial.jpg" alt="Financial Management" /></div>
        </div>--%>

 <div class="search-wrapper">

        <!-- Search Input -->
        <div class="search-controls">
            <asp:TextBox ID="txtSearch" runat="server" Placeholder="Enter book title..." CssClass="form-control" />
            <asp:Button ID="btnSearch" runat="server" Text="Search 🔍" OnClick="btnSearch_Click" />
        </div>

        <!-- Results -->
        <asp:Repeater ID="rptResults" runat="server">
            <HeaderTemplate>
                <div class="results-box">
            </HeaderTemplate>
            <ItemTemplate>
                <div class="result-item">
                    <img src='<%# Eval("coverImg") %>' alt="Book" />
                    <div>
                        <div class="result-title"><%# Eval("title") %></div>
                        <div class="result-price">R <%# Eval("price", "{0:N2}") %></div>
                        <div class="result-reviews">★ ★ ★ ☆ ☆ (<%# Eval("reviewsCount") %> reviews)</div>
                    </div>
                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Label ID="lblNoResults" runat="server" Text="" ForeColor="Red" Visible="false" />
    </div>
</asp:Content>
