﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KishanBlog.Models
{
    public class Replies
    {
        public int ReplyId { get; set; }
        public int CommentId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string PicPath { get; set; }
        public string Reply { get; set; }
        public string ReplyOn { get; set; }
    }
}