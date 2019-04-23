using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Models;

namespace Social.Interfaces
{
    public interface IChat
    {
        bool ReadAll(User user);
        bool ReadAll(User user, int userId);
        List<string> GetChatGroup(User user);
        string LatestChatMessage(User user);
        List<string> LaodMessages(User user, int userId);
        string LaodLatesMessage(User user, int userId);
        bool SendMessage(User user, int id, string content);
        int CountNewMessages(User user);
        int CountNewMessages(User user, int id);
        List<string> Notification(User user);
    }
}