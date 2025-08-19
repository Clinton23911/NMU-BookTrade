<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ManageListings.aspx.cs" Inherits="NMU_BookTrade.Seller.ClintonModule.ManageListings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle_section" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="ml-container">
        <div class="ml-header">
            <h1 class="ml-title">Manage Listings</h1>
            <div class="ml-search-results">
                <asp:Label ID="lblSearchResults" runat="server" Text="Your Listings"></asp:Label>
                <asp:LinkButton ID="lnkViewAll" runat="server" CssClass="ml-view-all">View all results</asp:LinkButton>
            </div>
        </div>

        <!-- Listings Grid -->
        <div class="md-deliveries-container">
            <asp:GridView ID="gvListings" runat="server" AutoGenerateColumns="False" CssClass="md-deliveries-grid" DataKeyNames="bookISBN"
                OnRowCommand="gvListings_RowCommand">
                <Columns>
                    <asp:BoundField DataField="title" HeaderText="Title" HeaderStyle-CssClass="md-grid-header" ItemStyle-CssClass="md-grid-cell" />
                    <asp:BoundField DataField="author" HeaderText="Author" HeaderStyle-CssClass="md-grid-header" ItemStyle-CssClass="md-grid-cell" />
                    <asp:BoundField DataField="bookISBN" HeaderText="ISBN" HeaderStyle-CssClass="md-grid-header" ItemStyle-CssClass="md-grid-cell" />
                    <asp:BoundField DataField="price" HeaderText="Price" DataFormatString="R{0:N2}" HeaderStyle-CssClass="md-grid-header" ItemStyle-CssClass="md-grid-cell" />
                    <asp:BoundField DataField="condition" HeaderText="Condition" HeaderStyle-CssClass="md-grid-header" ItemStyle-CssClass="md-grid-cell" />
                    <asp:TemplateField HeaderText="Actions" HeaderStyle-CssClass="md-grid-header" ItemStyle-CssClass="md-grid-cell md-actions-cell">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditListing" CommandArgument='<%# Eval("bookISBN") %>' 
                                CssClass="md-grid-action" ToolTip="Edit Listing">
                                <i class="fas fa-edit"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteListing" CommandArgument='<%# Eval("bookISBN") %>' 
                                CssClass="md-grid-action" ToolTip="Delete Listing">
                                <i class="fas fa-trash"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div class="md-empty-message">
                        No listings found. Create your first listing to get started.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>

        <!-- Modal Popup for Edit/Delete -->
        <div id="editModal" class="modal-overlay">
            <div class="modal-box" style="max-width: 420px;">
                <h2>Edit Listing</h2>
                <div class="ml-form">
                    <div class="ml-form-row">
                        <label class="ml-label">Title:</label>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="ml-input"></asp:TextBox>
                    </div>
                    <div class="ml-form-row">
                        <label class="ml-label">Author:</label>
                        <asp:TextBox ID="txtAuthor" runat="server" CssClass="ml-input"></asp:TextBox>
                    </div>
                    <div class="ml-form-row">
                        <label class="ml-label">Book ISBN:</label>
                        <asp:TextBox ID="txtISBN" runat="server" CssClass="ml-input" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="ml-form-row">
                        <label class="ml-label">Price:</label>
                        <asp:TextBox ID="txtPrice" runat="server" CssClass="ml-input"></asp:TextBox>
                    </div>
                    <div class="ml-form-row">
                        <label class="ml-label">Category:</label>
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="ml-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                            <asp:ListItem Text="Select a category" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="ml-form-row">
                        <label class="ml-label">Genre:</label>
                        <asp:DropDownList ID="ddlGenre" runat="server" CssClass="ml-select" Enabled="false">
                            <asp:ListItem Text="Select a category first" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="ml-form-row">
                        <label class="ml-label">Condition:</label>
                        <asp:DropDownList ID="ddlCondition" runat="server" CssClass="ml-select">
                            <asp:ListItem Text="Excellent" Value="excellent"></asp:ListItem>
                            <asp:ListItem Text="Very Good" Value="very-good"></asp:ListItem>
                            <asp:ListItem Text="Good" Value="good"></asp:ListItem>
                            <asp:ListItem Text="Fair" Value="fair"></asp:ListItem>
                            <asp:ListItem Text="Poor" Value="poor"></asp:ListItem>
                        </asp:DropDownList>
                        <p class="ml-condition-desc">Select the option that best describes the state of your book</p>
                    </div>
                    <div class="ml-form-row">
                        <label class="ml-label">Cover Image:</label>
                        <div class="ml-cover-image-container" style="flex-direction:column;">
                            <asp:Image ID="imgCoverImage" runat="server" CssClass="ml-cover-image" Visible="false" onclick="showImageUpload()" Style="cursor:pointer;" />
                            <asp:Label ID="lblNoImage" runat="server" Text="No cover image available" CssClass="ml-no-image" Visible="false"></asp:Label>
                            <asp:FileUpload ID="fuCoverImage" runat="server" CssClass="ml-image-upload" Style="display:none;" onchange="previewImage(this)" />
                        </div>
                    </div>
                    <div class="ml-form-actions">
                        <asp:Button ID="btnUpdate" runat="server" Text="UPDATE ▼" CssClass="ml-btn ml-btn-update" OnClick="btnUpdate_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="DELETE ▼" CssClass="ml-btn ml-btn-delete" OnClick="btnDelete_Click" />
                        <button type="button" class="ml-btn ml-btn-delete" onclick="hideEditModal()">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function showEditModal() {
            document.getElementById('editModal').classList.add('show');
        }
        function hideEditModal() {
            document.getElementById('editModal').classList.remove('show');
        }
        function showImageUpload() {
            document.getElementById('<%= fuCoverImage.ClientID %>').click();
        }
        function previewImage(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById('<%= imgCoverImage.ClientID %>').src = e.target.result;
                    document.getElementById('<%= imgCoverImage.ClientID %>').style.display = 'block';
                    document.getElementById('<%= lblNoImage.ClientID %>').style.display = 'none';
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>
</asp:Content>