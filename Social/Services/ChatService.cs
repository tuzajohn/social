using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Helpers;
using Social.Interfaces;
using Social.Models;

namespace Social.Services
{
    public class ChatService : IChat
    {
        private YeauplatoContext context;
        public ChatService()
        {
            context = new YeauplatoContext();
        }
        public List<string> GetChatGroup(User user)
        {
            throw new NotImplementedException();
        }

        public string LaodLatesMessage(User user, int userId)
        {
            var message = "";

            var _message = context.Messages
                .Where(msg => (msg.SourceId == user.Id && msg.DestinationId == userId
                || msg.SourceId == userId && msg.DestinationId == user.Id)
                && msg.IsRead == false
            ).OrderByDescending(t => t.Time).FirstOrDefault();

            if(_message != null) message = ShowChatMessage(user, _message);

            return message;
        }

        public List<string> LaodMessages(User user, int userId)
        {
            var messages = context.Messages
                .Where(msg => msg.SourceId == user.Id && msg.DestinationId == userId
                || msg.SourceId == userId && msg.DestinationId == user.Id
            ).OrderBy(t => t.Time);

            var _messages = new List<string>();
            foreach (var message in messages)
            {
                string d = ShowChatMessage(user, message);

                _messages.Add(d);
            }

            return _messages;
        }

        private string ShowChatMessage(User user, Models.Message message)
        {
            var direction = (message.SourceId == user.Id ? "right" : "left");
            var profileImage = context.UserImages.Where(pi => pi.UserId == message.SourceId).OrderByDescending(t => t.Id).FirstOrDefault();

            var _userData = context.Users.FirstOrDefault(u => u.Id == message.SourceId);
            var image = "assets/images/demo/people/300x300/b-min.jpg";
            if (profileImage != null)
            {
                var imageFile = context.Images.FirstOrDefault(img => img.Id == profileImage.ImageId);
                if (imageFile != null)
                    image = imageFile.Url;
            }

            var isread = true;
            if (message.DestinationId == user.Id)
            {
                isread = message.IsRead == false ? false: true;
            }

            var d = $@"
                <div class='clearfix margin-bottom-10'>
                    <!-- post item -->
                    <img class='thumbnail pull-{direction}' src='{image}' width='60' height='60' alt='{_userData.FirstName} {_userData.OtherNames}' />
                    <h4 class='size-13 nomargin noborder nopadding'>{_userData.FirstName} {_userData.OtherNames}</h4>
                    <span class='size-13 text-muted'>
	                   {(isread == false?"<strong>":"")} {message.Details}
                        <br/>
	                    <span class='text-success size-11'>{Support.GetRelativetime(message.Time)}</span> 
                        {(isread == false ? "</strong>" : "")}
                    </span>
                </div><!-- /messge item -->";
            return d;
        }

        public string LatestChatMessage(User user)
        {
            var messages = context.Messages
                               .Where(msg => msg.DestinationId == user.Id
                           ).OrderByDescending(t=>t.Time).ToList();


            throw new NotImplementedException();
        }

        public bool ReadAll(User user)
        {
            var check = false;
            var messages = context.Messages.Where(msg => msg.DestinationId == user.Id && msg.IsRead == false).ToList();
            if (messages != null)
            {
                try
                {
                    messages.ForEach(msg => msg.IsRead = true);
                    context.SaveChanges();
                    check = true;
                }
                catch (Exception) {}
            }
            return check;
        }

        public bool ReadAll(User user, int userId)
        {
            var check = false;
            var messages = context.Messages
                .Where(msg => (msg.SourceId == user.Id && msg.DestinationId == userId
                || msg.SourceId == userId && msg.DestinationId == user.Id)
                && msg.IsRead == false
            ).ToList();

            if (messages != null)
            {
                try
                {
                    messages.ForEach(msg => msg.IsRead = true);
                    context.SaveChanges();
                    check = true;
                }
                catch (Exception) { }
            }
            return check;
        }

        public bool SendMessage(User user, int id, string content)
        {
            var check = false;
            try
            {
                var _message = new Models.Message
                {
                    DestinationId = id,
                    Details = content,
                    Id = Support.GetID(),
                    IsRead = false,
                    SourceId = user.Id,
                    Time = DateTime.UtcNow
                };


                var notification = context.Notifications.FirstOrDefault(_not => _not.SourceId == user.Id && _not.DestinationId == id);
                if (notification is null)
                {
                    notification = new Notification
                    { Id = Support.GetID(), Content = content, DestinationId = id, SourceId = user.Id, Time = DateTime.UtcNow, Count = 1 };
                }
                else
                {
                    var count = notification.Count + 1;
                    notification.Count = count;
                    notification.Content = content;
                    notification.DestinationId = id;
                    notification.SourceId = user.Id;
                    notification.Time = DateTime.UtcNow;
                }
                context.Messages.Add(_message);
                context.Notifications.Add(notification);
                context.SaveChanges();
                check = true;
            }
            catch (Exception) { }
            return check;
        }

        public int CountNewMessages(User user)
        {
            var messages = context.Messages.Where(msg => msg.DestinationId == user.Id).ToList();
            return messages.Count;
        }
        public int CountNewMessages(User user, int id)
        {
            var messages = context.Messages.Where(msg => msg.DestinationId == user.Id && msg.SourceId == id).ToList();
            return messages.Count;
        }
        public List<string> Notification(User user)
        {
            var notifications = context.Notifications
                .Where(not => not.DestinationId == user.Id)
                .OrderByDescending(t => t.Time)
                .ToList();
            var notificationList = new List<string>();
            foreach (var notification in notifications)
            {
                var _user = context.Users
                    .FirstOrDefault(u => u.Id == notification.SourceId);
                var d = $@"
                <a href='chat?id={Support.Encode(notification.SourceId)}'>
                    <!-- cart item -->
                    <img src='~/assets/images/demo/people/300x300/4-min.jpg' alt='' width='45' height='45'>
                    <h6><span>{notification.Count}</span> {_user.OtherNames}</h6>
                    <small>{(notification.Content.Length > 60 ? notification.Content.Remove(55) + "..." : notification.Content)}</small><br/>
                    <small>{Support.GetRelativetime(notification.Time)}</small>
                </a><!-- /cart item -->";
                notificationList.Add(d);
            }
            return notificationList;
        }
    }
}