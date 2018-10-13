using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;

namespace KishanBlog.Controllers
{
    public class TestYTController : Controller
    {
        // GET: TestYT  //AIzaSyDEgF1sMlkVTvBE6hWpmf5qrzUDvQURUKw
        public ActionResult Index()
        {
            //YouTubeService yt = new YouTubeService(new BaseClientService.Initializer() { ApiKey = "AIzaSyDEgF1sMlkVTvBE6hWpmf5qrzUDvQURUKw" });


            //var searchListRequest = yt.Search.List("snippet");
            //searchListRequest.ChannelId = "UCUZ2qW7u-6lAvz1nUtdFwhQ";
            //var searchListResult = searchListRequest.Execute();
            //foreach (var item in searchListResult.Items)
            //{
            //    Console.WriteLine("ID:" + item.Id.VideoId);
            //    Console.WriteLine("snippet:" + item.Snippet.Title);
            //}


            return View();
        }
    }
}