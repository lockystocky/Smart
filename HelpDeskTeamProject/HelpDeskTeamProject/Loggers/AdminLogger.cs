using HelpDeskTeamProject.Context;
using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HelpDeskTeamProject.Loggers
{
    public static class AdminLogger
    {
        public static List<AdminAction> CheckAction(User originalObject, User changedObject)
        {
            //List<AdminAction> adminActions = new List<AdminAction>();
            //if (originalObject.IsBanned != changedObject.IsBanned)
            //{
            //    adminActions.Add(AdminAction.BlockUser);
            //}
            //if (originalObject.AppRole.Permissions.IsAdmin != changedObject.AppRole.Permissions.IsAdmin)
            //{
            //    adminActions.Add(AdminAction.ChangeUserRole);
            //}
            //else
            //{
            //    adminActions.Add(AdminAction.ChangedUserData);
            //}

            //return adminActions;

            List<AdminAction> adminActions = new List<AdminAction>();
            if (originalObject.IsBanned != changedObject.IsBanned)
            {
                adminActions.Add(AdminAction.BlockUser);
            }
            if (originalObject.IsAdmin != changedObject.IsAdmin)
            {
                adminActions.Add(AdminAction.ChangeUserRole);
            }
            else
            {
                adminActions.Add(AdminAction.ChangedUserData);
            }

            return adminActions;
        }

        public static void PostLogToDb(AppContext db, User user, List<AdminAction> adminActions) 
        {
            string state = "";

            if (adminActions.Contains(AdminAction.BlockUser))
            {
                state += " was blocked or unblocked ";
            }
            if (adminActions.Contains(AdminAction.ChangeUserRole))
            {
                state += "application role was changed";
            }
            if (adminActions.Contains(AdminAction.ChangedUserData))
            {
                state += "data was changed";
            }

            db.AdminLogs.Add(new AdminLog
            {
                Text = "User #" + user.Id + ": " + state,
                Time = DateTime.Now,
                UserId = user.Id
            });
            db.SaveChanges();
        }
    }
}