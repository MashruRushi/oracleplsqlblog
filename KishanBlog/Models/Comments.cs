using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KishanBlog.Models
{
    public class Comments
    {       
        public int CommentId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string PicPath { get; set; }
        public string Comment { get; set; }
        public string CommentOn { get; set; }       
    }
}