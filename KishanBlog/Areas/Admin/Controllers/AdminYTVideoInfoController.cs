using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using KishanBlog.Models;
using System.IO;

namespace KishanBlog.Areas.Admin.Controllers
{
    public class AdminYTVideoInfoController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: Admin/AdminYTVideoInfo
        public ActionResult YTVideoInfoIndex()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }

            if (!Request.IsAjaxRequest())
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd1 = new SqlCommand("AdminGetVideo", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.ExecuteNonQuery();

                    DataTable dt1 = new DataTable();
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    da1.Fill(dt1);

                    ViewBag.VideoList = dt1;

                    SqlCommand cmd = new SqlCommand("select Top 1 PublishedAt from [Video] Order by VideoId", con);
                    cmd.ExecuteNonQuery();

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow dr in dt.Rows)
                    {
                        ViewBag.date = dr["PublishedAt"];
                    }

                    con.Close();
                }
                return View();
            }

            else
                return Json("");
        }

        public ActionResult SaveVideoInfo(string VideoId, string VideoTitle, string VideoDescription, string ThumbnailUrl, string Date)
        {
            bool status = false;
            int id = 0;
            string date = string.Empty;
            YouTubeVideo yt = new YouTubeVideo();
            yt.YTVideoId = VideoId;
            yt.VideoTitle = VideoTitle;
            yt.VideoDescription = VideoDescription;
            yt.ThumbnailUrl = ThumbnailUrl;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("IsertUpdateVideo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VideoId", yt.VideoId);
                cmd.Parameters.AddWithValue("@ytVideoId", yt.YTVideoId);
                cmd.Parameters.AddWithValue("@ytVideoUrl", yt.VideoIdUrl);
                cmd.Parameters.AddWithValue("@ytVideoTitle", yt.VideoTitle);
                cmd.Parameters.AddWithValue("@ytVideoDescription", yt.VideoDescription);
                cmd.Parameters.AddWithValue("@ytVideoThumbnailUrl", yt.ThumbnailUrl);
                cmd.Parameters.AddWithValue("@ytDate", Date);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@date", SqlDbType.VarChar,200).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                id = (int)cmd.Parameters["@ID"].Value;
                date = (string)cmd.Parameters["@date"].Value;
                yt.VideoId = id;

                con.Close();
            }

            if (id > 0)
                status = true;

            return Json(new { status, yt.YTVideoId, date });
        }
    }
}