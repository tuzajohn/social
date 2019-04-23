using Social.Helpers;
using Social.Interfaces;
using Social.Models;
using Social.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social.Services
{
    public class RegisterService : IRegister
    {
        private YeauplatoContext Context;
        public RegisterService(YeauplatoContext context)
        {
            Context = context;
        }
        public string AddNewUser(UserViewModel user)
        {
            var _user = new User
            {
                Country = "",
                Dob = user.Dob,
                Email = user.Email,
                FirstName = user.Fname,
                Gender = user.Gender,
                OtherNames = user.Lname,
                Password = Support.GetMD5(user.Password),
                Time = DateTime.UtcNow,
                Type = "normal",
                Id = Support.GetID()
            };

            Context.Users.Add(_user);
            Context.SaveChanges();
            return Support.ShowMessage("We sent a verification link to your mail, please verify!", Support.MessageType.SUCCESS);
        }

        public (bool check, string message) CheckUser(UserViewModel user)
        {
            if ( string.IsNullOrEmpty(user.Fname) || string.IsNullOrEmpty(user.Lname) || string.IsNullOrEmpty(user.Password))
            {
                return (false, Support.ShowMessage("Please, fill in all boxes!", Support.MessageType.ERROR));
            }
            if (!user.Check)
            {
                return (user.Check, Support.ShowMessage("Accept our terms and services!", Support.MessageType.ERROR));
            }
            if (user.Password != user.RepPassword)
            {
                return (false, Support.ShowMessage("Passwords are not matching", Support.MessageType.ERROR));
            }
            
            var (message, check) = Support.CheckPassword(user.Password);
            if (!check) { return (check, message); }

            if (user.Dob > DateTime.Now.AddYears(-13))
            {
                return (false, Support.ShowMessage("You're to young!", Support.MessageType.WARNING));
            }

            (message, check) = Support.CheckEmail(user.Email);
            if (!check) { return (check, message); }

            var _user = Context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (!(_user is null))
            {
                return (false, Support.ShowMessage("This email already exist with us!", Support.MessageType.WARNING));
            }

            return (true, null);
        }
    }
}