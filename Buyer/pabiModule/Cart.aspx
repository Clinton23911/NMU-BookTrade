<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="NMU_BookTrade.Cart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">

     <div class="categories-inline">
    <asp:Repeater ID="rptCategory" runat="server" OnItemCommand="rptCategory_ItemCommand">
         <ItemTemplate>
             <asp:LinkButton runat="server" CommandName="SelectFaculty" CommandArgument='<%# Eval("categoryName") %>' CssClass="category-link">
                 <%# Eval("categoryName") %>
             </asp:LinkButton>
         </ItemTemplate>
     </asp:Repeater>
 </div>
<asp:Panel ID="pnlCart" runat="server" CssClass="cart-container">
    <div class="cart-header">
    <h2>CART</h2>
    <hr class="section-line" />
</div>
    <asp:Repeater ID="rptCartItems" runat="server">
        <ItemTemplate>
            <div class="cart-item">
                <img src='<%# Eval("coverImage") %>' class="cart-item-img" />
                <div class="cart-item-details">
                    <h4><%# Eval("title") %></h4>
                    <p>Qty: <%# Eval("quantity") %> | Price: R<%# Eval("price") %></p>
                    <asp:Button ID="btnRemove" runat="server" Text="🗑️ Remove"
                    CommandArgument='<%# Eval("bookISBN") %>'
                    OnClick="btnRemove_Click"
                    CssClass="remove-btn" />
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <div class="cart-summary">
        <strong>Total: </strong> R<asp:Label ID="lblTotal" runat="server" Text="0.00" />
    </div>

    <asp:Button ID="btnShowPayment" runat="server" Text="Proceed to Checkout" OnClick="btnShowPayment_Click" CssClass="checkout-btn" />

    <asp:Panel ID="pnlPayment" runat="server" Visible="false" CssClass="checkout-section">
        <h3>Ready to Pay?</h3>
        <asp:Label AssociatedControlID="txtName" Text="Name on Card:" runat="server" />
        <asp:TextBox ID="txtName" runat="server" CssClass="input" />

        <asp:Label AssociatedControlID="txtCard" Text="Card Number:" runat="server" />
        <asp:TextBox ID="txtCard" runat="server" CssClass="input" />

        <asp:Button ID="btnPurchase" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CssClass="checkout-btn" />
    </asp:Panel>

    <asp:Label ID="lblConfirmation" runat="server" Visible="false" ForeColor="Green" Font-Bold="true" />
</asp:Panel>

</asp:Content>
