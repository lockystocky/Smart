using HelpDeskTeamProject.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Security.Principal;

namespace HelpDeskTeamProject.Services
{
    public class AdminService
    {
        public bool CreateAdminButton(IPrincipal user)
        {
            AppContext dbContext = new AppContext();

            var userName = user.Identity.GetUserName();
            var currentUser = dbContext.Users.Where(u => u.Email == userName).FirstOrDefault();
            if (currentUser != null)
            {
                if (currentUser.AppRole.Permissions.IsAdmin)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
    
