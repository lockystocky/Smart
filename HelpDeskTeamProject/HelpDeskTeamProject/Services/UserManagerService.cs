using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HelpDeskTeamProject.Context;
using HelpDeskTeamProject.DataModels;
using Microsoft.AspNet.Identity;

namespace HelpDeskTeamProject.Services
{
    public class UserManagerService : IUserManager
    {
        IAppContext db;

        public UserManagerService(IAppContext context)
        {
            db = context;
        }

        public async Task<User> GetCurrentUser()
        {
            string userAppId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            User curUser = await db.Users.SingleOrDefaultAsync(x => x.AppId.Equals(userAppId));
            if (curUser != null)
            {
                return curUser;
            }
            throw new ArgumentNullException();
        }
    }
}