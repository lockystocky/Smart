using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HelpDeskTeamProject.Context;
using HelpDeskTeamProject.DataModels;

namespace HelpDeskTeamProject.Controllers
{
    public class TeamController : Controller
    {
        AppContext dbContext = new AppContext();


        public async Task<ActionResult> CreateTeamRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTeamRole(TeamRole newTeamRole)
        {
            if (ModelState.IsValid)
            {
                dbContext.TeamRoles.Add(newTeamRole);
                await dbContext.SaveChangesAsync();
            }
            
            return View();
        }

        public async Task<ActionResult> CreateAppRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAppRole(ApplicationRole newAppRole)
        {
            if (ModelState.IsValid)
            {
                dbContext.AppRoles.Add(newAppRole);
                await dbContext.SaveChangesAsync();
            }

            return View();
        }
    }
}