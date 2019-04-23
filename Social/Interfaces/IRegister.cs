using Social.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social.Interfaces
{
    public interface IRegister
    {
        (bool check, string message) CheckUser(UserViewModel user);
        string AddNewUser(UserViewModel user);
    }
}