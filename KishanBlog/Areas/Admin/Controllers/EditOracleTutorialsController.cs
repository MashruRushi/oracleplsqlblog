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
    public class EditOracleTutorialsController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: Admin/EditOracleTutorials
        [HttpGet]
        public ActionResult OracleTutorialList()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetAllTutorialsList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ViewBag.OracleTutorialsList = dt;
                con.Close();
            }
            return View();
        }

        public ActionResult DeleteTutorial(int Id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DeleteTutorial", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tutorialId", Id);
                cmd.ExecuteNonQuery();

                SqlCommand cmd1 = new SqlCommand("GetAllTutorialsList", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.ExecuteNonQuery();

                DataTable dt1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(dt1);

                ViewBag.OracleTutorialsList = dt1;
                con.Close();

                return View("OracleTutorialList");
            }
        }

        public JsonResult UpdateDisplayOrder(string TutorialIDs, string DisplayOrder)
        {
            bool Status = false;
            if (Session["Email"] != null)
            {                
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UpdateDisplayOrder", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tutorialIds", TutorialIDs);
                    cmd.ExecuteNonQuery();
                    Status = true;
                }
            }
            return Json(new { Status });
        }
    }
}