using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KishanBlog.Models;

namespace KishanBlog.Controllers
{
    public class InterviewQuestionController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: InterviewQuestion
        [HttpGet]
        public ActionResult InterviewQuestionIndex(InterviewQuestions iq)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetAllInterviewQuestion", con);
                cmd.CommandType = CommandType.StoredProcedure;                
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ViewBag.QAData = dt;
                con.Close();
            }

                return View();
        }
    }
}