using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HelpDeskTeamProject.Context;
using HelpDeskTeamProject.DataModels;

namespace HelpDeskTeamProject.Controllers
{
    public class RoleController : Controller
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

        public async Task<ActionResult> List()
        {
            IEnumerable<Role> allRoles = Enumerable.Concat<Role>(dbContext.TeamRoles.ToList(), dbContext.AppRoles.ToList());
            return View(allRoles);
        }

        public async Task<ActionResult> EditAppRole(int? roleId)
        {
            if (roleId != null || roleId < 1)
            {
                int goodId = Convert.ToInt32(roleId);
                ApplicationRole role = await dbContext.AppRoles.SingleOrDefaultAsync(x => x.Id == goodId);
                return View(role);
            }
            else
            {
                return Redirect("/Role/List");
            }
        }

        public async Task<ActionResult> EditTeamRole(int? roleId)
        {
            if (roleId != null || roleId < 1)
            {
                int goodId = Convert.ToInt32(roleId);
                TeamRole role = await dbContext.TeamRoles.SingleOrDefaultAsync(x => x.Id == goodId);
                return View(role);
            }
            else
            {
                return Redirect("/Role/List");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAppRole(ApplicationRole role)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole dbRole = await dbContext.AppRoles.SingleOrDefaultAsync(x => x.Id.Equals(role.Id));
                dbRole.Name = role.Name;
                dbRole.Permissions = role.Permissions;
                await dbContext.SaveChangesAsync();
                return Redirect("/Role/List");
            }

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTeamRole(TeamRole role)
        {
            if (ModelState.IsValid)
            {
                TeamRole dbRole = await dbContext.TeamRoles.SingleOrDefaultAsync(x => x.Id.Equals(role.Id));
                dbRole.Name = role.Name;
                dbRole.Permissions = role.Permissions;
                await dbContext.SaveChangesAsync();
                return Redirect("/Role/List");
            }

            return View(role);
        }
    }
}