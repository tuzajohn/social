using Social.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Interfaces
{
    interface INetwork
    {
        List<string> GetMyNetwork(User user, int page, int count);
        int CountMyNetwork(User user);
        (bool check, string message) SendRequest(User user, string reciverEmail);
        bool AcceptRequest(User user, string id);
        bool DeleteRequest(User user, string id);
        bool AcceptRequest(User user);
        /// <summary>
        /// Delete all request
        /// </summary>
        /// <param name="user">persone deleting</param>
        /// <returns></returns>
        bool DeleteRequest(User user);
        int CountNewRequest(User user);

        List<string> GetNewRequest(User user);
        bool IsInMyNetwork(int id);
        
    }
}
