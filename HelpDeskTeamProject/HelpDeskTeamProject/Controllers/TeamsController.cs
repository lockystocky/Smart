using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HelpDeskTeamProject.Context;
using HelpDeskTeamProject.DataModels;
using HelpDeskTeamProject.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Text;
using System.Web.Configuration;
using HelpDeskTeamProject.Services;

namespace HelpDeskTeamProject.Controllers
{
    public class TeamsController : Controller
    {
        private string TEAM_OWNER_ROLE_NAME = WebConfigurationManager.AppSettings["TeamOwnerRoleName"];
        private string DEFAULT_TEAM_ROLE_NAME = WebConfigurationManager.AppSettings["DefaultTeamRoleName"];
        private ITeamService teamService;

        public TeamsController(ITeamService teamService)
        {
            this.teamService = teamService;
        }
       
        
        [Authorize]
        public ActionResult GetCurrentUserTeamsList()
        {            
            var currentUser = GetCurrentUser();
            var currentUserTeamsList = teamService.GetUserTeamsList(currentUser);     

            return Json(currentUserTeamsList, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public ActionResult GetCurrentUserName()
        {
            var currentUser = GetCurrentUser();
            string name = currentUser.Name + " " + currentUser.Surname;

            return Json(name, JsonRequestBehavior.AllowGet);
        }

        
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Team team)
        {
            if (ModelState.IsValid)
            {               
                var currentUser = GetCurrentUser();
                Team createdTeam = teamService.CreateTeamWithOwner(team.Name, currentUser);
                
                return RedirectToAction("ManageTeam", new {teamId = createdTeam.Id });
            }

            return View(team);
        }

        private User GetCurrentUser()
        {
            var context = new ApplicationDbContext();
            var currentUserId = User.Identity.GetUserId();
            
            return teamService.GetCurrentUser(currentUserId);
        }
        
        public ActionResult ManageTeam(int? teamId)
        {
            var currentUser = GetCurrentUser();

            if (!teamService.TeamExists((int)teamId))
                return HttpNotFound();
        
            if (currentUser.Id != teamService.GetTeamOwnerId((int)teamId))
                return HttpNotFound();            

            ManageTeamViewModel viewModel = teamService.CreateManageTeamViewModel((int)teamId);

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult ChangeUserRoleInTeam(int userId, int teamId, int roleId)
        {
            bool successfulChanged = teamService.ChangeUserRoleInTeam(userId, teamId, roleId);

            if (!successfulChanged)
                return HttpNotFound();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        
        [HttpPost]
        public ActionResult DeleteUserFromTeam(int userId, int teamId)
        {
            var teamExists = teamService.TeamExists(teamId);

            if (!teamExists)
                return HttpNotFound();

            bool successfulluDeleted = teamService.DeleteUserFromTeam(userId, teamId);
            
            if(!successfulluDeleted)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public string GetTeamManagementLink(int? _teamId)
        {
            if (_teamId == null)
                return "";

            var currentUser = GetCurrentUser();

            bool isUserAbleToManageTeam = teamService.IsUserAbleToManageTeam((int)_teamId, currentUser);
           
            if(!isUserAbleToManageTeam)
                return "";
                        
            return Url.Action("ManageTeam", new { teamId = _teamId }); 
        }

        
        [Authorize]
        public ActionResult InviteUser(int? teamId)
        {
            var currentUser = GetCurrentUser();
            
            var viewModel = teamService.CreateInviteUserToTeamViewModel((int)teamId, currentUser);

            if (viewModel == null)
                return HttpNotFound();

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult InviteUser(InviteUserToTeamViewModel viewModel)
        {
            if (ModelState.IsValid)
            {                
                var _teamId = viewModel.TeamToInvite.Id;
                var email = viewModel.EmailOfInvitedUser;

                string errorMessage = teamService.InviteUserToTeam(_teamId, email);

                if(errorMessage == "")
                    return RedirectToAction("ManageTeam", new { teamId = _teamId});

                ViewBag.ErrorMessage = errorMessage;
                return View(viewModel);
            }
            
            return View(viewModel);
        }

        [Authorize]
        [Route("teams/jointeam/{teamGuid}/{invitedUserId}")]
        public ActionResult JoinTeam(Guid teamGuid, int invitedUserId)
        {
            var viewModel = teamService.CreateJoinTeamViewModel(teamGuid, invitedUserId);

            if (viewModel == null)
                return HttpNotFound();

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [Route("teams/jointeam/{teamGuid}/{invitedUserId}")]
        public ActionResult JoinTeam(JoinTeamViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var teamId = viewModel.TeamId;

                if (!teamService.TeamExists(teamId))
                    return HttpNotFound();
                
                var currentUser = GetCurrentUser();

                var errorMessage = teamService.JoinTeam(teamId, viewModel.InvitedUser.Id, viewModel.InvitedUser.Code, currentUser);

                if(errorMessage == "")
                    return Redirect("/ticket/tickets");

                ViewBag.ErrorMessage = errorMessage;

                return View(viewModel);
            }       

            return View(viewModel);
        }

      
        protected override void Dispose(bool disposing)
        {            
            base.Dispose(disposing);
        }
    }
}
