using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KishanBlog.Models
{
    public class Login
    {
        private int _userId;
        public int UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }

        private RegisterTypeEnum _registerType;
        public RegisterTypeEnum RegisterType
        {
            get
            {
                return _registerType;
            }
            set
            {
                _registerType = value;
            }
        }

        private string _email;
        [Required(ErrorMessage ="This field is required")]
        [EmailAddress(ErrorMessage ="Enter valid email id")]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }
        private string _password;
        [Required(ErrorMessage = "This field is required")]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        private string _confirmPassword;
        [Required(ErrorMessage = "This field is required")]
        [Compare("Password",ErrorMessage ="Password and Confirm Password didn't match")]
        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value;
            }
        }

        private bool _remember;
        [Display(Name ="Remember Me")]
        public bool Remember
        {
            get
            {
                return _remember;
            }
            set
            {
                _remember = value;
            }
        }

        private bool _isBlock;        
        public bool IsBlock
        {
            get
            {
                return _isBlock;
            }
            set
            {
                _isBlock = value;
            }
        }

        private string _firstName;
        [Required(ErrorMessage = "This field is required")]
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }

        private string _lastName;
        [Required(ErrorMessage = "This field is required")]
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
        }

        private string _photoPath;
        public string PhotoPath
        {
            get
            {
                return _photoPath;
            }
            set
            {
                _photoPath = value;
            }
        }
    
        private string _country;
        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                _country = value;
            }
        }

        private string _state;
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        private string _city;
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
            }
        }

        private string _postalAddress;
        public string PostalAddress
        {
            get
            {
                return _postalAddress;
            }
            set
            {
                _postalAddress = value;
            }
        }

        private UserTypeEnum _userType;
        public UserTypeEnum UserType
        {
            get
            {
                return _userType;
            }
            set
            {
                _userType = value;
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

        public enum RegisterTypeEnum:int
        {
           NormalRegister=0,
           GmailRegister=1,
           LinkedInRegister=2
        }     
        
        public enum UserTypeEnum
        {
            Student=1,
            Professor=2,
            Developer=3,
            Other=4
        }           
    }    
}