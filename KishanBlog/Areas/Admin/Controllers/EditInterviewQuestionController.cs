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
    public class EditInterviewQuestionController : Controller
    {
        // GET: Admin/EditInterviewQuestion
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        public ActionResult EditInterviewQuestionIndex()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetInterviewQuestionList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ViewBag.InterviewQuestionList = dt;
                con.Close();
            }

            return View();
        }

        public ActionResult DeleteInterviewQuestion(int Id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DeleteInterviewQuestion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@interviewQuestionId", Id);
                cmd.ExecuteNonQuery();

                SqlCommand cmd1 = new SqlCommand("GetInterviewQuestionList", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.ExecuteNonQuery();

                DataTable dt1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(dt1);

                ViewBag.InterviewQuestionList = dt1;
                con.Close();
            }
                return View("EditInterviewQuestionIndex");
        }
    }
}