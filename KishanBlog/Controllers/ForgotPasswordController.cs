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
    public class ForgotPasswordController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: ForgotPassword
        [HttpGet]
        public ActionResult SendPasswordLink()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendPasswordLink(Login lg)
        {
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
                    string c = dt.Rows.Count.ToString();
                    int x = Convert.ToInt32(c);
                    if (x <= 0)
                    {
                        ViewBag.Msg = "This email is not registered";
                        ViewBag.status = "alert-danger";
                        con.Close();
                        return View();
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lg.FirstName = dr["FirstName"].ToString();
                            lg.LastName = dr["LastName"].ToString();
                            lg.Email = dr["Email"].ToString();
                        }

                        var sublink = lg.Email + "|" + DateTime.Now;
                        var host = HttpContext.Request.Url.Authority;

                        var url = "http://" + host + "/SetNewPassword/SetNewPassword?Q=" + HttpUtility.UrlEncode(sublink);


                        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("rushi.p.mashru@gmail.com", "rushi666");
                        //(from,to,subject,body);               
                        string bod = this.PopulateBody(url, lg);  /*"Welcome " + textconct + ". Your password for Admission Assistance is " + randomnumber.ToString() + ". This mail is send to you for authentication purpose. Log-in by using this password. We recomment you to change this password after log-in.";*/
                        MailMessage mail = new MailMessage("rushi.p.mashru@gmail.com", lg.Email, "Reset Password Link for KishanMashru's Blog", bod.ToString());
                        mail.IsBodyHtml = true;
                        smtp.Send(mail);

                        con.Close();

                        ViewBag.Msg = "Link has been send to your registered email id";
                        ViewBag.status = "alert-info";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Oops! somthing went wrong";
                ViewBag.status = "alert-danger";
            }
            return View();
        }

        private string PopulateBody(string url, Login lg)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/ResetPasswordLink.html")))
            {
                body = reader.ReadToEnd();
            }
            string textconct = lg.FirstName + " " + lg.LastName;
            body = body.Replace("{textconct}", textconct);
            body = body.Replace("{url}", url);
            return body;
        }
    }
}