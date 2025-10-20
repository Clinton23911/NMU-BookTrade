<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ViewTextBookDetails.aspx.cs" Inherits="NMU_BookTrade.ViewTextBookDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
     <div class="reviews-title">
        <h2>Textbook Details</h2>
    </div>
    <hr class="section-line" />

    <div class="details-container">

        <!-- Cover -->
        <div class="book-cover">
            <asp:Image ID="imgBookCover" runat="server" AlternateText="Book Cover" />
        </div>

        <!-- Info -->
        <div class="book-info">
            <h2><asp:Label ID="lblTitle" runat="server" /></h2>
            <p class="book-meta">Author: <asp:Label ID="lblAuthor" runat="server" /></p>
            <p class="book-meta">Edition: <asp:Label ID="lblEdition" runat="server" /></p>
            <p class="book-meta">ISBN: <asp:Label ID="lblISBN" runat="server" /></p>
            <p class="book-meta">Condition: <asp:Label ID="lblCondition" runat="server" /></p>
            <p class="price">R<asp:Label ID="lblPrice" runat="server" /></p>
            <p class="availability"><asp:Label ID="lblAvailability" runat="server" /></p>

            <!-- Seller Info -->
            <div class="seller-box">
                <h4>Seller Information</h4>
                <p><strong>Name:</strong> <asp:Label ID="lblSellerName" runat="server" /></p>
                <p><strong>Email:</strong> <asp:Label ID="lblSellerEmail" runat="server" /></p>
            </div>

            <!-- Write Review Button -->
            <asp:Button ID="btnWriteReview" runat="server" Text="Write a Review" CssClass="btn-write-review" OnClick="btnWriteReview_Click" />
        </div>
    </div>

    <!-- Reviews Section -->
    <div class="reviews-summary">
        <h4>Ratings & Reviews</h4>
        <p><span class="stars">★★★★★</span>
            <asp:Label ID="lblAverageRating" runat="server" /> average rating based on 
            <asp:Label ID="lblTotalReviews" runat="server" /> reviews</p>

        <asp:Repeater ID="rptBookReviews" runat="server">
            <ItemTemplate>
                <div class="review-item">
                    <strong><%# Eval("buyerName") %></strong>
                    <span class="stars"><%# new string('★', (int)Eval("reviewRating")) %></span><br />
                    <p><%# Eval("reviewComment") %></p>
                    <small><%# Eval("reviewDate", "{0:dd MMM yyyy}") %></small>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

</asp:Content>