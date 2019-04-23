using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Interfaces;
using Social.Models;

namespace Social.Services
{
    public class UserService : IUser
    {
        private YeauplatoContext context;
        public UserService()
        {
            context = new YeauplatoContext();
        }
        public User GetUser(int id)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }
    }
}