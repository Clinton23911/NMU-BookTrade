<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SellerDashboard.aspx.cs" Inherits="NMU_BookTrade.SellerDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
    <div class="sd-container">
        <div class="sd-header">
            <h1 class="sd-welcome">Welcome <asp:Label ID="lblUsername" runat="server" Text="Seller"></asp:Label>!</h1>
            <p class="sd-subtitle">START LISTING YOUR BOOKS OR MANAGING YOUR LISTINGS!</p>
        </div>

        <div class="sd-stats">
            <div class="sd-stat-card">
                <div class="sd-stat-value"><asp:Label ID="lblActiveListings" runat="server" Text="12"></asp:Label></div>
                <div class="sd-stat-label">Active Listings</div>
            </div>
            <div class="sd-stat-card">
                <div class="sd-stat-value"><asp:Label ID="lblSoldBooks" runat="server" Text="24"></asp:Label></div>
                <div class="sd-stat-label">Books Sold</div>
            </div>
            <div class="sd-stat-card">
                <div class="sd-stat-value"><asp:Label ID="lblAvgRating" runat="server" Text="4.8"></asp:Label></div>
                <div class="sd-stat-label">Average Rating</div>
            </div>
        </div>

        <div class="sd-actions">
            <asp:HyperLink ID="lnkCreateListing" runat="server" CssClass="sd-action-btn sd-primary" NavigateUrl="~/CreateListings.aspx">
                <i class="fas fa-plus-circle"></i> Create New Listing
            </asp:HyperLink>
            <asp:HyperLink ID="lnkManageListings" runat="server" CssClass="sd-action-btn sd-secondary" NavigateUrl="~/ManageListings.aspx">
                <i class="fas fa-edit"></i> Manage Listings
            </asp:HyperLink>
        </div>

        <div class="sd-recent-activity">
            <h2 class="sd-section-title">Recent Activity</h2>
            <asp:Repeater ID="rptRecentActivity" runat="server">
                <ItemTemplate>
                    <div class="sd-activity-item">
                        <div class="sd-activity-icon">
                            <i class="fas fa-<%# Eval("Icon") %>"></i>
                        </div>
                        <div class="sd-activity-details">
                            <div class="sd-activity-text"><%# Eval("Message") %></div>
                            <div class="sd-activity-time"><%# Eval("Time") %></div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="sd-footer">
            <div class="sd-footer-section">
                <h3>NMU BookTrade</h3>
                <p>Thanks for choosing us!</p>
            </div>
            <div class="sd-footer-section">
                <h3>Company</h3>
                <ul>
                    <li><a href="Home.aspx">Home</a></li>
                    <li><a href="CreateListings.aspx">Create Listings</a></li>
                    <li><a href="ManageListings.aspx">Manage Listings</a></li>
                    <li><a href="Contact.aspx">Contact Support</a></li>
                </ul>
            </div>
            <div class="sd-footer-section">
                <h3>Documentation</h3>
                <ul>
                    <li><a href="FAQ.aspx">FAQ</a></li>
                    <li><a href="PrivacyPolicy.aspx">Privacy Policy</a></li>
                </ul>
            </div>
            <div class="sd-footer-section">
                <h3>Social</h3>
                <a href="#" class="sd-social-icon"><i class="fab fa-whatsapp"></i></a>
            </div>
        </div>
    </div>
</asp:Content>