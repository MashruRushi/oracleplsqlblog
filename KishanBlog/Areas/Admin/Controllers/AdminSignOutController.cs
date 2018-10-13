using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace KishanBlog.Areas.Admin.Controllers
{
    public class AdminSignOutController : Controller
    {
        // GET: Admin/AdminSignOut
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        public ActionResult SignOut()
        {
            if(Session["Email"] != null)
                 Session.Clear();

            return RedirectToAction("Login", "AdminLogin");
        }
    }
}