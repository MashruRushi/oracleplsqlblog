using System;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace KishanBlog.Controllers
{
    [OutputCache(Duration = 600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Server)]
    public class YouTubeVideoGridController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: YouTubeVideoGrid
        public ActionResult YouTubeVideoGridIndex(string Type, int? PS)
        {
            int pagecounter = 1;
            int totalRecord = 0;
            int pagesize = 0;

            if (PS == null)
            {
                pagesize = 10;
            }
            else
            {
                pagesize = Convert.ToInt32(PS);
            }


            if (Type == "load")
            {
                pagesize = pagesize + 10;
            }

            if (Type == "back")
            {
                pagesize = pagesize - 10;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("VideoListForGrid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pagesize", pagesize);
                cmd.Parameters.Add("@totalrecord", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                totalRecord = (int)cmd.Parameters["@totalrecord"].Value;

                if (totalRecord <= pagesize)
                {
                    ViewBag.LoadClass = "hidden";
                }
                else
                {
                    ViewBag.LoadClass = "visible";
                }

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ViewBag.VideoData = dt;

                //var count = dt.Rows.Count;

                if (pagesize > 10)
                {
                    ViewBag.BackClass = "visible";
                }
                else
                {
                    ViewBag.BackClass = "hidden";
                }

                ViewBag.pageSize = pagesize;
                con.Close();
            }
            return View();
        }
    }
}