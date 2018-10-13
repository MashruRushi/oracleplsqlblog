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

namespace KishanBlog.Areas.Admin.Controllers
{
    public class PublishBlogController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: Admin/PublishBlog
        public ActionResult CreateBlog(int? Id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }

            Blog bg = new Blog();
            string path = string.Empty;

            if (Id == null)
            {
                byte[] imgbytedata = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/default.png"));
                string imgbase64data = Convert.ToBase64String(imgbytedata);
                string imgurl = string.Format("data:image/png/jpg;base64,{0}", imgbase64data);
                ViewBag.ImageData = imgurl;
                bg.PublishOn = DateTime.Now.ToString("MMM dd yyyy hh:mm tt");

                ViewBag.IsEdit = false;

            }
            else
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetBlogById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BlogId", Id);
                    cmd.ExecuteNonQuery();

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        bg.BlogPic = dr["BlogPic"].ToString();
                        byte[] imgbytedata = System.IO.File.ReadAllBytes(Server.MapPath("~/" + bg.BlogPic));
                        string imgbase64data = Convert.ToBase64String(imgbytedata);
                        string imgurl = string.Format("data:image/png/jpg;base64,{0}", imgbase64data);
                        ViewBag.ImageData = imgurl;

                        bg.BlogId = Convert.ToInt32(dr["BlogId"]);
                        bg.BlogTitle = dr["BlogTitle"].ToString();
                        bg.BlogShortDescription = dr["BlogShortDescription"].ToString();
                        bg.BlogLongdescription = dr["BlogLongDescription"].ToString();
                        bg.PublishOn = dr["PublishOn"].ToString();
                        if (!DBNull.Value.Equals(dr["YtVideoId"]))
                            bg.YtVideoId = dr["YtVideoId"].ToString();
                        if (!DBNull.Value.Equals(dr["VideoUrl"]))
                            bg.YtVideoUrl = dr["VideoUrl"].ToString();
                        if (!DBNull.Value.Equals(dr["VideoId"]))
                            bg.VideoId = Convert.ToInt32(dr["VideoId"]);
                        if (!DBNull.Value.Equals(dr["Tags"]))
                            bg.Tags = Convert.ToString(dr["Tags"]);
                    }

                    ViewBag.IsEdit = true;
                    ViewBag.Id = Id;

                    con.Close();
                }
            }
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd2 = new SqlCommand("VideoList", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd2);
                da.Fill(dt);
                List<Blog> listvideo = new List<Blog>();
                listvideo = (from DataRow dr in dt.Rows
                             select new Blog()
                             {
                                 YtVideoTitle = dr["VideoTitle"].ToString(),
                                 YtVideoId = dr["YtVideoId"].ToString()
                             }).ToList();
                bg.ListVideo = listvideo;
                con.Close();
            }
            return View(bg);
        }

        [HttpPost]
        public ActionResult CreateBlog(HttpPostedFileBase BlogPicUpload, Blog bg, bool? IsEdit, int? Id)
        {
            string path = string.Empty;

            if (Convert.ToBoolean(IsEdit))
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    if (BlogPicUpload != null)
                    {
                        BlogPicUpload.SaveAs(Request.PhysicalApplicationPath + "/Images/" + BlogPicUpload.FileName.ToString());
                        path = "Images/" + BlogPicUpload.FileName.ToString();

                        bg.BlogPic = path;
                    }
                    else
                    {
                        SqlCommand cmd1 = new SqlCommand("select BlogPic from [Blog] where BlogId='" + Id + "' ", con);
                        cmd1.ExecuteNonQuery();

                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd1);
                        da.Fill(dt);

                        foreach (DataRow dr in dt.Rows)
                        {
                            bg.BlogPic = dr["Blogpic"].ToString();
                        }
                    }

                    int id = 0;
                    bool IsVideoIdPresent = false;

                    if (bg.YtVideoId != null || bg.YtVideoUrl != null)
                    {

                        SqlCommand cmd2 = new SqlCommand("GetVideosById", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@YTVideoId", bg.YtVideoId);
                        cmd2.ExecuteNonQuery();

                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd2);
                        da.Fill(dt);


                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["YTVideoId"].ToString() == bg.YtVideoId)
                            {
                                IsVideoIdPresent = true;
                                bg.VideoId = Convert.ToInt16(dr["VideoId"]);
                            }
                        }

                        if (!IsVideoIdPresent)
                        {
                            bg.VideoId = 0;
                            string Date = string.Empty;
                            SqlCommand cmd1 = new SqlCommand("IsertUpdateVideo", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@VideoId", bg.VideoId);
                            cmd1.Parameters.AddWithValue("@ytVideoId", bg.YtVideoId);
                            cmd1.Parameters.AddWithValue("@ytVideoUrl", bg.YtVideoUrl);
                            cmd1.Parameters.AddWithValue("@ytVideoTitle", bg.YtVideoTitle);
                            cmd1.Parameters.AddWithValue("@ytVideoDescription", bg.VideoDescription);
                            cmd1.Parameters.AddWithValue("@ytVideoThumbnailUrl", bg.ThumbnailUrl);
                            cmd1.Parameters.AddWithValue("@ytDate", Date);
                            cmd1.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                            cmd1.Parameters.Add("@date", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                            cmd1.ExecuteNonQuery();
                            id = (int)cmd1.Parameters["@ID"].Value;
                            bg.VideoId = id;
                        }
                    }

                    bg.BlogId = Convert.ToInt32(Id);
                    SqlCommand cmd = new SqlCommand("InsertUpdateBlog", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BlogId", bg.BlogId);
                    cmd.Parameters.AddWithValue("@BlogTitel", bg.BlogTitle);
                    cmd.Parameters.AddWithValue("@BlogShortDescription", bg.BlogShortDescription);
                    cmd.Parameters.AddWithValue("@BlogLongDescription", bg.BlogLongdescription);
                    cmd.Parameters.AddWithValue("@VideoId", bg.VideoId);
                    cmd.Parameters.AddWithValue("@BlogPic", bg.BlogPic);
                    cmd.Parameters.AddWithValue("@Views", bg.Views);
                    cmd.Parameters.AddWithValue("@PublishOn", bg.PublishOn);
                    cmd.Parameters.AddWithValue("@Tags", bg.Tags);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                byte[] imgbytedata = System.IO.File.ReadAllBytes(Server.MapPath("~/" + bg.BlogPic));
                string imgbase64data = Convert.ToBase64String(imgbytedata);
                string imgurl = string.Format("data:image/png/jpg;base64,{0}", imgbase64data);
                ViewBag.ImageData = imgurl;

                ViewBag.IsEdit = true;
                ViewBag.Id = bg.BlogId;

                ViewBag.Msg = "Article Updated susseccful";                
                ViewBag.Status = "alert-success";

            }

            else
            {
                if (BlogPicUpload != null)
                {
                    BlogPicUpload.SaveAs(Request.PhysicalApplicationPath + "/Images/" + BlogPicUpload.FileName.ToString());
                    path = "Images/" + BlogPicUpload.FileName.ToString();
                }
                else
                {
                    path = "Images/default.png";
                }

                bg.BlogPic = path;
                byte[] imgbytedata = System.IO.File.ReadAllBytes(Server.MapPath("~/" + bg.BlogPic));
                string imgbase64data = Convert.ToBase64String(imgbytedata);
                string imgurl = string.Format("data:image/png/jpg;base64,{0}", imgbase64data);
                ViewBag.ImageData = imgurl;

                using (SqlConnection con = new SqlConnection(cs))
                {
                    int id = 0;
                    bool IsVideoIdPresent = false;
                    con.Open();

                    if (bg.YtVideoId != null || bg.YtVideoUrl != null)
                    {
                        SqlCommand cmd2 = new SqlCommand("GetVideosById", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@YTVideoId", bg.YtVideoId);
                        cmd2.ExecuteNonQuery();

                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd2);
                        da.Fill(dt);


                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["YTVideoId"].ToString() == bg.YtVideoId)
                            {
                                IsVideoIdPresent = true;
                                bg.VideoId = Convert.ToInt16(dr["VideoId"]);
                            }
                        }

                        if (!IsVideoIdPresent)
                        {
                            bg.VideoId = 0;
                            SqlCommand cmd1 = new SqlCommand("IsertUpdateVideo", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@VideoId", bg.VideoId);
                            cmd1.Parameters.AddWithValue("@ytVideoId", bg.YtVideoId);
                            cmd1.Parameters.AddWithValue("@ytVideoUrl", bg.YtVideoUrl);
                            cmd1.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                            cmd1.ExecuteNonQuery();
                            id = (int)cmd1.Parameters["@ID"].Value;
                            bg.VideoId = id;
                        }
                    }

                    SqlCommand cmd = new SqlCommand("InsertUpdateBlog", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BlogId", bg.BlogId);
                    cmd.Parameters.AddWithValue("@BlogTitel", bg.BlogTitle);
                    cmd.Parameters.AddWithValue("@BlogShortDescription", bg.BlogShortDescription);
                    cmd.Parameters.AddWithValue("@BlogLongDescription", bg.BlogLongdescription);
                    cmd.Parameters.AddWithValue("@VideoId", bg.VideoId);
                    cmd.Parameters.AddWithValue("@BlogPic", bg.BlogPic);
                    cmd.Parameters.AddWithValue("@Views", bg.Views);
                    cmd.Parameters.AddWithValue("@PublishOn", bg.PublishOn);
                    cmd.Parameters.AddWithValue("@Tags", bg.Tags);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    ViewBag.IsEdit = false;

                    ViewBag.Msg = "Article added susseccful";
                    ViewBag.DisableBtn = "disabled";
                    ViewBag.Status = "alert-success";

                }
            }
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd2 = new SqlCommand("VideoList", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd2);
                da.Fill(dt);
                List<Blog> listvideo = new List<Blog>();
                listvideo = (from DataRow dr in dt.Rows
                             select new Blog()
                             {
                                 YtVideoTitle = dr["VideoTitle"].ToString(),
                                 YtVideoId = dr["YtVideoId"].ToString()
                             }).ToList();
                bg.ListVideo = listvideo;
                con.Close();
            }


            return View(bg);
        }

        [HttpPost]
        public JsonResult Upload()
        {
            bool status = false;
            var filename = string.Empty;

            if (Session["Email"] != null)
            {
                var fileName = string.Empty;
                if (Request.Files.Count >= 1)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];

                        fileName = Path.GetFileName(file.FileName);

                        var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        file.SaveAs(path);
                    }

                    filename = fileName;
                    status = true;
                    return Json(new { filename, status, JsonRequestBehavior = true });
                }
                else
                {
                    status = false;
                    return Json(new { filename, status, JsonRequestBehavior = true });
                }
            }

            return Json(new { filename, status, JsonRequestBehavior = true });
        }

        public JsonResult Delete(string filename)
        {
            bool status = false;
            if (Session["Email"] != null)
            {

                if (!String.IsNullOrWhiteSpace(filename))
                {
                    var path = Path.Combine(Server.MapPath("~/Images/"), filename);
                    FileInfo File = new FileInfo(path);
                    if (File.Exists)
                    {
                        File.Delete();
                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
                }
            }
            return Json(new { status });
        }
    }
}