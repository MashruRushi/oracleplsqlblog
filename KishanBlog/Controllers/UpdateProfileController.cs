using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KishanBlog.Models;
using System.IO;

namespace KishanBlog.Controllers
{
    public class UpdateProfileController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: UpdateProfile
        [HttpGet]
        public ActionResult UpdateProfile(Login lg)
        {

            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            lg.Email = Session["Email"].ToString();
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
                        lg.Password = dr["Password"].ToString();
                        lg.PhotoPath = string.IsNullOrEmpty(dr["PhotoPath"].ToString()) ? "UserImages/default_avatar.jpg" : dr["PhotoPath"].ToString();
                        UserType = Convert.ToInt16(dr["UserType"]);
                        lg.Country = dr["Country"] != null ? dr["Country"].ToString() : null;
                        lg.State = dr["State"] != null ? dr["State"].ToString() : null;
                        lg.City = dr["City"] != null ? dr["City"].ToString() : null;
                    }

                    ViewBag.UserType = UserType;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Oops! somthing went wrong" + ex.Message;
                ViewBag.status = "alert-danger";
            }
            return View(lg);
        }

        [HttpPost]
        public ActionResult UpdateProfile(HttpPostedFileBase ProfilPicUploader, Login lg, string UserType)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (ProfilPicUploader != null)
            {
                if (ProfilPicUploader.ContentType.ToLower() != "image/jpg" && ProfilPicUploader.ContentType.ToLower() != "image/png" && ProfilPicUploader.ContentType.ToLower() != "image/jpeg")
                {
                    ViewBag.Msg = "Trying to act smart ! Better Luck Next Time";
                    ViewBag.status = "alert-danger";
                    return View(lg);
                }
            }
            lg.Email = Session["Email"].ToString();

            string path = string.Empty;

            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM [User] WHERE Email='" + lg.Email + "'", con);
                    cmd.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {

                        lg.RegisterType = Login.RegisterTypeEnum.NormalRegister;
                        lg.Password = dr["Password"].ToString();
                        lg.PhotoPath = dr["PhotoPath"] != null ? dr["PhotoPath"].ToString() : null;
                    }

                    if (ProfilPicUploader != null)
                    {
                        ProfilPicUploader.SaveAs(Request.PhysicalApplicationPath + "/UserImages/" + ProfilPicUploader.FileName.ToString());
                        path = "UserImages/" + ProfilPicUploader.FileName.ToString();

                        lg.PhotoPath = path;
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

                    ViewBag.Msg = "Profile Updated Successfully";
                    ViewBag.status = "alert-success";

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Oops! somthing went wrong";
                ViewBag.status = "alert-danger";
            }

            return View(lg);
        }
    }
}