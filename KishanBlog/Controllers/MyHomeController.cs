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
    public class MyHomeController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: MyHome
        [HttpGet]
        public ActionResult Index()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("GetBlogs",con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ViewBag.BlogList = dt;

                SqlCommand cmd1 = new SqlCommand("GetVideos", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.ExecuteNonQuery();

                DataTable dt1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(dt1);

                ViewBag.VideoList = dt1;

                SqlCommand cmd2 = new SqlCommand("GetTutorials", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.ExecuteNonQuery();

                DataTable dt2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                da2.Fill(dt2);

                ViewBag.TutorialList = dt2;

                SqlCommand cmd3 = new SqlCommand("GetInterviewQuestions", con);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.ExecuteNonQuery();


                DataTable dt3 = new DataTable();
                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                da3.Fill(dt3);

                ViewBag.InterviewQuestionList = dt3;

                SqlCommand cmd4 = new SqlCommand("GetRandomTipsNTrick", con);
                cmd4.CommandType = CommandType.StoredProcedure;
                cmd4.ExecuteNonQuery();


                DataTable dt4 = new DataTable();
                SqlDataAdapter da4 = new SqlDataAdapter(cmd4);
                da4.Fill(dt4);

                foreach(DataRow dr in dt4.Rows)
                {
                    ViewBag.TipsNTrick = dr["TipsNTrickTitle"];
                }

            }
                return View();
        }
    }
}