using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace KishanBlog.Models
{
    public class AboutMe
    {
        private int _AboutMeId;
        public int AboutMeId
        {
            get { return _AboutMeId; }
            set { _AboutMeId = value; }
        }

        private EnumDescriptionType _DescriptionType;
        public EnumDescriptionType DescriptionType
        {
            get { return _DescriptionType; }
            set { _DescriptionType = value; }
        }        
        private string _AboutMeDescription;
        [AllowHtml]
        public string AboutMeDescription
        {
            get { return _AboutMeDescription; }
            set { _AboutMeDescription = value; }
        }

        public enum EnumDescriptionType:int
        {
            [Display(Name ="About Blog")]
            AboutBlog=0,
            [Display(Name = "About Experience")]
            AboutExperience =1
        }
    }    
}