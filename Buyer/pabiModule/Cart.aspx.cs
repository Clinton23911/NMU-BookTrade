using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadCart();
            LoadCategories();
        }

        private void LoadCart()
        {
            if (Session["buyerID"] == null)
            {
                lblTotal.Text = "0.00";
                rptCartItems.DataSource = null;
                rptCartItems.DataBind();
                return;
            }

            int buyerID = Convert.ToInt32(Session["buyerID"]);
            decimal total = 0;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"
            SELECT b.bookISBN AS bookISBN, 
                   b.title AS Title, 
                   b.price AS Price, 
                   b.condition AS Condition, 
                   ci.quantity AS Quantity, 
                   b.coverImage, 
                   s.SellerName AS SellerName, 
                   s.SellerSurname AS SellerSurname,
                   b.version AS Version
            FROM Cart c
            INNER JOIN CartItems ci ON c.cartID = ci.cartID
            INNER JOIN Book b ON b.bookISBN = ci.bookISBN
            INNER JOIN Seller s ON s.sellerID = b.sellerID
            WHERE c.buyerID = @buyerID", con);

                cmd.Parameters.Add("@buyerID", SqlDbType.Int).Value = buyerID;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        decimal price = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0;
                        int qty = row["Quantity"] != DBNull.Value ? Convert.ToInt32(row["Quantity"]) : 0;
                        total += price * qty;
                    }

                    rptCartItems.DataSource = dt;
                    rptCartItems.DataBind();

                    lblTotal.Text = total.ToString("0.00");
                }
                else
                {
                    // No items in the cart
                    rptCartItems.DataSource = null;
                    rptCartItems.DataBind();
                    lblTotal.Text = "0.00";
                }
            }
        }



        protected void LoadCategories()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT categoryName FROM Category", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                int splitIndex = dt.Rows.Count / 2;

                DataTable dt1 = dt.Clone();
                DataTable dt2 = dt.Clone();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i < splitIndex)
                        dt1.ImportRow(dt.Rows[i]);
                    else
                        dt2.ImportRow(dt.Rows[i]);
                }

                rptCategory1.DataSource = dt1;
                rptCategory1.DataBind();

                rptCategory2.DataSource = dt2;
                rptCategory2.DataBind();
            }
        }

        protected void rptCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectCategory")
            {
                string faculty = e.CommandArgument.ToString();
                Response.Redirect("~/Admin/GraceModule/ManageCategories.aspx?category=" + Server.UrlEncode(faculty));
            }

            if (e.Item.ItemIndex == 2)
            {
                PlaceHolder phSearchBar = (PlaceHolder)e.Item.FindControl("phSearchBar");
                if (phSearchBar != null)
                {
                    phSearchBar.Visible = true;
                }
            }
        }

        private void PerformSearch(string searchTerm)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 2 bookISBN, title, author, coverImage, price " + "FROM Book " + " WHERE title LIKE @SearchTerm OR author LIKE @SearchTerm", con);
                cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    lblSearchResults.Text = $"Search results for \"{searchTerm}\"";
                    lblSearchResults.Visible = true;
                    rptSearchResults.DataSource = dt;
                    rptSearchResults.DataBind();
                }
                else
                {
                    lblSearchResults.Text = $"No results found for \"{searchTerm}\"";
                    lblSearchResults.Visible = true;
                    rptSearchResults.DataSource = null;
                    rptSearchResults.DataBind();
                }

                pnlSearchResults.Visible = true;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                PerformSearch(searchTerm);
            }
            else
            {
                lblSearchResults.Visible = false;
                rptSearchResults.Visible = false;
                pnlSearchResults.Visible = false;
            }
            Response.Redirect("~/Buyer/pabiModule/SearchResult.aspx?query=" + Server.UrlEncode(searchTerm));
        }

      
        protected void btnPurchase_Click(object sender, EventArgs e)
        {
            if (Session["buyerID"] == null) return;

            int buyerID = Convert.ToInt32(Session["buyerID"]);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                con.Open();

                // Get the cartID for this buyer
                SqlCommand getCartCmd = new SqlCommand("SELECT cartID FROM Cart WHERE buyerID = @buyerID", con);
                getCartCmd.Parameters.AddWithValue("@buyerID", buyerID);
                int cartID = (int)getCartCmd.ExecuteScalar();

                // Get all books in the cart
                SqlCommand getCartItemsCmd = new SqlCommand(
                    "SELECT bookISBN FROM CartItems WHERE cartID = @cartID", con);
                getCartItemsCmd.Parameters.AddWithValue("@cartID", cartID);

                List<string> bookISBNs = new List<string>();
                using (SqlDataReader reader = getCartItemsCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bookISBNs.Add(reader["bookISBN"].ToString());
                    }
                }

                foreach (var bookISBN in bookISBNs)
                {
                    // Generate next saleID manually
                    SqlCommand getNextSaleIDCmd = new SqlCommand("SELECT ISNULL(MAX(saleID), 0) + 1 FROM Sale", con);
                    int nextSaleID = (int)getNextSaleIDCmd.ExecuteScalar();

                    // Get price + sellerID of the book in one query
                    SqlCommand getBookCmd = new SqlCommand("SELECT price, sellerID FROM Book WHERE bookISBN = @bookISBN", con);
                    getBookCmd.Parameters.AddWithValue("@bookISBN", bookISBN);

                    decimal amount = 0;
                    int sellerID = 0;
                    using (SqlDataReader bookReader = getBookCmd.ExecuteReader())
                    {
                        if (bookReader.Read())
                        {
                            amount = Convert.ToDecimal(bookReader["price"]);
                            sellerID = Convert.ToInt32(bookReader["sellerID"]);
                        }
                    }

                    // Insert into Sale
                    SqlCommand insertSaleCmd = new SqlCommand(
                        "INSERT INTO Sale (saleID, buyerID, sellerID, bookISBN, amount, saleDate) " +
                        "VALUES (@saleID, @buyerID, @sellerID, @bookISBN, @amount, @saleDate)", con);

                    insertSaleCmd.Parameters.AddWithValue("@saleID", nextSaleID);
                    insertSaleCmd.Parameters.AddWithValue("@buyerID", buyerID);
                    insertSaleCmd.Parameters.AddWithValue("@sellerID", sellerID);   // ✅ now valid
                    insertSaleCmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                    insertSaleCmd.Parameters.AddWithValue("@amount", amount);
                    insertSaleCmd.Parameters.AddWithValue("@saleDate", DateTime.Now);

                    insertSaleCmd.ExecuteNonQuery();

                    // Update CartItems to link the sale
                    SqlCommand updateCartItemsCmd = new SqlCommand(
                        "UPDATE CartItems SET saleID = @saleID WHERE cartID = @cartID AND bookISBN = @bookISBN", con);
                    updateCartItemsCmd.Parameters.AddWithValue("@saleID", nextSaleID);
                    updateCartItemsCmd.Parameters.AddWithValue("@cartID", cartID);
                    updateCartItemsCmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                    updateCartItemsCmd.ExecuteNonQuery();

                    // Mark book as unavailable
                    SqlCommand updateBookStatusCmd = new SqlCommand(
                        "UPDATE Book SET status = 'unavailable' WHERE bookISBN = @bookISBN", con);
                    updateBookStatusCmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                    updateBookStatusCmd.ExecuteNonQuery();
                }

                // Clear cart
                SqlCommand clearCartCmd = new SqlCommand("DELETE FROM CartItems WHERE cartID = @cartID", con);
                clearCartCmd.Parameters.AddWithValue("@cartID", cartID);
                clearCartCmd.ExecuteNonQuery();

                lblConfirmation.Text = "Purchase successful!";
                lblConfirmation.Visible = true;
                pnlPayment.Visible = false;
                pnlDetails.Visible = true;

                LoadCart();
                BindCartItems();

                SqlCommand getOrderDetailsCmd = new SqlCommand(@"
    SELECT s.saleID, s.saleDate, s.amount, ci.quantity, 
           b.title, b.coverImage, b.price, 
           se.sellerName, se.sellerSurname,
           bu.buyerAddress,
           DATEADD(DAY, 5, s.saleDate) AS estimatedDelivery
    FROM Sale s
    INNER JOIN Book b ON s.bookISBN = b.bookISBN
    INNER JOIN Seller se ON s.sellerID = se.sellerID
    INNER JOIN CartItems ci ON ci.saleID = s.saleID
    INNER JOIN Buyer bu ON s.buyerID = bu.buyerID
    WHERE s.buyerID = @buyerID AND CAST(s.saleDate AS DATE) = CAST(GETDATE() AS DATE)", con);

                getOrderDetailsCmd.Parameters.AddWithValue("@buyerID", buyerID);

                SqlDataAdapter da = new SqlDataAdapter(getOrderDetailsCmd);
                DataTable orderDetails = new DataTable();
                da.Fill(orderDetails);

                rptOrderDetails.DataSource = orderDetails;
                rptOrderDetails.DataBind();

                pnlDetails.Visible = true;


            }
        }

        protected void btnShowPayment_Click(object sender, EventArgs e)
        {
            pnlPayment.Visible = true;
        }


        protected void btnRemove_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string bookISBN = btn.CommandArgument;
            int buyerID = Convert.ToInt32(Session["buyerID"]);

            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Get the buyer's cartID
                int cartID = 0;
                string getCartIDQuery = "SELECT cartID FROM Cart WHERE buyerID = @buyerID";
                using (SqlCommand getCartCmd = new SqlCommand(getCartIDQuery, conn))
                {
                    getCartCmd.Parameters.AddWithValue("@buyerID", buyerID);
                    object result = getCartCmd.ExecuteScalar();
                    if (result != null)
                    {
                        cartID = Convert.ToInt32(result);
                    }
                    else
                    {
                        return; // No cart found
                    }
                }

                // Delete the book completely from CartItems
                string deleteQuery = "DELETE FROM CartItems WHERE cartID = @cartID AND bookISBN = @bookISBN";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@cartID", cartID);
                    deleteCmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                    deleteCmd.ExecuteNonQuery();
                }
            }

            // Refresh the cart repeater and cart count
            BindCartItems();
            UpdateCartCount();
        }
        

        protected void rptCartItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddl = (DropDownList)e.Item.FindControl("ddlQuantity");

                if (ddl != null)
                {
                    ddl.Items.Clear();
                    ddl.Items.Add(new ListItem("0 (Remove)", "0"));

                    for (int i = 1; i <= 10; i++)
                    {
                        ddl.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }

                    DataRowView drv = (DataRowView)e.Item.DataItem;
                    string qty = drv["Quantity"].ToString();

                    if (ddl.Items.FindByValue(qty) != null)
                    {
                        ddl.SelectedValue = qty;
                    }
                }
            }
        }


        protected void ddlQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            RepeaterItem item = (RepeaterItem)ddl.NamingContainer;

            HiddenField hfBookISBN = (HiddenField)item.FindControl("hfBookISBN");
            string bookISBN = hfBookISBN.Value;
            int newQuantity = Convert.ToInt32(ddl.SelectedValue);
            int buyerID = Convert.ToInt32(Session["buyerID"]);

            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                int cartID = 0;
                string getCartIDQuery = "SELECT cartID FROM Cart WHERE buyerID = @buyerID";
                using (SqlCommand getCartCmd = new SqlCommand(getCartIDQuery, conn))
                {
                    getCartCmd.Parameters.AddWithValue("@buyerID", buyerID);
                    object result = getCartCmd.ExecuteScalar();
                    if (result == null) return;
                    cartID = Convert.ToInt32(result);
                }

                if (newQuantity == 0)
                {
                    // Remove row
                    string deleteQuery = "DELETE FROM CartItems WHERE cartID = @cartID AND bookISBN = @bookISBN";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@cartID", cartID);
                        deleteCmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                        deleteCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Update quantity
                    string updateQuery = "UPDATE CartItems SET quantity = @qty WHERE cartID = @cartID AND bookISBN = @bookISBN";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@qty", newQuantity);
                        updateCmd.Parameters.AddWithValue("@cartID", cartID);
                        updateCmd.Parameters.AddWithValue("@bookISBN", bookISBN);
                        updateCmd.ExecuteNonQuery();
                    }
                }
            }

            // Refresh cart display
            LoadCart();
        }

        private void BindCartItems()
        {
            int buyerID = Convert.ToInt32(Session["buyerID"]);
            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = @"SELECT ci.bookISBN, ci.quantity, b.title, b.price, b.coverImage, b.condition, 
                         s.sellerName, s.sellerSurname, b.version
                         FROM CartItems ci
                         INNER JOIN Book b ON ci.bookISBN = b.bookISBN
                         INNER JOIN Seller s ON b.sellerID = s.sellerID
                         INNER JOIN Cart c ON ci.cartID = c.cartID
                         WHERE c.buyerID = @buyerID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@buyerID", buyerID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptCartItems.DataSource = dt;
                    rptCartItems.DataBind();

                    // Update total
                    decimal total = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        total += Convert.ToDecimal(row["price"]) * Convert.ToInt32(row["quantity"]);
                    }
                    lblTotal.Text = total.ToString("0.00");
                }
            }
        }

        private void UpdateCartCount()
        {
            int buyerID = Convert.ToInt32(Session["buyerID"]);
            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = @"SELECT SUM(quantity) FROM CartItems ci
                         INNER JOIN Cart c ON ci.cartID = c.cartID
                         WHERE c.buyerID = @buyerID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@buyerID", buyerID);
                    object result = cmd.ExecuteScalar();
                    int count = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                    
                }
            }
        }
    }
}