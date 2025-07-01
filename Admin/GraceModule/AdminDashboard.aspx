<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="NMU_BookTrade.AdminDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">

     <h2 class="section-heading">Available Drivers</h2>
<div class="driver-grid">
    <asp:Repeater ID="rptDrivers" runat="server">
        <ItemTemplate>
            <div class="driver-card" onclick="location.href='<%# Eval("driverID", "ViewDriver.aspx?id={0}") %>'">
                <img src='<%# ResolveUrl("~/UploadedImages/" + Eval("driverProfileImage")) %>' alt="Driver" class="driver-img" />
                <div class="driver-name"><%# Eval("driverName") %> <%# Eval("driverSurname") %></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <!-- Add Driver Card -->
    <div class="driver-card add-card" onclick="location.href='AddDriver.aspx'">
        <div class="add-icon">+</div>
        <div class="driver-name">Add Driver</div>
    </div>
</div>

    

</asp:Content>
