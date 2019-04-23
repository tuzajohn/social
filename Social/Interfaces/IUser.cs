using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social.Models;

namespace Social.Interfaces
{
    interface IUser
    {
        User GetUser(int id);
    }
}
