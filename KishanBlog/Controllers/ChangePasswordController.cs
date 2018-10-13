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
    public class ChangePasswordController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        // GET: ChangePassword
        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login","Login");
            }

            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(Login lg, string OldPassword)
        {
            if (Session["Email"] == null)
                return RedirectToAction("Login","Login");

            lg.Email = Session["Email"].ToString();
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    var UserType = 0;
                    var DbOldPassword = string.Empty;
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
                        DbOldPassword = dr["Password"].ToString();
                        lg.PhotoPath = dr["PhotoPath"] != null ? dr["PhotoPath"].ToString() : null;
                        UserType = Convert.ToInt16(dr["UserType"]);
                        lg.Country = dr["Country"] != null ? dr["Country"].ToString() : null;
                        lg.State = dr["State"] != null ? dr["State"].ToString() : null;
                        lg.City = dr["City"] != null ? dr["City"].ToString() : null;
                    }

                    if (DbOldPassword != OldPassword)
                    {
                        ViewBag.Msg = "Old Password is incorrect";
                        ViewBag.status = "alert-danger";
                        return View();
                    }
                    else
                    {
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

                        ViewBag.Msg = "Password Changed Successfully";
                        ViewBag.status = "alert-success";
                    }
                    con.Close();
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