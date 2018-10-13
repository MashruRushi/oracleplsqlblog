using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KishanBlog.Models;

namespace KishanBlog.Areas.Admin.Controllers
{
    public class AdminInterviewQuestionController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: Admin/AdminInterviewQuestion
        [HttpGet]
        public ActionResult CreateInterviewQuestion(int? Id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }

            InterviewQuestions iq = new InterviewQuestions();

            if (Id == null)
            {
                iq.PublishOn = DateTime.Now.ToString("MMM dd yyyy hh:mm tt");
                ViewBag.IsEdit = false;
            }
            else
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetInterviewQuestionById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@interviewQuestionId", Id);
                    cmd.ExecuteNonQuery();

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        iq.InterviewQuestionId = Convert.ToInt32(dr["InterviewQuestionId"]);
                        iq.InterviewQuestion = dr["InterviewQuestion"].ToString();
                        iq.InterviewQuestionLongDescription = dr["InterviewQuestionLongDescription"].ToString();
                        iq.PublishOn = dr["PublishOn"].ToString();
                    }

                    ViewBag.IsEdit = true;
                    ViewBag.Id = Id;
                }
            }
            return View(iq);
        }

        [HttpPost]
        public ActionResult CreateInterviewQuestion(InterviewQuestions iq, bool IsEdit, int? Id)
        {

            if (IsEdit)
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    try
                    {
                        con.Open();
                        iq.InterviewQuestionId = Convert.ToInt32(Id);
                        SqlCommand cmd = new SqlCommand("InsertUpdateInterviewQuestion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@interviewQuestionId", iq.InterviewQuestionId);
                        cmd.Parameters.AddWithValue("@interviewQuestion", iq.InterviewQuestion);
                        cmd.Parameters.AddWithValue("@interviewQuestionLongDescription", iq.InterviewQuestionLongDescription);
                        cmd.Parameters.AddWithValue("@createdOn", iq.CreatedOn);
                        cmd.Parameters.AddWithValue("@modifiedOn", iq.ModifiedOn);
                        cmd.Parameters.AddWithValue("@publishOn", iq.PublishOn);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Msg = "Oops! something went wrong";
                        ViewBag.Status = "alert-danger";
                    }
                }
                ViewBag.IsEdit = true;
                ViewBag.Id = iq.InterviewQuestionId;

                ViewBag.Msg = "Interview Question and Answer Updated susseccful";
                ViewBag.Status = "alert-success";
            }
            else
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    try
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("InsertUpdateInterviewQuestion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@interviewQuestionId", iq.InterviewQuestionId);
                        cmd.Parameters.AddWithValue("@interviewQuestion", iq.InterviewQuestion);
                        cmd.Parameters.AddWithValue("@interviewQuestionLongDescription", iq.InterviewQuestionLongDescription);
                        cmd.Parameters.AddWithValue("@createdOn", iq.CreatedOn);
                        cmd.Parameters.AddWithValue("@modifiedOn", iq.ModifiedOn);
                        cmd.Parameters.AddWithValue("@publishOn", iq.PublishOn);
                        cmd.ExecuteNonQuery();
                        con.Close();


                        ViewBag.Msg = "Interview Question and Answer added susseccful";
                        ViewBag.Status = "alert-success";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Msg = "Oops! something went wrong";
                        ViewBag.Status = "alert-danger";
                    }
                }

                ViewBag.IsEdit = false;
            }
            return View();
        }
    }
}