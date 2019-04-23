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
    public class LoginService : ILogin
    {
        private YeauplatoContext context;
        public LoginService(YeauplatoContext context)
        {
            this.context = context;
        }
        public (bool check, string message) CheckUser(UserViewModel user, out User _user)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                _user = null;
                return (false, Support.ShowMessage("Email required!", Support.MessageType.ERROR));
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                _user = null;
                return (false, Support.ShowMessage("Password required!", Support.MessageType.ERROR));
            }
            var _password = Support.GetMD5(user.Password);
            _user = context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == _password);
            if (_user is null)
            {
                return (false, Support.ShowMessage("Wrong email or password!", Support.MessageType.ERROR));
            }
            return (true, null);
        }
    }
}