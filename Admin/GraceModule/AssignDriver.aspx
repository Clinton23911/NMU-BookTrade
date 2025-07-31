<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AssignDriver.aspx.cs" Inherits="NMU_BookTrade.WebForm14" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">

    <asp:GridView ID="gvDeliveries" runat="server" AutoGenerateColumns="false"
    OnRowCommand="gvDeliveries_RowCommand"
    OnRowDataBound="gvDeliveries_RowDataBound"
    DataKeyNames="deliveryID"
    CssClass="styled-table"
    GridLines="None">
    
    <HeaderStyle CssClass="table-header" />
    <RowStyle CssClass="table-row" />
    <AlternatingRowStyle CssClass="table-row-alt" />

    <Columns>
        <asp:BoundField DataField="BookTitle" HeaderText="Book" />
        <asp:BoundField DataField="SellerName" HeaderText="Seller" />
        <asp:BoundField DataField="BuyerName" HeaderText="Buyer" />
        <asp:BoundField DataField="PickupAddress" HeaderText="Pickup Address" />
        <asp:BoundField DataField="DeliveryAddress" HeaderText="Delivery Address" />
        <asp:BoundField DataField="deliveryDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />

        <asp:TemplateField HeaderText="Driver">
            <ItemTemplate>
                <asp:DropDownList ID="ddlDrivers" runat="server" CssClass="driver-dropdown"></asp:DropDownList>
            </ItemTemplate>

        </asp:TemplateField>

        <asp:TemplateField HeaderText="Delivery Date">
            

        <ItemTemplate>
            <asp:TextBox 
                ID="txtDeliveryDate" 
                runat="server" 
                Text='<%# Eval("deliveryDate", "{0:yyyy-MM-ddTHH:mm}") %>' 
                TextMode="DateTimeLocal" 
                CssClass="date-picker" />
        </ItemTemplate>



</asp:TemplateField>


        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:Button ID="btnAssign" runat="server" Text="Assign" CommandName="AssignDriver"
                    CommandArgument='<%# Eval("deliveryID") %>' CssClass="assign-btn" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>




</asp:Content>
