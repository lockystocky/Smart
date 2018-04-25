using HelpDeskTeamProject.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace HelpDeskTeamProject.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ValidationController : Controller
    {
        private AppContext db = new AppContext();

        
        public JsonResult IsApplicationRoleNameAvailable(string Name)
        {
            bool applicationRoleIsAvailable = db.AppRoles
                .Where(role => role.Name == Name)
                .Count() == 0;

            return Json(applicationRoleIsAvailable, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsTeamRoleNameAvailable(string Name)
        {
            bool teamRoleIsAvailable = db.TeamRoles
                .Where(role => role.Name == Name)
                .Count() == 0;

            return Json(teamRoleIsAvailable, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsTeamNameAvailable(string Name)
        {
            bool teamNameIsAvailable = db.Teams
                .Where(team => team.Name == Name)
                .Count() == 0;

            return Json(teamNameIsAvailable, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsTicketTypeNameAvailable(string Name)
        {
            bool ticketTypeNameIsAvailable = db.TicketTypes
                .Where(ticketType => ticketType.Name == Name)
                .Count() == 0;

            return Json(ticketTypeNameIsAvailable, JsonRequestBehavior.AllowGet);
        }
    }
}