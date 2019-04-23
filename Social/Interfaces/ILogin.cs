using Social.Models;
using Social.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social.Interfaces
{
    public interface ILogin
    {
        (bool check, string message) CheckUser(UserViewModel user, out User _user);
    }
}