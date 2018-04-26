using HelpDeskTeamProject.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Security.Principal;

namespace HelpDeskTeamProject.Services
{
    public static class AdminService
    {
        public static bool CreateAdminButton(string userName)
        {
            AppContext dbContext = new AppContext();

            var currentUser = dbContext.Users.Where(u => u.Email == userName).FirstOrDefault();
            if (currentUser != null)
            {
                if (currentUser.IsAdmin)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
    
