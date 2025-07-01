using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NMU_BookTrade
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Only load drivers if it's the first time the page is loading (not on postbacks)
            if (!IsPostBack)
            {
                LoadDrivers();
            }
        }

        private void LoadDrivers()
        {
            // Get connection string from Web.config
            string connStr = ConfigurationManager.ConnectionStrings["NMUBookTradeConnection"].ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // SQL query to get all drivers and their profile images
                string query = "SELECT driverID, driverName, driverSurname, driverProfileImage FROM Driver";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();

                // Fill the DataTable with driver data
                da.Fill(dt);

                // Bind the data to the Repeater
                rptDrivers.DataSource = dt;
                rptDrivers.DataBind();
            }
        }
    }
}