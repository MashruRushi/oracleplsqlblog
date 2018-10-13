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
    public class EditTipsNTrickController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: Admin/EditTipsNTrick
        public ActionResult EditTipsNTrickIndex()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetTipsNTrickList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ViewBag.TipsNTrickList = dt;
                con.Close();
            }

            return View();
        }
        public ActionResult DeleteTipsNTrick(int Id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DeleteTipsNTrick", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipsNtrickId", Id);
                cmd.ExecuteNonQuery();

                SqlCommand cmd1 = new SqlCommand("GetTipsNTrickList", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.ExecuteNonQuery();

                DataTable dt1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(dt1);

                ViewBag.TipsNTrickList = dt1;
                con.Close();
            }
            return View("EditTipsNTrickIndex");
        }
    }
}