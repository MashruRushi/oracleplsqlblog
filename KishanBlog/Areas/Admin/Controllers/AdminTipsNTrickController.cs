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
    public class AdminTipsNTrickController : Controller
    {
        String cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: Admin/AdminTipsNTrick
        [HttpGet]
        public ActionResult AdminTipsNTrickIndex(int? Id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }

            TipsNTricks tt = new TipsNTricks();
            if (Id == null)
            {
                tt.PublishOn = DateTime.Now.ToString("MMM dd yyyy hh:mm tt");
                ViewBag.IsEdit = false;
            }
            else
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetTipsNTrickById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tipsNTrickId", Id);
                    cmd.ExecuteNonQuery();

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        tt.TipsNTrickId = Convert.ToInt32(dr["TipsNTrickId"]);
                        tt.TipsNTrickTitle = dr["TipsNTrickTitle"].ToString();
                        tt.TipsNTrickLongDescription = dr["TipsNTrickLongDescription"].ToString();
                        tt.PublishOn = dr["PublishOn"].ToString();
                    }

                    ViewBag.IsEdit = true;
                    ViewBag.Id = Id;
                }
            }

            return View(tt);
        }

        [HttpPost]

        public ActionResult AdminTipsNTrickIndex(TipsNTricks tt, bool IsEdit, int? Id)
        {
            if (IsEdit)
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    try
                    {
                        con.Open();
                        tt.TipsNTrickId = Convert.ToInt32(Id);
                        SqlCommand cmd = new SqlCommand("InsertUpdateTipsNTrick", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tipsNTrickId", tt.TipsNTrickId);
                        cmd.Parameters.AddWithValue("@tipsNTrickedTitle", tt.TipsNTrickTitle);
                        cmd.Parameters.AddWithValue("@tipsNTrickedLongDescription", tt.TipsNTrickLongDescription);
                        cmd.Parameters.AddWithValue("@createdOn", tt.CreatedOn);
                        cmd.Parameters.AddWithValue("@modifiedOn", tt.ModifiedOn);
                        cmd.Parameters.AddWithValue("@publishOn", tt.PublishOn);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        ViewBag.Msg = "Tips & Trick Updated susseccful";
                        ViewBag.Status = "alert-success";
                    }

                    catch (Exception ex)
                    {
                        ViewBag.Msg = "Oops! something went wrong";
                        ViewBag.Status = "alert-danger";
                    }
                }
                ViewBag.IsEdit = true;
                ViewBag.Id = tt.TipsNTrickId;

               
            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("InsertUpdateTipsNTrick", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tipsNTrickId", tt.TipsNTrickId);
                        cmd.Parameters.AddWithValue("@tipsNTrickedTitle", tt.TipsNTrickTitle);
                        cmd.Parameters.AddWithValue("@tipsNTrickedLongDescription", tt.TipsNTrickLongDescription);
                        cmd.Parameters.AddWithValue("@createdOn", tt.CreatedOn);
                        cmd.Parameters.AddWithValue("@modifiedOn", tt.ModifiedOn);
                        cmd.Parameters.AddWithValue("@publishOn", tt.PublishOn);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    ViewBag.Msg = "Tips and Tick Added susseccful";
                    ViewBag.Status = "alert-success";
                }
                catch (Exception ex)
                {
                    ViewBag.Msg = "Oops! something went wrong";
                    ViewBag.Status = "alert-danger";
                }
                ViewBag.IsEdit = false;
            }
            return View();
        }
    }
}