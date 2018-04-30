using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HelpDeskTeamProject.Services
{
    public interface IUserManager
    {
        Task<User> GetCurrentUser();
    }
}