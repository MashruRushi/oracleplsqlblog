using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace KishanBlog.Models
{
    public class Blog
    {
        private int _blogId;
        public int BlogId
        {
            get
            {
                return _blogId;
            }
            set
            {
                _blogId = value;
            }
        }

        private string _blogTitle;
        [Required(ErrorMessage ="This field is required")]
        public string BlogTitle
        {
            get
            {
                return _blogTitle;
            }
            set
            {
                _blogTitle = value;
            }
        }

        private string _blogShortDescription;
        [Required(ErrorMessage ="This field is required")]
        public string BlogShortDescription
        {
            get
            {
                return _blogShortDescription;
            }
            set
            {
                _blogShortDescription = value;
            }
        }

        private string _bloglongDescription;
        [Required(AllowEmptyStrings = false,ErrorMessage ="This field is required")]       
        [AllowHtml] 
        public string BlogLongdescription
        {
            get
            {
                return _bloglongDescription;
            }
            set
            {
                _bloglongDescription = value;
            }
        }

        private int _videoId;
        public int VideoId
        {
            get
            {
                return _videoId;
            }
            set
            {

                _videoId = value;
            }
        }

        private string _ytVideoId;
        public string YtVideoId
        {
            get
            {
                return _ytVideoId;
            }
            set
            {
                _ytVideoId = value;
            }
        }

        private string _ytVideoUrl;
        public string YtVideoUrl
        {
            get
            {
                return _ytVideoUrl;
            }
            set
            {
                _ytVideoUrl = value;
            }
        }


        private string _ytVideoTitle;
        public string YtVideoTitle
        {
            get
            {
                return _ytVideoTitle;
            }
            set
            {
                _ytVideoTitle = value;
            }
        }

        private string _videoDescription;
        public string VideoDescription
        {
            get
            {
                return _videoDescription;
            }
            set
            {
                _videoDescription = value;
            }
        }

        private string _thumbnailUrl;
        public string ThumbnailUrl
        {
            get
            {
                return _thumbnailUrl;
            }
            set
            {
                _thumbnailUrl = value;
            }
        }

        private string _blogPic;
        public string BlogPic
        {
            get
            {
                return _blogPic;
            }
            set
            {
                _blogPic = value;
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
        [Required(ErrorMessage ="This field is required")]
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

        private LikeTypeEnum _likeType;
        public LikeTypeEnum LikeType
        {
            get
            {
                return _likeType;
            }
            set
            {
                _likeType = value;
            }
        }

        private int _likeId;

        public int LikeId
        {
            get
            {
                return _likeId;
            }
            set
            {
                _likeId = value;
            }
        }

        private int _referanceId;
        public int ReferanceId
        {
            get
            {
                return _referanceId;
            }
            set
            {
                _referanceId = value;
            }
        }
                           

        public enum LikeTypeEnum
        {
            Blog=0,
            OracleTutorial=1,
            TipsAndTricks=2
        }


        public List<Blog> ListVideo
        {
            get;
            set;
        }

        public int CommentId
        {
            get;
            set;
        }

        public string CommentType
        {
            get;
            set;
        }

        public string Comments
        {
            get;
            set;
        }     

        public int UserId
        {
            get;
            set;
        }

        public string CommentedOn
        {
            get;
            set;
        }

        public int ReplyId
        {
            get;
            set;
        }

        public string Reply
        {
            get;
            set;
        }

        public string ReplyType
        {
            get;
            set;
        }

        public string RepliedOn
        {
            get;
            set;
        }

        [Required(AllowEmptyStrings = false,ErrorMessage = "This field is required")]
        public string Tags
        {
            get;
            set;
        }
    }
} 