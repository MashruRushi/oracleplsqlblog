using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KishanBlog.Areas.Admin.Models
{
    public class AdminLogin
    {
        public int AdminId { get; set; }

        [Required(ErrorMessage = "Admin name is required")]
        public string AdminName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string AdminPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public bool SuperAdmin { get; set; }
        
        public bool IsActive { get; set; }     
    }
}