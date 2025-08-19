using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class WebForm14 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDeliveries();
                LoadAssignedRecords();
            }
        }
        private void LoadDeliveries()// hear we are loading deliveries that still need drivers
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {// we are loading deliveries with books that are sold but not assigned yet to the driver.  

                string query = @"
                            SELECT
                                sa.saleID,
                                b.title AS BookTitle,
                                s.sellerName,
                                s.sellerAddress AS PickupAddress,
                                bu.buyerName,
                                bu.buyerAddress AS DeliveryAddress,
                                del.deliveryID,      -- may be NULL
                                del.status,
                                del.deliveryDate
                            FROM Sale sa
                            JOIN Book   b  ON sa.bookISBN = b.bookISBN
                            JOIN Seller s  ON sa.sellerID = s.sellerID
                            JOIN Buyer  bu ON sa.buyerID = bu.buyerID
                            LEFT JOIN Delivery del ON del.saleID = sa.saleID
                            WHERE del.driverID IS NULL      -- also matches rows where del is NULL
                            ORDER BY sa.saleDate DESC";


                // the dataAdapter executes this query and we then fill in the `
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                // the information is placed in the grid view 
                gvDeliveries.DataSource = dt;
                gvDeliveries.DataBind();
            }
        }




        // It's used here to load the driver list into each dropdown in the grid
        protected void gvDeliveries_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Only work on data rows (ignore header/footer rows)
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Find the DropDownList inside the current row
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlDrivers");

                // Fetch the list of drivers from the database
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT driverID, driverName + ' ' + driverSurname AS FullName FROM Driver", con);
                    con.Open();

                    // Use a data reader to populate the dropdown
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddl.DataSource = reader;
                    ddl.DataTextField = "FullName"; // What users see
                    ddl.DataValueField = "driverID"; // What gets saved
                    ddl.DataBind();
                }
            }
        }


        // This handles the "Assign" button click in each row
        protected void gvDeliveries_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AssignDriver")
            {

                //we are picking the index of the row  
                int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
                GridViewRow row = gvDeliveries.Rows[rowIndex];

                DropDownList ddl = (DropDownList)row.FindControl("ddlDrivers");
                TextBox txtDate = (TextBox)row.FindControl("txtDeliveryDate");

                string deliveryID = e.CommandArgument.ToString();
                string driverID = ddl.SelectedValue;

                DateTime deliveryDate;
                if (!DateTime.TryParse(txtDate.Text, out deliveryDate))
                {
                    // Show error or fallback to current date + 2 days
                    deliveryDate = DateTime.Now.AddDays(2).Date.AddHours(10);
                }


                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Delivery SET driverID = @driverID, status = 1, deliveryDate = @date WHERE deliveryID = @deliveryID", con);

                    cmd.Parameters.AddWithValue("@driverID", driverID);
                    cmd.Parameters.AddWithValue("@deliveryID", deliveryID);
                    cmd.Parameters.AddWithValue("@date", deliveryDate);

                    cmd.ExecuteNonQuery();
                }

                LoadDeliveries();
                LoadAssignedRecords();
            }
        }

        // Load deliveries that were already assigned to drivers (history or confirmation)
        protected void LoadAssignedRecords()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ConnectionString))
            {
                                // we are only fetch deliveries where a driver has already been assigned (driverID is NOT null)
                                string query = @"
                SELECT 
                    b.title AS BookTitle,
                    s.sellerName AS SellerName,
                    bu.buyerName AS BuyerName,
                    d.driverName + ' ' + d.driverSurname AS DriverName,
                    del.deliveryDate
                FROM Delivery del
                JOIN Sale   sa ON del.saleID  = sa.saleID
                JOIN Book    b ON sa.bookISBN = b.bookISBN
                JOIN Seller  s ON sa.sellerID = s.sellerID
                JOIN Buyer  bu ON sa.buyerID = bu.buyerID
                JOIN Driver  d ON del.driverID = d.driverID
                WHERE del.driverID IS NOT NULL
                ORDER BY del.deliveryDate DESC";


                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvAssignedDrivers.DataSource = dt;
                gvAssignedDrivers.DataBind();
            }

        }
    }
}