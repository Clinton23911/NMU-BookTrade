<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ManageReviews.aspx.cs" Inherits="NMU_BookTrade.Admin.GraceModule.ManageReviews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">

     

  <div class="rv-card">
    <div class="rv-header">
      <h2>Reviews Moderation</h2>
      <div class="rv-actions">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="rv-input" Placeholder="Search comment..."></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="rv-btn" OnClick="btnSearch_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="rv-btn ghost" OnClick="btnClear_Click" />
      </div>
    </div>

    <asp:GridView ID="gvReviews" runat="server" AutoGenerateColumns="False" DataKeyNames="reviewID"
                  CssClass="rv-grid" AllowPaging="true" PageSize="12"
                  OnPageIndexChanging="gvReviews_PageIndexChanging"
                  OnRowCommand="gvReviews_RowCommand">
      <Columns>
        <asp:BoundField DataField="reviewID" HeaderText="ID" />
        <asp:BoundField DataField="reviewDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
        <asp:BoundField DataField="BuyerName" HeaderText="Buyer" />
        <asp:BoundField DataField="reviewRating" HeaderText="★" />
        <asp:BoundField DataField="reviewComment" HeaderText="Comment" />
        <asp:TemplateField HeaderText="Actions">
          <ItemTemplate>
            <asp:Button runat="server" Text="Remove" CssClass="rv-btn danger"
              CommandName="Remove" CommandArgument='<%# Eval("reviewID") %>'
              OnClientClick="return confirm('Permanently delete this review?');" />
          </ItemTemplate>
        </asp:TemplateField>
      </Columns>
    </asp:GridView>
  </div>
</asp:Content>
