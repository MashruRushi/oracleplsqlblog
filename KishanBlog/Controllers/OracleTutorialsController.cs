using System;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace KishanBlog.Controllers
{
    [OutputCache(Duration = 600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Server)]
    public class OracleTutorialsController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: OracleTutorials
        public ActionResult OracleTutorialsIndex()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetAllTutorials", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ViewBag.OTData = dt;
                con.Close();
            }
            return View();
        }
    }
}