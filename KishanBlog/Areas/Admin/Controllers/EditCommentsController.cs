using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace KishanBlog.Areas.Admin.Controllers
{
    public class EditCommentsController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: Admin/EditComments
        public ActionResult EditCommentsIndex()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("AdminGetAllComments", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ViewBag.CommentsList = dt;
                con.Close();
            }

            return View();
        }

        //public ActionResult DeleteComment(int Id, bool IsDelete=false)
        //{
        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("DeleteComments", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@commentId", Id);
        //        cmd.Parameters.AddWithValue("@isDelete", IsDelete);
        //        cmd.ExecuteNonQuery();

        //        SqlCommand cmd1 = new SqlCommand("AdminGetAllComments", con);
        //        cmd1.CommandType = CommandType.StoredProcedure;
        //        cmd1.ExecuteNonQuery();

        //        DataTable dt1 = new DataTable();
        //        SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
        //        da1.Fill(dt1);

        //        ViewBag.CommentsList = dt1;
        //        con.Close();
        //    }
        //    return View("EditCommentsIndex");
        //}
        public ActionResult DeleteComment(int Id, string status, bool IsDelete = false)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DeleteComments", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@isDelete", IsDelete);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.ExecuteNonQuery();

                SqlCommand cmd1 = new SqlCommand("AdminGetAllComments", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.ExecuteNonQuery();

                DataTable dt1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(dt1);

                ViewBag.CommentsList = dt1;
                con.Close();
            }
            return View("EditCommentsIndex");
        }
    }
}