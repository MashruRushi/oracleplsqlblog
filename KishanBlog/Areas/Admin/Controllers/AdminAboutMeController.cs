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
    public class AdminAboutMeController : Controller
    {
        // GET: Admin/AdminAboutMe
        
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        [HttpGet]
        public ActionResult AboutMeIndex(AboutMe ab,int? x)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }

            //var enumData = from AboutMe.EnumDescriptionType e in Enum.GetValues(typeof(AboutMe.EnumDescriptionType))
            //               select new
            //               {
            //                   ID = (int)e,
            //                   Name = e.ToString()
            //               };

            //ViewBag.EnumList= new SelectList(enumData, "ID", "Name");          
            using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select * from [AboutMe] where DescriptionType=0", con);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        ab.AboutMeId = Convert.ToInt32(dr["AboutMeId"]);
                        int e = Convert.ToInt32(dr["DescriptionType"]);
                        ab.DescriptionType = (AboutMe.EnumDescriptionType)Enum.ToObject(typeof(AboutMe.EnumDescriptionType),e);
                        ab.AboutMeDescription = dr["AboutMeDescription"].ToString();
                    }
                }            

            return View(ab);
        }
        [HttpPost]        
        public ActionResult AboutMeIndex(AboutMe ab)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("InsertUpdateAboutMe", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@aboutMeId", ab.AboutMeId);
                    cmd.Parameters.AddWithValue("@descriptionType", (int)ab.DescriptionType);
                    cmd.Parameters.AddWithValue("@aboutMeDescription", ab.AboutMeDescription);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                ViewBag.Message = "Description added susseccfully";
                ViewBag.Status = "alert-success";
            }

            catch (Exception ex)
            {
                ViewBag.Message = "Oops! something went wrong";
                ViewBag.Status = "alert-danger";
            }
            return View();
        }

        public JsonResult AboutMeFillData(int? Type,AboutMe ab)
       {
            bool Status = false;
            using (SqlConnection con = new SqlConnection(cs))
            {                
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from [AboutMe] where DescriptionType='" + Type + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    Status = true;

                foreach (DataRow dr in dt.Rows)
                {
                    ab.AboutMeId = Convert.ToInt32(dr["AboutMeId"]);
                    int e = Convert.ToInt32(dr["DescriptionType"]);
                    ab.DescriptionType = (AboutMe.EnumDescriptionType)Enum.ToObject(typeof(AboutMe.EnumDescriptionType), e);
                    ab.AboutMeDescription = dr["AboutMeDescription"].ToString();
                }
            }

            return Json(new {Status,ab.AboutMeId,ab.DescriptionType,ab.AboutMeDescription });
        }
    }
}