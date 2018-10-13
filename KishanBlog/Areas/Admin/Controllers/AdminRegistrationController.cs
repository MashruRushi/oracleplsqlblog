using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using KishanBlog.Areas.Admin.Models;
using System.IO;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace KishanBlog.Areas.Admin.Controllers
{
    public class AdminRegistrationController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: Admin/AdminRegistration
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        private string PopulateBody(string randomnumber, string name)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/AdminPassword.html")))
            {
                body = reader.ReadToEnd();
            }
            string textconct = name;
            body = body.Replace("{textconct}", textconct);
            return body;
        }
        [HttpPost]
        public ActionResult Register(AdminLogin reg)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("select * from Admin where Email='" + reg.Email + "'", con);
                    cmd.ExecuteNonQuery();

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    string c = dt.Rows.Count.ToString();
                    int x = Convert.ToInt32(c);
                    if (x > 0)
                    {
                        ViewBag.loginError = "This email is already registered";
                        return View();
                    }
                    else
                    {
                        Random ra = new Random();
                        string randomnumber = (ra.Next(10000, 99999)).ToString();
                        reg.AdminPassword = randomnumber;
                        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("rushi.p.mashru@gmail.com", "rushi666");
                        //(from,to,subject,body);  

                        string bod = this.PopulateBody(randomnumber, reg.AdminName);
                        MailMessage mail = new MailMessage("rushi.p.mashru@gmail.com", reg.Email, "Do Not Reply:Kishan Mashru's Bolg", bod.ToString());
                        mail.IsBodyHtml = true;
                        smtp.Send(mail);

                        SqlCommand cmd1 = new SqlCommand("InsertUpdateAdmin", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@AdminName", reg.AdminName);
                        cmd1.Parameters.AddWithValue("@AdminPassword", reg.AdminPassword);
                        cmd1.Parameters.AddWithValue("@Email", reg.Email);
                        cmd1.Parameters.AddWithValue("@SuperAdmin", reg.SuperAdmin);
                        cmd1.Parameters.AddWithValue("@IsActive", reg.IsActive);
                        cmd1.ExecuteNonQuery();

                        con.Close();
                        ViewBag.loginMsg = "Mail Send";
                        return View();
                    }
                }
            }
            catch(Exception ex)
            {
                ViewBag.loginError = "Oops! something went wrong";
                return View();
            }           
        }
    }
}