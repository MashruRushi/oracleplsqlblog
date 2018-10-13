using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KishanBlog.Models
{
    public class InterviewQuestions
    {
        private int _interviewQuestionId;
        public int InterviewQuestionId
        {
            get
            {
                return _interviewQuestionId;
            }
            set
            {
                _interviewQuestionId = value;
            }
        }

        private string _interviewQuestion;

        public string InterviewQuestion
        {
            get
            {
                return _interviewQuestion;
            }
            set
            {
                _interviewQuestion = value;
            }
        }



        private string _interviewQuestionLongDescription;
        [AllowHtml]
        public string InterviewQuestionLongDescription
        {
            get
            {
                return _interviewQuestionLongDescription;
            }
            set
            {
                _interviewQuestionLongDescription = value;
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
        //[Required(ErrorMessage = "This field is required")]
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