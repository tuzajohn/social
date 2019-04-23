using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social.ViewModels
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Password { get; set; }
        public string RepPassword { get; set; }
        public bool Check { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }

    }
}