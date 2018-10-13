using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KishanBlog.Controllers
{
    public class TheBlogController : Controller
    {
        // GET: TheBlog
        public ActionResult Blog(int Id)
        {
            return View();
        }
    }
}