using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KishanBlog.Models
{
    public class YouTubeVideo
    {
        public int VideoId { get; set;}

        public string YTVideoId { get; set; }

        public string VideoIdUrl { get; set; }

        public string VideoTitle { get; set; }

        public string VideoDescription { get; set; }

        public string ThumbnailUrl { get; set; }

        public string CreatedOn { get; set; }

        public string PublishedAt { get; set; }
    }
}