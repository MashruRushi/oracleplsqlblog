using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace KishanBlog.Models
{
    public class OracleTutorials
    {
        private int _tutorialId;
        public int TutorialId
        {
            get
            {
                return _tutorialId;
            }
            set
            {
                _tutorialId = value;
            }
        }
        private string _tutorialTabnName;
        public string TutorialTabName
        {
            get
            {
                return _tutorialTabnName;
            }
            set
            {
                _tutorialTabnName = value;
            }
        }

        private string _tutorialTitle;
        public string TutorialTitle
        {
            get
            {
                return _tutorialTitle;
            }
            set
            {
                _tutorialTitle = value;
            }
        }

        private string _tutorialDescription;
        [AllowHtml]
        public string TutorialDescription
        {
            get
            {
                return _tutorialDescription;
            }
            set
            {
                _tutorialDescription = value;
            }
        }

        private int _displayOrder;
        public int DisplayOrder
        {
            get
            {
                return _displayOrder;
            }
            set
            {
                _displayOrder = value;
            }
        }
        private int _views;
        public int Views
        {
            get
            {
                return _views;
            }
            set
            {
                _views = value;
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

        public bool IsDelete{ get; set; }        
    }
}