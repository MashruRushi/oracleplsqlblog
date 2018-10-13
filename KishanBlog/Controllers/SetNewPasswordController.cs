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
    public class SetNewPasswordController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        // GET: SetNewPassword
        [HttpGet]
        public ActionResult SetNewPassword(Login lg)
        {
            var QString = Request.QueryString["Q"].ToString();
            var SubLinkData = QString.Split('|');

            lg.Email = SubLinkData[0];
            var Date = Convert.ToDateTime(SubLinkData[1]).AddHours(1);

            ViewBag.IsExpired = false;

            if (Date < DateTime.Now)
            {
                ViewBag.IsExpired = true;
                ViewBag.Msg = "This link is expired";
                ViewBag.status = "alert-danger";
            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("SELECT Email FROM [User] WHERE  Email='" + lg.Email + "'", con);
                        cmd.ExecuteNonQuery();

                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        if (dt.Rows.Count <= 0)
                        {
                            ViewBag.IsExpired = true;
                            ViewBag.Msg = "This link is invalid";
                            ViewBag.status = "alert-danger";
                        }
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.IsExpired = true;
                    ViewBag.Msg = "Oops! somthing went wrong";
                    ViewBag.status = "alert-danger";
                }
            }
            return View(lg);
        }

        [HttpPost]
        public ActionResult SetNewPassword(Login lg, bool IsExpired)
        {
            if (IsExpired)
            {
                ViewBag.Msg = "Acting Smart! Better Luck Next Time";
                ViewBag.status = "alert-danger";

                return View();
            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        var UserType = 0;
                        con.Open();

                        SqlCommand cmd = new SqlCommand("SELECT * FROM [User] WHERE Email='" + lg.Email + "'", con);
                        cmd.ExecuteNonQuery();
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        foreach (DataRow dr in dt.Rows)
                        {
                            lg.UserId = Convert.ToInt16(dr["UserId"]);
                            lg.RegisterType = Login.RegisterTypeEnum.NormalRegister;
                            lg.FirstName = dr["FirstName"].ToString();
                            lg.LastName = dr["LastName"].ToString();
                            lg.Email = dr["Email"].ToString();
                            lg.PhotoPath = dr["PhotoPath"] != null ? dr["PhotoPath"].ToString() : null;
                            UserType = Convert.ToInt16(dr["UserType"]);
                            lg.Country = dr["Country"] != null ? dr["Country"].ToString() : null;
                            lg.State = dr["State"] != null ? dr["State"].ToString() : null;
                            lg.City = dr["City"] != null ? dr["City"].ToString() : null;
                        }


                        SqlCommand cmd1 = new SqlCommand("InsertUpdateUser", con);
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.AddWithValue("userId", lg.UserId);
                        cmd1.Parameters.AddWithValue("registerType", lg.RegisterType);
                        cmd1.Parameters.AddWithValue("firstName", lg.FirstName);
                        cmd1.Parameters.AddWithValue("lastName", lg.LastName);
                        cmd1.Parameters.AddWithValue("Email", lg.Email);
                        cmd1.Parameters.AddWithValue("password", lg.Password);
                        cmd1.Parameters.AddWithValue("picPath", lg.PhotoPath);
                        cmd1.Parameters.AddWithValue("userType", UserType);
                        cmd1.Parameters.AddWithValue("country", lg.Country);
                        cmd1.Parameters.AddWithValue("state", lg.State);
                        cmd1.Parameters.AddWithValue("city", lg.City);

                        cmd1.ExecuteNonQuery();

                        ViewBag.Msg = "Password Reset Successfully";
                        ViewBag.status = "alert-success";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Msg = "Oops! somthing went wrong";
                    ViewBag.status = "alert-danger";
                }

                return View();
            }
        }
    }
}