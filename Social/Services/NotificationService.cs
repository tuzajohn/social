using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Interfaces;
using Social.Models;

namespace Social.Services
{
    public class NotificationService : INotification
    {
        private YeauplatoContext contex;
        public NotificationService()
        {
            contex = new YeauplatoContext();
        }
        public List<KeyValuePair<string, int>> NewNotifications(User user, int userId)
        {
            throw new NotImplementedException();
        }
    }
}