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
    public class AdminOracleTutorialsController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: Admin/AdminOracleTutorials
        [HttpGet]
        public ActionResult AdminOracleTutorialsCreate(int? Id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }

            OracleTutorials OT = new OracleTutorials();

            if (Id == null)
            {
                OT.PublishOn = DateTime.Now.ToString("MM dd yyyy hh:mm tt");
                ViewBag.IsEdit = false;
            }
            else
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetTutorialById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tutorialId", Id);
                    cmd.ExecuteNonQuery();

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        OT.TutorialId = Convert.ToInt16(dr["TutorialId"]);
                        OT.PublishOn = dr["PublishOn"].ToString();
                        OT.TutorialTabName = dr["TutorialTabName"].ToString();
                        OT.TutorialTitle = dr["TutorialTitle"].ToString();
                        OT.TutorialDescription = dr["TutorialDescription"].ToString();
                    }

                    ViewBag.IsEdit = true;
                    ViewBag.Id = Id;

                    con.Close();
                }
            }

            return View(OT);
        }

        [HttpPost]
        public ActionResult AdminOracleTutorialsCreate(OracleTutorials ot, bool IsEdit, int? id)
        {
            int result = 0;
            if (IsEdit)
            {
                using (SqlConnection con = new SqlConnection(cs))
                {

                    try
                    {
                        ot.TutorialId = Convert.ToInt16(id);
                        con.Open();
                        SqlCommand cmd = new SqlCommand("InsertUpdateOracleTutorial", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tutorialId", ot.TutorialId);
                        cmd.Parameters.AddWithValue("@tutorialTabName", ot.TutorialTabName);
                        cmd.Parameters.AddWithValue("@tutorialTitle", ot.TutorialTitle);
                        cmd.Parameters.AddWithValue("@tutorialDescription", ot.TutorialDescription);
                        cmd.Parameters.AddWithValue("@publishOn", ot.PublishOn);
                        cmd.Parameters.AddWithValue("@Views", ot.Views);
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
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
                ViewBag.Id = ot.TutorialId;

                ViewBag.Msg = "Tutorial Updated susseccful";
                ViewBag.Status = "alert-success";
            }
            else
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("InsertUpdateOracleTutorial", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tutorialId", ot.TutorialId);
                    cmd.Parameters.AddWithValue("@tutorialTabName", ot.TutorialTabName);
                    cmd.Parameters.AddWithValue("@tutorialTitle", ot.TutorialTitle);
                    cmd.Parameters.AddWithValue("@tutorialDescription", ot.TutorialDescription);
                    cmd.Parameters.AddWithValue("@publishOn", ot.PublishOn);
                    cmd.Parameters.AddWithValue("@Views", ot.Views);
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    result = cmd.ExecuteNonQuery();
                    con.Close();
                }

                if (result > 0)
                {
                    ViewBag.Msg = "Tutprial Added susseccful";
                    ViewBag.Status = "alert-success";
                }
                else
                {
                    ViewBag.Msg = "Oops! something went wrong.";
                    ViewBag.Status = "alert-danger";
                }

                ViewBag.IsEdit = false;
            }
            return View();
        }
    }
}