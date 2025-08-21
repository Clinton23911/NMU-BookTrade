<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Reviews.aspx.cs" Inherits="NMU_BookTrade.WebForm7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
     <div class="reviews-main">
        <h2>Product Reviews</h2>

       <!-- Tab Navigation -->
    <div class="tabs">
        <button type="button" class="tab-btn active" onclick="showTab('purchasesTab')">Your Purchases</button>
        <button type="button" class="tab-btn" onclick="showTab('historyTab')">Review History</button>
    </div>
        <!-- Summary for selected product (shown after you click Write Review or when ?isbn=... is present) -->
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
                            <div class="fill" style='width:<%# Eval("percentage") %>%'></div>
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

        <!-- Your Purchases -->
        <div class="purchases">
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
        </div>

       <!-- Write Review Side Panel -->
<div id="reviewPanel" class="side-panel">
    <div class="panel-content">
        <span class="close" onclick="closePanel()">&times;</span>

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

        <div class="grid-2">
            <asp:TextBox ID="txtReviewTitle" runat="server" CssClass="field" Placeholder="Review Title" />
            <asp:Label ID="lblFirstName" runat="server" CssClass="field" />
        </div>

        <div class="field">
            <asp:TextBox ID="txtReviewComment" runat="server" TextMode="MultiLine" Rows="5" Width="100%" Placeholder="Your review..."></asp:TextBox>
        </div>

        <asp:HiddenField ID="hfBookISBN" runat="server" />
        <asp:Button ID="btnSubmitReview" runat="server" Text="Submit Review" CssClass="btn" OnClick="btnSubmitReview_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn secondary" OnClientClick="closePanel(); return false;" />
    </div>

    <!-- Review History -->
<div id="historyTab" class="tab-content">
    <h3>Your Review History</h3>
    <asp:Repeater ID="rptReviewHistory" runat="server">
        <ItemTemplate>
            <div class="review-history-item">
                <span class="stars"><%# new string('★', (int)Eval("reviewRating")) %></span>
                <strong><%# Eval("title") %></strong> 
                <p><%# Eval("reviewComment") %></p>
                <small><%# Eval("reviewDate", "{0:dd MMM yyyy}") %></small>
                <br />
                <asp:Button ID="btnDeleteReview" runat="server" CssClass="btn danger"
                    Text="Delete"
                    CommandArgument='<%# Eval("reviewID") %>'
                    OnClick="btnDeleteReview_Click" />
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

</div>
       <script type="text/javascript">
           function openPanel() {
               document.getElementById("reviewPanel").style.width = "400px";
           }

           function closePanel() {
               document.getElementById("reviewPanel").style.width = "0";
           }
       </script>

       <script>
           function showTab(tabId) {
          
     // Hide all tabs
               document.querySelectorAll('.tab-content').forEach(t => t.classList.remove('active'));
               document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));

               // Show selected tab
               document.getElementById(tabId).classList.add('active');
               event.target.classList.add('active');
           }
       </script>
</asp:Content>

