using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social.Models;

namespace Social.Interfaces
{
    interface ITimeLine
    {
        List<string> GetTimeLines(int page, int count, User user);
        string ShowTimeLinePost(Article article);
    }
}
