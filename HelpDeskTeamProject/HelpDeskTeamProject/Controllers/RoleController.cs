using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HelpDeskTeamProject.Context;
using HelpDeskTeamProject.DataModels;
using HelpDeskTeamProject.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace HelpDeskTeamProject.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        IAppContext dbContext;// = new AppContext();

        public RoleController(IAppContext context)
        {
            dbContext = context;
        }


        public async Task<ActionResult> CreateTeamRole()
        {
            User curUser = await GetCurrentUser();
            if (curUser.AppRole.Permissions.CanManageUserRoles == true || curUser.AppRole.Permissions.IsAdmin == true)
            {
                return View();
            }
            return RedirectToAction("NoPermissionError", "Ticket");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTeamRole(TeamRole newTeamRole)
        {
            if (ModelState.IsValid)
            {
                User curUser = await GetCurrentUser();
                if (curUser.AppRole.Permissions.CanManageUserRoles == true || curUser.AppRole.Permissions.IsAdmin == true)
                {
                    if (await dbContext.TeamRoles.SingleOrDefaultAsync(x => x.Name.Equals(newTeamRole.Name)) == null)
                    {
                        dbContext.TeamRoles.Add(newTeamRole);
                        await dbContext.SaveChangesAsync();
                        return Redirect("/Role/List");
                    }
                    else
                    {
                        ModelState.AddModelError("NameExists", "Role with that name already exists.");
                    }
                }
                else
                {
                    return RedirectToAction("NoPermissionError", "Ticket");
                }
            }
            
            return View();
        }

        public async Task<ActionResult> CreateAppRole()
        {
            User curUser = await GetCurrentUser();
            if (curUser.AppRole.Permissions.CanManageUserRoles == true || curUser.AppRole.Permissions.IsAdmin == true)
            {
                return View();
            }
            return RedirectToAction("NoPermissionError", "Ticket");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAppRole(ApplicationRole newAppRole)
        {
            if (ModelState.IsValid)
            {
                User curUser = await GetCurrentUser();
                if (curUser.AppRole.Permissions.CanManageUserRoles == true || curUser.AppRole.Permissions.IsAdmin == true)
                {
                    if (await dbContext.AppRoles.SingleOrDefaultAsync(x => x.Name.Equals(newAppRole.Name)) == null)
                    {
                        dbContext.AppRoles.Add(newAppRole);
                        await dbContext.SaveChangesAsync();
                        return Redirect("/Role/List");
                    }
                    else
                    {
                        ModelState.AddModelError("NameExists", "Role with that name already exists.");
                    }
                }
                else
                {
                    return RedirectToAction("NoPermissionError", "Ticket");
                }
            }

            return View();
        }

        public async Task<ActionResult> List()
        {
            User curUser = await GetCurrentUser();
            if (curUser.AppRole.Permissions.CanManageUserRoles == true || curUser.AppRole.Permissions.IsAdmin == true)
            {
                var appRoles = await dbContext.AppRoles.ToListAsync();
                var teamRoles = await dbContext.TeamRoles.ToListAsync();
                var allRoles = new AppAndTeamRolesViewModel()
                {
                    TeamRoles = teamRoles,
                    ApplicationRoles = appRoles
                };

                return View(allRoles);
            }
            else
            {
                return RedirectToAction("NoPermissionError", "Ticket");
            }
        }

        public async Task<ActionResult> EditAppRole(int? roleId)
        {
            if (roleId != null || roleId < 1)
            {
                User curUser = await GetCurrentUser();
                if (curUser.AppRole.Permissions.CanManageUserRoles == true || curUser.AppRole.Permissions.IsAdmin == true)
                {
                    int goodId = Convert.ToInt32(roleId);
                    ApplicationRole role = await dbContext.AppRoles.SingleOrDefaultAsync(x => x.Id == goodId);
                    return View(role);
                }
                else
                {
                    return RedirectToAction("NoPermissionError", "Ticket");
                }
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
                User curUser = await GetCurrentUser();
                if (curUser.AppRole.Permissions.CanManageUserRoles == true || curUser.AppRole.Permissions.IsAdmin == true)
                {
                    int goodId = Convert.ToInt32(roleId);
                    TeamRole role = await dbContext.TeamRoles.SingleOrDefaultAsync(x => x.Id == goodId);
                    return View(role);
                }
                else
                {
                    return RedirectToAction("NoPermissionError", "Ticket");
                }
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
                User curUser = await GetCurrentUser();
                if (curUser.AppRole.Permissions.CanManageUserRoles == true || curUser.AppRole.Permissions.IsAdmin == true)
                {
                    var appRoles = dbContext.AppRoles.Where(x => x.Name.Equals(role.Name));
                    bool hasNoSameNames = true;
                    foreach (ApplicationRole value in appRoles)
                    {
                        if (value.Name.Equals(role.Name) && value.Id != role.Id)
                        {
                            hasNoSameNames = false;
                            break;
                        }
                    }
                    if (hasNoSameNames)
                    {
                        ApplicationRole dbRole = await dbContext.AppRoles.SingleOrDefaultAsync(x => x.Id.Equals(role.Id));
                        dbRole.Name = role.Name;
                        dbRole.Permissions = role.Permissions;
                        await dbContext.SaveChangesAsync();
                        return Redirect("/Role/List");
                    }
                    else
                    {
                        ModelState.AddModelError("NameExists", "Role with that name already exists.");
                    }
                }
                else
                {
                    return RedirectToAction("NoPermissionError", "Ticket");
                }
            }

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTeamRole(TeamRole role)
        {
            if (ModelState.IsValid)
            {
                User curUser = await GetCurrentUser();
                if (curUser.AppRole.Permissions.CanManageUserRoles == true || curUser.AppRole.Permissions.IsAdmin == true)
                {
                    var teamRoles = dbContext.TeamRoles.Where(x => x.Name.Equals(role.Name));
                    bool hasNoSameNames = true;
                    foreach (TeamRole value in teamRoles)
                    {
                        if (value.Name.Equals(role.Name) && value.Id != role.Id)
                        {
                            hasNoSameNames = false;
                            break;
                        }
                    }
                    if (hasNoSameNames)
                    {
                        TeamRole dbRole = await dbContext.TeamRoles.SingleOrDefaultAsync(x => x.Id.Equals(role.Id));
                        dbRole.Name = role.Name;
                        dbRole.Permissions = role.Permissions;
                        await dbContext.SaveChangesAsync();
                        return Redirect("/Role/List");
                    }
                    else
                    {
                        ModelState.AddModelError("NameExists", "Role with that name already exists.");
                    }
                }
                else
                {
                    return RedirectToAction("NoPermissionError", "Ticket");
                }
            }

            return View(role);
        }

        public async Task<JsonResult> GetUserTeamPermissions(int? teamId)
        {
            User appUser = await GetCurrentUser();
            if (appUser != null && teamId != null)
            {
                if (appUser.AppRole.Permissions.IsAdmin)
                {
                    TeamPermissions teamPerms = new TeamPermissions()
                    {
                        CanChangeTicketState = true,
                        CanCommentTicket = true,
                        CanCreateTicket = true,
                        CanDeleteComments = true,
                        CanDeleteTickets = true,
                        CanEditComments = true,
                        CanEditTickets = true,
                        CanInviteToTeam = true,
                        CanSetUserRoles = true
                    };
                    return Json(teamPerms, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TeamPermissions curPerms = appUser.Teams.SingleOrDefault(x => x.Id == teamId).UserPermissions
                    .SingleOrDefault(x => x.User.Id == appUser.Id).TeamRole.Permissions;
                    return Json(curPerms, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetUserAppPermissions()
        {
            User appUser = await GetCurrentUser();
            if (appUser != null)
            {
                ApplicationPermissions curPerms = appUser.AppRole.Permissions;
                if (curPerms != null)
                {
                    return Json(curPerms, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        private async Task<User> GetCurrentUser()
        {
            string userAppId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            User curUser = await dbContext.Users.SingleOrDefaultAsync(x => x.AppId.Equals(userAppId));
            return curUser;
        }
    }
}