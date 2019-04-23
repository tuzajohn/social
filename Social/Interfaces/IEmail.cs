using Social.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Social.Interfaces
{
    interface IEmail
    {
        bool SendCrushEmail(string email, string emailBody);
        (bool check, string message) SendInviteEmail(User user, string email, string emailBody);
        /// <summary>
        /// this sends email from a sender to a receiver with a certain smtp and network credentials
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        bool SendEmail(string mail, string body);
        string InviteTemplate(string name, string url);
        string RequestTemplate(string email);
        string CrushTemplate(string email);
    }
}
