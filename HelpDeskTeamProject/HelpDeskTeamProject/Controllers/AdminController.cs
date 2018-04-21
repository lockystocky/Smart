using HelpDeskTeamProject.Context;
using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDeskTeamProject.Controllers
{
    public class AdminController : Controller
    {
        private AppContext dbContext = new AppContext();
        
        // GET: Admin
        public ActionResult Index()
        {
            dynamic compositeModel = new ExpandoObject();
            compositeModel.Users = GetUsers();
            compositeModel.TeamRoles = GetTeamRoles();
            compositeModel.AppRoles = GetAppRoles();
            return View(compositeModel);
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