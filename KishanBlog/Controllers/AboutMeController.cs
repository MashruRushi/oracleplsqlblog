using System;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace KishanBlog.Controllers
{
    [OutputCache(Duration = 1200, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Server)]
    public class AboutMeController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: AboutMe
        [HttpGet]
        public ActionResult AboutMeIndex()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetAboutMeDescription",con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ViewBag.aboutblog = dt.Rows[0][2];
                ViewBag.aboutexperience = dt.Rows[1][2];                

                con.Close();
            }
                return View();
        }
    }
}