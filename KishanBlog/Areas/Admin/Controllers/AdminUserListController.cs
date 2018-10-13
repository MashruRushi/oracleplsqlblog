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
    public class AdminUserListController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: Admin/AdminUserList
        [HttpGet]
        public ActionResult UserListIndex()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetAllUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ViewBag.UserList = dt;
            }

            return View();
        }
        [HttpPost]
        public JsonResult UserListIndex(int UserId, bool IsBlock)
        {
            string name = string.Empty;
            string classname = string.Empty;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                IsBlock = IsBlock ? false : true;
                SqlCommand cmd = new SqlCommand("UpdateUserStatus", con);
                cmd.Parameters.AddWithValue("UserId", UserId);
                cmd.Parameters.AddWithValue("Status", IsBlock);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                name = IsBlock ? "Unblock" : "Block";
                classname = IsBlock ? "btn-danger" : "btn-info";
            }
            return Json(new { name, classname });
        }
    }
}