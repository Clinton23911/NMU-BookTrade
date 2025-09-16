<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="BuyerOrders.aspx.cs" Inherits="NMU_BookTrade.BuyerOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
    <div class="orders">
        <h2>My Orders</h2>

        <div class="order-filter">
            <p>Orders placed in:</p>
            <asp:DropDownList ID="orderDate" runat="server" AutoPostBack="true"
                OnSelectedIndexChanged="dateSelected_SelectedIndexChanged"></asp:DropDownList>
        </div>

        <asp:Repeater ID="rptOrders" runat="server" OnItemCommand="rptOrders_ItemCommand" OnItemDataBound="rptOrders_ItemDataBound">
            <ItemTemplate>
                <!-- Summary View -->
                <div class="order-summary">
                    <img src='<%# ResolveUrl(Eval("coverImage").ToString()) %>' class="order-thumbnail" />
                    <p class="order-title"><%# Eval("title") %></p>
                    <asp:Button ID="btnViewDetails" runat="server"
                        CommandName="ToggleDetails"
                        CommandArgument='<%# Eval("saleID") %>'
                        Text="Order Details" CssClass="btn-view-details" />
                </div>

                <!-- Details Panel -->
                <asp:Panel ID="pnlDetails" runat="server" CssClass="order-details hidden-panel">
                    <div class="order-card">
                        <!-- Order Details Card -->
                        <div class="order-info-card">
                            <div class="order-header">
                                <strong>Order #<%# Eval("saleID") %></strong> | 
                                Ordered: <%# Eval("saleDate", "{0:dd MMM yyyy}") %> | 
                                Paid: <%# Eval("saleDate", "{0:dd MMM yyyy}") %>
                            </div>
                            <p><strong>Shipping To:</strong> <%# Eval("buyerAddress") %></p>
                            <p><strong>Payment Method:</strong> Credit/Debit Card</p>
                            <div class="order-items">
                                <img src='<%# ResolveUrl(Eval("coverImage").ToString()) %>' class="order-image-large" />
                                <div>
                                    <p><%# Eval("title") %> (Qty: <%# Eval("quantity") %>)</p>
                                    <p><strong>Price:</strong> R<%# Eval("price", "{0:F2}") %></p>
                                    <p><strong>Seller:</strong> <%# Eval("sellerName") + " " + Eval("sellerSurname") %></p>
                                </div>
                            </div>
                            <p><strong>Order Total:</strong> R<%# Eval("price", "{0:F2}") %></p>
                        </div>

                        <!-- Delivery Details Card -->
                        <div class="delivery-info-card">
                            <div class="delivery-header">
                                <strong>Delivery Details</strong>
                                </br>
                                <%# Eval("deliveryDate") == DBNull.Value 
                                    ? "<span class='status-pending'>Delivery Pending</span>" 
                                    : "<span class='status-delivered'>Delivered: " + string.Format("{0:dd MMM yyyy}", Eval("deliveryDate")) + "</span>" %>
<%--                                <%# Eval("deliveryDate") != DBNull.Value ? "<br />Signed by: Paballo (Customer)" : "" %>--%>
                            </div>
                            <asp:Button ID="btnTrack" runat="server"
                                CommandName="Track"
                                CommandArgument='<%# Eval("saleID") %>'
                                Text="Track Delivery" CssClass="btn-track"
                                Visible='<%# Eval("deliveryDate") == DBNull.Value %>' />
                        </div>
                    </div>
                </asp:Panel>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>