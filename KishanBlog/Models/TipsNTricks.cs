using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KishanBlog.Models
{
    public class TipsNTricks
    {
        private int _tipsNTrickId;
        public int TipsNTrickId
        {
            get
            {
                return _tipsNTrickId;
            }
            set
            {
                _tipsNTrickId = value;
            }
        }

        private string _tipsNTrickTitle;
        [AllowHtml]
        public string TipsNTrickTitle
        {
            get
            {
                return _tipsNTrickTitle;
            }
            set
            {
                _tipsNTrickTitle = value;
            }
               
        }
       

        private string _tipsNTrickLongDescription;
        [AllowHtml]
        public string TipsNTrickLongDescription
        {
            get
            {
                return _tipsNTrickLongDescription;
            }
            set
            {
                _tipsNTrickLongDescription = value;
            }
        }

        private string _createdOn;
        public string CreatedOn
        {
            get
            {
                return _createdOn;
            }
            set
            {
                _createdOn = value;
            }
        }
        private string _modifiedOn;
        public string ModifiedOn
        {
            get
            {
                return _modifiedOn;
            }
            set
            {
                _modifiedOn = value;
            }
        }

        private string _publishOn;
        public string PublishOn
        {
            get
            {
                return _publishOn;
            }
            set
            {
                _publishOn = value;
            }
        }
    }
}