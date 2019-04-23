using Social.Helpers;
using Social.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social.Interfaces
{
    interface IPost
    {
        string SavePost(string post, User user);
        string SavePost(Posting post, User user);
        List<string> GetPosts(int start, int count, User user);
        Article GetPost(int articleid);
        int CountPost(User user);
        string ShowPost(Article article);
    }
}