using System;
using System.Web;

namespace NMU_BookTrade
{
    public static class AuthorizationHelper
    {
        // This method checks if a user is logged in and has the correct access role
        public static void Authorize(string requiredAccessID)
        {
            // Step 1: Check if user is logged in
            if (HttpContext.Current.Session["AccessID"] == null)
            {
                // Not logged in → redirect to login
                HttpContext.Current.Response.Redirect("~/UserManagement/Login.aspx");
                return;
            }

            // Step 2: Check if the current user's AccessID matches the required role
            string currentAccessID = HttpContext.Current.Session["AccessID"].ToString();

            if (currentAccessID != requiredAccessID)
            {
                // Wrong access level → send back to login page
                HttpContext.Current.Response.Redirect("~/UserManagement/Login.aspx");
                return;
            }
        }
    }
}
