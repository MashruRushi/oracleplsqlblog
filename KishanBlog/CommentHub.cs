using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.SignalR.Hubs;
using System.Data.SqlClient;
using System.Configuration;
using KishanBlog.Models;
using System.Data;

namespace KishanBlog
{
    [HubName("Comment")]
    public class CommentHub : Hub
    {
        public void Load()
        {
            Clients.All.LoadComment("hello");
        }
    }
}