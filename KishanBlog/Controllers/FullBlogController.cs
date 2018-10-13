using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KishanBlog.Models;

namespace KishanBlog.Controllers
{
    public class FullBlogController : Controller
    {
        // GET: FullBlog

        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        [HttpGet]
        public ActionResult FullBlog(Blog bg, int Id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetBlogById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Blogid", Id);
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ViewBag.Data = dt;

                    bg.BlogTitle = Convert.ToString(dt.Rows[0][1]);
                    if (!DBNull.Value.Equals(dt.Rows[0][11]))
                        bg.Tags = Convert.ToString(dt.Rows[0][11]);

                    SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) AS Count FROM [Like] WHERE (LikeType=0 AND ReferanceId='" + Id + "')", con);
                    cmd1.ExecuteNonQuery();
                    DataTable dt1 = new DataTable();
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    da1.Fill(dt1);

                    foreach (DataRow dr1 in dt1.Rows)
                    {
                        ViewBag.Count = dr1["Count"];
                    }

                    if (Session["Email"] != null)
                    {
                        SqlCommand cmd2 = new SqlCommand("SELECT LikeId FROM [Like] WHERE (UserEmail='" + Session["Email"].ToString() + "' AND LikeType=0 AND ReferanceId='" + Id + "')", con);
                        cmd2.ExecuteNonQuery();
                        DataTable dt2 = new DataTable();
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        da2.Fill(dt2);

                        int likeId = dt2.Rows.Count;

                        if (likeId > 0)
                        {
                            ViewBag.likedClass = "liked";
                            foreach (DataRow dr2 in dt2.Rows)
                            {
                                ViewBag.LikeId = dr2["LikeId"];
                            }
                        }
                    }
                    con.Close();
                    return View(bg);
                }
                else
                {
                    return Redirect("~/PageNotFound.html");
                }
            }

        }


        public JsonResult Like(Blog bg, int likeType, string email)
        {
            bool status = false;
            int count = 0;
            int ID = 0;

            if (!String.IsNullOrEmpty(email))
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("InsertUpadateLike", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LikeId", bg.LikeId);
                    cmd.Parameters.AddWithValue("@RefId", bg.BlogId);
                    cmd.Parameters.AddWithValue("@LikeType", likeType);
                    cmd.Parameters.AddWithValue("@UserEmail", email);
                    cmd.Parameters.Add("@LikeCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    count = (int)cmd.Parameters["@LikeCount"].Value;

                    con.Close();
                    if (bg.LikeId == 0)
                    {
                        status = true;
                        ID = (int)cmd.Parameters["@ID"].Value;
                    }
                    else
                    {
                        status = false;
                    }
                }
            }
            return Json(new { status, count, ID });
        }

        public JsonResult Comments(string comment, int? blogId, string userId)
        {
            bool status = false;
            var userFirstName = string.Empty;
            var userLastName = string.Empty;
            var picPath = string.Empty;
            string time = string.Empty;
            int CommentId = 0;
            if (!string.IsNullOrWhiteSpace(userId) && blogId != null && !string.IsNullOrWhiteSpace(userId))
            {
                Blog bg = new Blog();

                using (SqlConnection con1 = new SqlConnection(cs))
                {
                    con1.Open();
                    SqlCommand cmd1 = new SqlCommand("GetUser", con1);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@email", userId);
                    cmd1.ExecuteNonQuery();

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        bg.UserId = Convert.ToInt16(dr["UserId"]);
                        userFirstName = dr["FirstName"].ToString();
                        userLastName = dr["LastName"].ToString();
                        picPath = string.IsNullOrEmpty(dr["PhotoPath"].ToString()) ? "UserImages/default_avatar.jpg" : dr["PhotoPath"].ToString();
                    }
                }
                bg.Comments = comment;
                bg.BlogId = (int)blogId;

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("InsertUpdateComments", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CommentId", bg.CommentId);
                    cmd.Parameters.AddWithValue("@Comment", bg.Comments);
                    cmd.Parameters.AddWithValue("@BlogId", bg.BlogId);
                    cmd.Parameters.AddWithValue("@UserId", bg.UserId);
                    cmd.Parameters.AddWithValue("@CommentType", "Comment");
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    CommentId = (int)cmd.Parameters["@ID"].Value;

                    con.Close();
                    if (CommentId > 0)
                    {
                        status = true;
                        time = DateTime.Now.ToString("MMM dd, yyyy hh:mm tt");
                    }

                }
            }
            return Json(new { status, comment, userFirstName, userLastName, picPath, time, CommentId });
        }

        public JsonResult Reply(string reply, int? blogId, string userId, int? CommentId)
        {
            bool status = false;
            var userFirstName = string.Empty;
            var userLastName = string.Empty;
            var picPath = string.Empty;
            string time = string.Empty;
            int ReplyId = 0;

            if (!string.IsNullOrWhiteSpace(userId) && blogId != null && !string.IsNullOrWhiteSpace(userId) && CommentId != null)
            {
                Blog bg = new Blog();

                using (SqlConnection con1 = new SqlConnection(cs))
                {
                    con1.Open();
                    SqlCommand cmd1 = new SqlCommand("GetUser", con1);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@email", userId);
                    cmd1.ExecuteNonQuery();

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        bg.UserId = Convert.ToInt16(dr["UserId"]);
                        userFirstName = dr["FirstName"].ToString();
                        userLastName = dr["LastName"].ToString();
                        picPath = string.IsNullOrEmpty(dr["PhotoPath"].ToString()) ? "UserImages/default_avatar.jpg" : dr["PhotoPath"].ToString();
                    }
                }
                bg.Reply = reply;
                bg.BlogId = (int)blogId;
                bg.CommentId = (int)CommentId;

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("InsertUpdateReplies", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReplyId", bg.ReplyId);
                    cmd.Parameters.AddWithValue("@Reply", bg.Reply);
                    cmd.Parameters.AddWithValue("@CommentId", bg.CommentId);
                    cmd.Parameters.AddWithValue("@BlogId", bg.BlogId);
                    cmd.Parameters.AddWithValue("@UserId", bg.UserId);
                    cmd.Parameters.AddWithValue("@ReplyType", "Reply");
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    ReplyId = (int)cmd.Parameters["@ID"].Value;
                    con.Close();
                    if (ReplyId > 0)
                    {
                        status = true;
                        time = DateTime.Now.ToString("MMM dd, yyyy hh:mm tt");
                    }

                }


            }
            return Json(new { status, reply, userFirstName, userLastName, picPath, time, CommentId });
        }

        public JsonResult LoadComments(int? bolgId, int? skipId)
        {
            bool status = false;
            int count = 0;
            var userFirstName = string.Empty;
            var userLastName = string.Empty;
            var picPath = string.Empty;
            var time = string.Empty;
            var comment = string.Empty;
            int ID = 0;

            List<Comments> commentList = new List<Comments>();

            if (bolgId != null)
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetAllComments", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BlogId", bolgId);
                    cmd.Parameters.AddWithValue("@SkipId", skipId);
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    ID = cmd.ExecuteNonQuery();

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    DataTable dt = ds.Tables[0];
                    DataTable dt1 = ds.Tables[1];

                    foreach (DataRow dr1 in dt1.Rows)
                    {
                        ID = Convert.ToInt16(dr1["CommentId"]);
                    }


                    commentList = (from DataRow dr in dt.Rows
                                   select new Comments()
                                   {
                                       CommentId = Convert.ToInt16(dr["CommentId"]),
                                       UserFirstName = dr["FirstName"].ToString(),
                                       UserLastName = dr["LastName"].ToString(),
                                       PicPath = string.IsNullOrEmpty(dr["PhotoPath"].ToString()) ? "UserImages/default_avatar.jpg" : dr["PhotoPath"].ToString(),
                                       Comment = dr["Comment"].ToString(),
                                       CommentOn = dr["CommentedOn"].ToString()
                                   }).ToList();
                    commentList.Reverse();

                    count = dt.Rows.Count;

                    if (count > 0)
                    {
                        status = true;
                    }
                    con.Close();
                }
            }
            return Json(new { status, count, commentList, ID });
        }

        public JsonResult LoadReplies(int? bolgId, int? skipId)
        {
            bool status = false;
            int count = 0;
            var userFirstName = string.Empty;
            var userLastName = string.Empty;
            var picPath = string.Empty;
            var time = string.Empty;
            var reply = string.Empty;
            int ID = 0;

            List<Replies> ReplyList = new List<Replies>();

            if (bolgId != null)
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetAllReplies", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@blogId", bolgId);
                    cmd.Parameters.AddWithValue("@SkipId", skipId);
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();


                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    //DataTable dt = new DataTable();

                    DataTable dt = ds.Tables[0];
                    DataTable dt1 = ds.Tables[1];

                    foreach (DataRow dr1 in dt1.Rows)
                    {
                        ID = Convert.ToInt16(dr1["ReplyId"]);
                    }

                    ReplyList = (from DataRow dr in dt.Rows
                                 select new Replies()
                                 {
                                     ReplyId = Convert.ToInt16(dr["ReplyId"]),
                                     CommentId = Convert.ToInt16(dr["CommentId"]),
                                     UserFirstName = dr["FirstName"].ToString(),
                                     UserLastName = dr["LastName"].ToString(),
                                     PicPath = string.IsNullOrEmpty(dr["PhotoPath"].ToString()) ? "UserImages/default_avatar.jpg" : dr["PhotoPath"].ToString(),
                                     Reply = dr["Reply"].ToString(),
                                     ReplyOn = dr["RepliedOn"].ToString()
                                 }).ToList();
                    ReplyList.Reverse();

                    count = dt.Rows.Count;

                    if (count > 0)
                    {
                        status = true;
                    }
                    con.Close();
                }
            }
            return Json(new { status, count, ReplyList, ID });
        }
    }
}