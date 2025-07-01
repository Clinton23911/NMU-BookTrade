<%--<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SearchTextBook.aspx.cs" Inherits="NMU_BookTrade.SearchTextBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
 <div class="search-wrapper">
<!-- 🔍 Search -->
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" Placeholder="Search for book title"></asp:TextBox>
             <button class="search-btn">🔍</button>


            <br /><br />

            <!-- 🔎 Show search result header -->
            <asp:Label ID="lblSearchResults" runat="server" Font-Bold="true"></asp:Label>
            <asp:LinkButton ID="lnkViewAllResults" runat="server" OnClick="lnkViewAllResults_Click">
                View all results <i class="fas fa-arrow-right"></i>
            </asp:LinkButton>

            <!-- 🏷️ Categories (Faculties) -->
            <div>
                <asp:Repeater ID="rptCategory" runat="server" OnItemCommand="rptCategory_ItemCommand">
                    <ItemTemplate>
                        <span class="category">
                            <asp:LinkButton runat="server" CommandName="SelectFaculty" CommandArgument='<%# Eval("categoryName") %>'>
                                <%# Eval("categoryName") %>
                            </asp:LinkButton>
                        </span>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- 🚨 OUT NOW SECTION -->
            <div class="section-title">OUT NOW!</div>
            <asp:Repeater ID="rptOutNow" runat="server">
                <ItemTemplate>
                    <div class="textbook">
                        <img src='<%# Eval("coverImg") %>'/><br />
                        <b><%# Eval("title") %></b><br />
                        R<%# Eval("price") %>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <!-- 🆕 RECENTLY ADDED -->
            <div class="section-title">Recently Added Textbooks!</div>
            <asp:Repeater ID="rptRecentlyAdded" runat="server">
                <ItemTemplate>
                    <div class="textbook">
                        <img src='<%# Eval("coverImg") %>' /><br />
                        <b><%# Eval("title") %></b><br />
                        R<%# Eval("price") %>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

        </div>
</asp:Content>--%>
