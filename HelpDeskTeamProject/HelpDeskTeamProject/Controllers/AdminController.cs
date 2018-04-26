using HelpDeskTeamProject.Context;
using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HelpDeskTeamProject.Loggers;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HelpDeskTeamProject.Controllers
{
    public class AdminController : Controller
    {
        private AppContext dbContext = new AppContext();
        
        public async Task<ActionResult> EditRolesList()
        {
            string userAppId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            User curUser = await dbContext.Users.SingleOrDefaultAsync(x => x.AppId.Equals(userAppId));
            if (curUser != null)
            {
                if (curUser.AppRole.Permissions.CanManageUserRoles || curUser.AppRole.Permissions.IsAdmin)
                {
                    IEnumerable<User> usersList = await dbContext.Users.Include(x => x.AppRole).ToListAsync();
                    List<UserDTO> dtoUsersList = new List<UserDTO>();
                    foreach (User value in usersList)
                    {
                        dtoUsersList.Add(new UserDTO(value));
                    }
                    List<ApplicationRole> appRoles = await dbContext.AppRoles.ToListAsync();
                    ViewBag.AppRoles = appRoles;
                    return View(dtoUsersList);
                }
                else
                {
                    return RedirectToAction("NoPermissionError", "Ticket");
                }
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public async Task<ActionResult> EditRolesListSave(string ids, string values)
        {
            string[] userIdsArray = ids.Split(',');
            string[] appIdsArray = values.Split(',');
            string userAppId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            User curUser = await dbContext.Users.SingleOrDefaultAsync(x => x.AppId.Equals(userAppId));
            if (curUser != null)
            {
                if (curUser.AppRole.Permissions.CanManageUserRoles || curUser.AppRole.Permissions.IsAdmin)
                {
                    if (userIdsArray.Length > 0 && appIdsArray.Length > 0 && userIdsArray.Length == appIdsArray.Length)
                    {
                        List<ApplicationRole> roles = await dbContext.AppRoles.ToListAsync();
                        for (int counter = 0;counter < userIdsArray.Length; counter++)
                        {
                            int tempId = Convert.ToInt32(userIdsArray[counter]);
                            User temp = await dbContext.Users.SingleOrDefaultAsync(x => x.Id == tempId);
                            if (temp != null)
                            {
                                int tempRoleId = Convert.ToInt32(appIdsArray[counter]);
                                temp.AppRole = roles.SingleOrDefault(x => x.Id == tempRoleId);
                            }
                        }
                        await dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    return RedirectToAction("NoPermissionError", "Ticket");
                }
            }
            return RedirectToAction("EditRolesList", "Admin");
        }

        // GET: Admin
        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            var currentUser = dbContext.Users.Where(u => u.Email == userName).FirstOrDefault();
            if (currentUser != null)
            {
                if (currentUser.AppRole.Permissions.IsAdmin)
                {
                    dynamic compositeModel = new ExpandoObject();
                    compositeModel.Users = GetUsers();
                    compositeModel.TeamRoles = GetTeamRoles();
                    compositeModel.AppRoles = GetAppRoles();
                    return View(compositeModel);
                }
            }            
            return View("Error");           
        }

        public ActionResult GetTeamsAndRoles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = dbContext.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }            
            return View(user);
        }

        public ActionResult Edit(int? id = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = dbContext.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                dbContext.AppRoles.Attach(user.AppRole);
                if (user.Id == 0)
                {
                    dbContext.Users.Add(user);
                }
                else
                {
                    var userInDb = dbContext.Users.Include(u => u.AppRole)
                        .SingleOrDefault(u => u.Id == user.Id);
                    bool wasChangedUserInDb = userInDb!= user;
                    if (wasChangedUserInDb)
                    {                        
                        AdminLogger.PostLogToDb(dbContext, userInDb, AdminLogger.CheckAction(userInDb, user));
                    }
                    if (userInDb != null)
                    {
                        dbContext.Entry(userInDb).CurrentValues.SetValues(user);
                        userInDb.AppRole.Permissions = user.AppRole.Permissions;
                    }
                }
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult ShowAdminLogs()
        {
            return View(dbContext.AdminLogs.ToList());
        }

        private List<User> GetUsers()
        {
            return dbContext.Users.ToList();
        }

        private List<TeamRole> GetTeamRoles()
        {
            return dbContext.TeamRoles.ToList();
        }

        private List<ApplicationRole> GetAppRoles()
        {
            return dbContext.AppRoles.ToList();
        }
    }
}