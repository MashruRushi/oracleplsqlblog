using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using KishanBlog.Areas.Admin.Models;
using System.Configuration;

namespace KishanBlog.Areas.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: Admin/AdminLogin
        public ActionResult Login()
        {
            HttpCookie cookie = Request.Cookies["AdminInfo"];
            if (cookie != null)
            {
                ViewBag.email = cookie["AdminEmail"];
                ViewBag.password = cookie["AdminPassword"];
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(AdminLogin login, string remember)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    bool Sadmin = false, IsActive = false;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select * from Admin where Email=@adminEmail AND AdminPassword=@adminPassword", con);
                    cmd.Parameters.AddWithValue("adminEmail", login.Email);
                    cmd.Parameters.AddWithValue("adminPassword", login.AdminPassword);
                    cmd.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt); 

                    foreach (DataRow dr in dt.Rows)
                    {
                        Sadmin = Convert.ToBoolean(dr["SuperAdmin"]);
                        IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }

                    Session["SuperAdmin"] = Sadmin;

                    string c = dt.Rows.Count.ToString();
                    int x = Convert.ToInt32(c);

                    if (remember != null)
                    {
                        HttpCookie cookie = new HttpCookie("AdminInfo");
                        cookie["AdminEmail"] = login.Email;
                        cookie["AdminPassword"] = login.AdminPassword;

                        cookie.Expires.AddDays(30);
                        Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        HttpCookie cookie = new HttpCookie("AdminInfo");
                        cookie.Expires.AddDays(-1);
                        Response.Cookies.Add(cookie);
                    }

                    if (x > 0)
                    {
                        if (Sadmin == true || IsActive == true)
                        {
                            con.Close();
                            //Response.Write("Welcome!");
                            Session["Email"] = login.Email;
                            return RedirectToAction("CreateBlog", "PublishBlog");
                        }
                        else
                        {
                            ViewBag.loginError = "Invalid name or password";
                            con.Close();
                            return View();
                        }

                    }
                    else
                    {
                        ViewBag.loginError = "Invalid name or password";
                        con.Close();
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