<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Reviews.aspx.cs" Inherits="NMU_BookTrade.WebForm7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
<div class="reviews-main">
        <h2>Product Reviews</h2>

  <div class="tabs">
    <asp:Button ID="btnShowPurchases" runat="server" Text="Your Purchases"
        CssClass="tab-btn" OnClick="btnShowPurchases_Click" />
    <asp:Button ID="btnShowHistory" runat="server" Text="Review History"
        CssClass="tab-btn" OnClick="btnShowHistory_Click" />
</div>

        <asp:Panel ID="pnlSummary" runat="server" Visible="false">
            <div class="average-rating">
                <asp:Label ID="lblAverageRating" runat="server" CssClass="avg-rating"></asp:Label>
                <asp:Label ID="lblTotalReviews" runat="server" CssClass="total-reviews"></asp:Label>
            </div>

            <asp:Repeater ID="rptBreakdown" runat="server">
                <ItemTemplate>
                    <div class="rating-breakdown">
                        <span><%# Eval("reviewRating") %> ★</span>
                        <div class="bar">
<%--                            <div class="fill" style='width:<%# Eval("percentage") %>%'></div>--%>
                        </div>
                        <span>(<%# Eval("CountReviews") %>)</span>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <div id="history" class="tab-content active">
                <asp:Repeater ID="rptReviews" runat="server">
                    <ItemTemplate>
                        <div class="review-history-item">
                            <span class="stars"><%# new string('★', (int)Eval("reviewRating")) %></span>
                            <strong><%# Eval("buyerName") %></strong>
                            <p><%# Eval("reviewComment") %></p>
                            <small>
                                <%# Eval("reviewDate", "{0:dd MMM yyyy}") %>
                                <%# (bool)Eval("Verified") ? " (Verified Buyer)" : "" %>
                            </small>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </asp:Panel>

       <!-- Purchases Tab -->
<asp:Panel ID="pnlPurchasesTab" runat="server" Visible="true">
    <h3>Your Purchases</h3>
    <asp:Repeater ID="rptPurchases" runat="server">
        <ItemTemplate>
            <div class="purchase-item">
                <asp:Image ID="imgPurchaseProduct" runat="server" ImageUrl='<%# Eval("coverImage") %>' Width="60" />
                <span class="product-name"><%# Eval("title") %></span>
                <asp:Button ID="btnWriteReview" runat="server" CssClass="btn"
                    Text="Write Review"
                    CommandArgument='<%# Eval("bookISBN") %>'
                    OnClick="btnWriteReview_Click" />
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>

<!-- Review History Tab -->
<asp:Panel ID="pnlHistoryTab" runat="server" Visible="false">
    <h3>Your Review History</h3>
    <asp:DropDownList ID="ddlReviewFilter" runat="server" AutoPostBack="true"
        OnSelectedIndexChanged="ddlReviewFilter_SelectedIndexChanged" />
    <asp:Repeater ID="rptReviewHistory" runat="server">
        <ItemTemplate>
            <div class="review-history-item">
                <span class="stars"><%# new string('★', (int)Eval("reviewRating")) %></span>
                <strong><%# Eval("title") %></strong>
                <p><%# Eval("reviewComment") %></p>
                <small><%# Eval("reviewDate", "{0:dd MMM yyyy}") %></small>
                <br />
              <asp:Button ID="btnDeleteReview" runat="server" Text="Delete" CommandArgument='<%# Eval("reviewID") %>' OnClick="btnDeleteReview_Click" OnClientClick="return confirm('Are you sure you want to delete this review?');" CssClass="delete-btn" />

            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>

<!-- Slide-in Review Panel (Hidden until Write Review is clicked) -->
<asp:Panel ID="pnlReviewPanel" runat="server" CssClass="side-panel" Visible="false">
    <div class="panel-content">
        <asp:Label ID="lblHeader" runat="server" Text="<strong>Write a Review</strong>" font ="16px"/>

        <div class="product-head">
            <asp:Image ID="imgProduct" runat="server" Width="64" />
            <asp:Label ID="lblProductName" runat="server" CssClass="product-name" />
        </div>

        <div class="field">
            <label>Choose a Rating</label>
            <asp:DropDownList ID="ddlRating" runat="server" CssClass="stars-select">
                <asp:ListItem Value="5">★★★★★</asp:ListItem>
                <asp:ListItem Value="4">★★★★☆</asp:ListItem>
                <asp:ListItem Value="3">★★★☆☆</asp:ListItem>
                <asp:ListItem Value="2">★★☆☆☆</asp:ListItem>
                <asp:ListItem Value="1">★☆☆☆☆</asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="field">
                        <asp:Label ID="lblFirstName" runat="server" CssClass="buyer-name" />
            <asp:TextBox ID="txtReviewComment" runat="server" TextMode="MultiLine" Rows="5" Width="100%" Placeholder="Your review..."></asp:TextBox>
        </div>

        <asp:HiddenField ID="hfBookISBN" runat="server" />
        <asp:Button ID="btnSubmitReview" runat="server" Text="Submit Review" CssClass="btn" OnClick="btnSubmitReview_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="form-button" OnClick="btnClear_Click" CausesValidation="false"/>
    </div>
</asp:Panel>
</div>

       </asp:Content>