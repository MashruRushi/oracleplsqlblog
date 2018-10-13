using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KishanBlog.Models;
using CaptchaMvc.HtmlHelpers;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace KishanBlog.Controllers
{
    public class LoginController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: Login        
        [HttpGet]
        public ActionResult Login(int? ID)
        {
            if (ID != null)
            {
                ViewBag.BlogID = ID;
            }
            if (Session["Email"] != null)
            {
                Session["Email"] = null;
                return RedirectToAction("Index", "MyHome");
            }
            else
            {
                HttpCookie cookie = Request.Cookies["userInfo"];
                if (cookie != null)
                {
                    ViewBag.name = cookie["Username"];
                    ViewBag.password = cookie["password"];
                }
            }
            return View();
        }

        private string PopulateBody(string randomnumber, Login reg)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/MailPassword.html")))
            {
                body = reader.ReadToEnd();
            }
            string textconct = reg.FirstName + " " + reg.LastName;
            body = body.Replace("{textconct}", textconct);
            body = body.Replace("{randomnumber}", randomnumber);
            return body;
        }

        [HttpPost]
        public ActionResult SignUp(Login reg, string UserType,int? BlogId)
        {
            ViewBag.page = "signup";
            if (this.IsCaptchaValid("Captcha is valid"))
            {

                try
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("SELECT * FROM [User] WHERE Email='" + reg.Email + "'", con);
                        cmd.ExecuteNonQuery();
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        string c = dt.Rows.Count.ToString();
                        int x = Convert.ToInt32(c);
                        if (x > 0)
                        {
                            ViewBag.Msg = "This email is already registered";
                            ViewBag.status = "alert-danger";
                            con.Close();
                            return View("Login");
                        }
                        else
                        {
                            Random ra = new Random();
                            string randomnumber = (ra.Next(10000, 99999)).ToString();

                            reg.Password = randomnumber;

                            //sending email by smtp...

                            SmtpClient smtp = new SmtpClient("mail.oracleplsqlblog.com");
                            smtp.EnableSsl = true;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential("support@oracleplsqlblog.com", "BlackDog@66");
                            //(from,to,subject,body);               
                            string bod = this.PopulateBody(randomnumber, reg);  /*"Welcome " + textconct + ". Your password for Admission Assistance is " + randomnumber.ToString() + ". This mail is send to you for authentication purpose. Log-in by using this password. We recomment you to change this password after log-in.";*/
                            MailMessage mail = new MailMessage("support@oracleplsqlblog.com", reg.Email, "Password for Kishan Mashru's Oracle Blog", bod.ToString());
                            mail.IsBodyHtml = true;
                            smtp.Send(mail);

                            SqlCommand cmd1 = new SqlCommand("InsertUpdateUser", con);
                            cmd1.CommandType = CommandType.StoredProcedure;

                            cmd1.Parameters.AddWithValue("userId", reg.UserId);
                            cmd1.Parameters.AddWithValue("registerType", reg.RegisterType);
                            cmd1.Parameters.AddWithValue("firstName", reg.FirstName);
                            cmd1.Parameters.AddWithValue("lastName", reg.LastName);
                            cmd1.Parameters.AddWithValue("Email", reg.Email);
                            cmd1.Parameters.AddWithValue("password", reg.Password);
                            cmd1.Parameters.AddWithValue("picPath", reg.PhotoPath);
                            cmd1.Parameters.AddWithValue("userType", UserType);
                            cmd1.Parameters.AddWithValue("country", reg.Country);
                            cmd1.Parameters.AddWithValue("state", reg.State);
                            cmd1.Parameters.AddWithValue("city", reg.City);

                            int result = cmd1.ExecuteNonQuery();

                            if (result > 0)
                            {
                                if(BlogId!=null)
                                {
                                    ViewBag.BlogID = BlogId;
                                }
                                ViewBag.Msg = "Sign Up successful. Login using your Email id and Password";
                                ViewBag.status = "alert-success";
                            }

                            con.Close();
                            return View("Login");
                        }

                    }
                }

                catch (Exception e)
                {
                    ViewBag.Msg = "Oops! somthing went wrong";
                    ViewBag.status = "alert-danger";
                }
            }
            else
            {
                ViewBag.CaptchaMsg = "Incorrect answer";
                return View("Login");
            }
            return View("Login");
        }
        [HttpPost]
        public ActionResult Login(Login login,int? BlogId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * from [User] where Email='" + login.Email + "' AND Password='" + login.Password + "'", con);
                    cmd.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    string c = dt.Rows.Count.ToString();
                    int x = Convert.ToInt32(c);

                    if (login.Remember == true)
                    {
                        HttpCookie cookie = new HttpCookie("userInfo");
                        cookie["username"] = login.Email;
                        cookie["password"] = login.Password;

                        cookie.Expires = DateTime.Now.AddDays(30);
                        Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        HttpCookie cookie = new HttpCookie("userInfo");
                        cookie.Expires = DateTime.Now.AddDays(-1);
                    }


                    if (x > 0)
                    {
                        Session["Email"] = login.Email;
                        if (Session["Email"] != null)
                        {
                            var picPath = string.Empty;
                            foreach(DataRow dr in dt.Rows)
                            {
                                picPath = dr["PhotoPath"].ToString();
                            }
                            Session["UserImagePath"] = !string.IsNullOrEmpty(picPath) ? picPath : "UserImages/default_avatar.jpg";

                            ViewBag.Msg = "Login successful";
                            ViewBag.status = "alert-success";
                            if(BlogId!=null)
                            {
                                return RedirectToAction("FullBlog", "FullBlog", new { ID = BlogId });
                            }
                            else
                            {
                                
                                return RedirectToAction("Index", "MyHome");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                ViewBag.Msg = "Oops! somthing went wrong";
                ViewBag.status = "alert-danger";
            }
            return View();
        }
    }
}