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

namespace HelpDeskTeamProject.Controllers
{
    public class AdminController : Controller
    {
        private IAppContext dbContext;// = new AppContext();

        public AdminController(IAppContext context)
        {
            dbContext = context;
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