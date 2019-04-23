using Social.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Interfaces
{
    interface IComment
    {
        List<Comment> GetComments(int id);
        bool SaveComment(string content, int elementId, User user);
        int CounComment(int id);
        string LatestComment(int id, User user);
    }
}
