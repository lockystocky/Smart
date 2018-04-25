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

namespace HelpDeskTeamProject.Controllers
{
    public class TeamsController : Controller
    {
        private AppContext db = new AppContext();
        private string TEAM_OWNER_ROLE_NAME = WebConfigurationManager.AppSettings["TeamOwnerRoleName"];
        private string DEFAULT_TEAM_ROLE_NAME = WebConfigurationManager.AppSettings["DefaultTeamRoleName"];
        private const string SITE_LINK = "http://localhost:50244/";

        //view list of all existing teams
        public ActionResult AllTeams()
        {
            return View(db.Teams.ToList());
        }
        
        [Authorize]
        public ActionResult Teams()
        {
            return View();
        }

        //used in view to create teams menu for curent user 
        [Authorize]
        public ActionResult GetCurrentUserTeamsList()
        {            
            db.Configuration.ProxyCreationEnabled = false;

            var currentUser = GetCurrentUser();

            var currentUserTeamsList = db.Teams
                .Include(z => z.Users)
                .Include(t => t.Tickets)
                .Where(t => t.Users.Select(u => u.Id).Contains(currentUser.Id))
                .ToList();            

            List<TeamWithLastChangesViewModel> data = new List<TeamWithLastChangesViewModel>();
            foreach (var team in currentUserTeamsList)
            {
                var teamViewModel = CreateTeamWithLastChangesViewModel(team);
                data.Add(teamViewModel);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        private TeamWithLastChangesViewModel CreateTeamWithLastChangesViewModel(Team team)
        {
            Ticket lastTicketInTeam = team.Tickets
                    .OrderByDescending(ticket => ticket.TimeCreated)
                    .FirstOrDefault();

            TeamWithLastChangesViewModel teamViewModel = new TeamWithLastChangesViewModel();
            if (lastTicketInTeam != null)
            {
                teamViewModel.Team = team;
                teamViewModel.LastTicketText = lastTicketInTeam.Description;
                teamViewModel.LastTicketTime = MakeDateTimeMoreUserFriendly(lastTicketInTeam.TimeCreated, DateTime.Now);
                teamViewModel.LastTicketAuthor = lastTicketInTeam.User.Name + " " + lastTicketInTeam.User.Surname;
            }
            else
            {
                teamViewModel.Team = team;
            }

            return teamViewModel;
        }

        private string MakeDateTimeMoreUserFriendly(DateTime date, DateTime nowDate)
        {
            string userFriendlyDate = "";

            if (date.Year == nowDate.Year && date.Month == nowDate.Month && date.Day == nowDate.Day)
            {
                userFriendlyDate = (nowDate.Hour - date.Hour).ToString() + " hours ago";
            }
            else if (date.Year == nowDate.Year && date.Month == nowDate.Month)
            {
                userFriendlyDate = (nowDate.Day - date.Day).ToString() + " days ago";
            }
            else
                userFriendlyDate = date.ToString();

            return userFriendlyDate;
        }

        //create new team
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        
        //create new team
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Team team)
        {
            if (ModelState.IsValid)
            {               
                var helpDeskUser = GetCurrentUser();

                Team createdTeam = CreateTeam(team.Name, helpDeskUser.Id);
                createdTeam.Users.Add(helpDeskUser);
                db.Teams.Add(createdTeam);
                db.SaveChanges();

                return RedirectToAction("AllTeams");
            }

            return View(team);
        }

        private Team CreateTeam(string teamName, int ownerId)
        {
            Guid teamGuid = Guid.NewGuid();

            var owner = db.Users.Find(ownerId);

            Team team = new Team()
            {
                Name = teamName,
                OwnerId = ownerId,
                TeamGuid = teamGuid,
                InvitationLink = SITE_LINK +  "/teams/jointeam/" + teamGuid.ToString() + "/",
                InvitedUsers = new List<InvitedUser>(),
                UserPermissions = new List<UserPermission>(),
                Tickets = new List<Ticket>(),
                Users = new List<User>()
            };

            var ownerRole = db.TeamRoles
               .Where(tr => tr.Name == TEAM_OWNER_ROLE_NAME)
               .FirstOrDefault();


            if (ownerRole == null)
                ownerRole = CreateTeamOwnerRole();
            

            var ownerPermission = new UserPermission()
            {
                TeamRole = ownerRole,
                User = owner,
                TeamId = team.Id
            };

            team.UserPermissions.Add(ownerPermission);


            return team;
        }

        private TeamRole CreateTeamOwnerRole()
        {
            return new TeamRole()
            {
                Name = TEAM_OWNER_ROLE_NAME,
                Permissions = new TeamPermissions()
                {
                    CanInviteToTeam = true,
                    CanChangeTicketState = true,
                    CanCommentTicket = true,
                    CanCreateTicket = true,
                    CanDeleteComments = true,
                    CanDeleteTickets = true,
                    CanEditComments = true,
                    CanEditTickets = true,
                    CanSetUserRoles = true
                }
            };
        }

        public ActionResult TeamInfo(int? teamId)
        {
            var team = db.Teams
                .Where(t => t.Id == teamId).FirstOrDefault();

            if (team == null)
                return HttpNotFound();

            return View(team);
        }


        private User GetCurrentUser()
        {
            var context = new ApplicationDbContext();
            var currentUserId = User.Identity.GetUserId();

            var currentUser = db.Users
                .Where(user => user.AppId == currentUserId)
                .FirstOrDefault();

            return currentUser;
        }


        
        //Team Administrator must be able to manage Team  members and their roles


        public ActionResult ManageTeam(int? teamId)
        {
            var currentUser = GetCurrentUser();

            var team = db.Teams.Find(teamId);

            if (team == null)
                return HttpNotFound();
        
            if (currentUser.Id != team.OwnerId)
                return HttpNotFound();

            var teamMembers = team.Users;

            var viewModel = new ManageTeamViewModel()
            {
                Team = team,
                TeamMembers = new List<TeamMemberInfo>()
            };
            

            foreach (var teamMember in teamMembers)
            {
                if (teamMember.Id == team.OwnerId)
                    continue;

                var teamRole = team.UserPermissions
                    .Where(permission => permission.User == teamMember && permission.TeamId == team.Id)
                    .FirstOrDefault().TeamRole;
               
                var availableTeamRoles = db.TeamRoles
                    .Where(role => role.Name != TEAM_OWNER_ROLE_NAME).
                    ToList();

                var selectListForAvailableTeamRoles 
                    = new SelectList(availableTeamRoles, "Id", "Name", teamRole.Id); 

                var memberInfo = new TeamMemberInfo()
                {
                    TeamMember = teamMember,
                    TeamRole = teamRole,
                    AvailableTeamRoles = selectListForAvailableTeamRoles
                };

                viewModel.TeamMembers.Add(memberInfo);
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ChangeUserRoleInTeam(int userId, int teamId, int roleId)
        {
            var team = db.Teams.Find(teamId);

            var user = db.Users.Find(userId);

            var role = db.TeamRoles.Find(roleId);

            if (team == null || user == null || role == null)
                return HttpNotFound();

            var userPermission = team.UserPermissions
                .Where(up => up.User == user && up.TeamId == team.Id)
                .FirstOrDefault();

            if (userPermission == null)
                return HttpNotFound();

            if (userPermission.TeamRole != role)
                userPermission.TeamRole = role;

            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        
        [HttpPost]
        public ActionResult DeleteUserFromTeam(int userId, int teamId)
        {
            var team = db.Teams.Find(teamId);

            var user = db.Users.Find(userId);

            if (team == null || user == null)
                return HttpNotFound();

            var userPermission = team.UserPermissions
                .Where(up => up.User == user && up.TeamId == team.Id)
                .FirstOrDefault();
                       
            team.UserPermissions.Remove(userPermission);
            user.Teams.Remove(team);
            team.Users.Remove(user);
            var userTickets = team.Tickets.Where(t => t.User == user).ToList();
            team.Tickets.RemoveAll(ticket => userTickets.Contains(ticket));
            var teamTickets = team.Tickets.ToList();
            foreach(var ticket in teamTickets)
            {
                var userComments = ticket.Comments.Where(comment => comment.User == user);
                ticket.Comments.RemoveAll(t => userComments.Contains(t));
            }
            db.Permissions.Remove(userPermission);
            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public string GetTeamManagementLink(int? _teamId)
        {
            var team = db.Teams.Find(_teamId);
            if (team == null)
                return "";
           
            var currentUser = GetCurrentUser();

            if(team.OwnerId != currentUser.Id)
                return "";
                        
            return Url.Action("ManageTeam", new { teamId = _teamId }); 
        }

        //Team Administrator must be able to invite users into the team by adding user using email address 
        //and sending automatic invitation with link to registration

        [Authorize]
        public ActionResult InviteUser(int? teamId)
        {
            var currentUser = GetCurrentUser();

            var team = db.Teams.Find(teamId);

            if (team == null || team.OwnerId != currentUser.Id)
                return HttpNotFound();

            var viewModel = new InviteUserToTeamViewModel()
            {
                TeamToInvite = team
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult InviteUser(InviteUserToTeamViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var team = db.Teams.Find(viewModel.TeamToInvite.Id);
                var email = viewModel.EmailOfInvitedUser;

                bool isUserAlreadyInvited = team.InvitedUsers.Where(user => user.Email == email).Count() > 0;
                bool isUserAlreadyTeamMember = team.Users.Where(user => user.Email == email).Count() > 0;

                if(!isUserAlreadyInvited && !isUserAlreadyTeamMember)
                {
                    var userCode = GenerateInvitationCode();

                    var invitedUser = new InvitedUser()
                    {
                        Email = email,
                        Code = userCode
                    };

                    team.InvitedUsers.Add(invitedUser);
                    db.SaveChanges();

                    string invitationLink = team.InvitationLink + invitedUser.Id.ToString();
                    string emailMessage = CreateInvitationEmailMessage(team.Name, invitationLink, userCode);

                    SendInvitationEmail(email, emailMessage);
                }

                return RedirectToAction("ManageTeam", new { teamId = team.Id});
            }
            
            return View(viewModel);
        }

        private void SendInvitationEmail(string emailTo, string emailText)
        {
            string emailServiceLogin = WebConfigurationManager.AppSettings["EmailServiceLogin"];
            string emailServicePassword = WebConfigurationManager.AppSettings["EmailServicePassword"];

            var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(emailServiceLogin, emailServicePassword),
                EnableSsl = true
            };

            client.Send(emailServiceLogin, emailTo, "Help Desk Invitation To The Team", emailText);
        }

        private string CreateInvitationEmailMessage(string teamName, string invitationLink, int code)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("Dear Sir/Madam,");
            message.AppendLine();
            message.AppendLine($"You were invited to the team \"{teamName}\".");
            message.AppendLine($"To join the team follow the link: {invitationLink}");
            message.AppendLine($"Your code: {code}");
            message.AppendLine("If you are not interested in our service, delete this mail.");
            message.AppendLine();
            message.AppendLine("Yours faithfully,");
            message.AppendLine("Help Desk Service");
            return message.ToString();
        }

        private int GenerateInvitationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }

        [Authorize]
        [Route("teams/jointeam/{teamGuid}/{invitedUserId}")]
        public ActionResult JoinTeam(Guid teamGuid, int invitedUserId)
        {
            var team = db.Teams
                .Where(t => t.TeamGuid == teamGuid)
                .FirstOrDefault();

            if(team == null)
                return HttpNotFound();
            
            var invitedUser = team.InvitedUsers.Find(iu => iu.Id == invitedUserId);

            if (invitedUser == null)
                return HttpNotFound();

            invitedUser.Code = 0;
            
            var viewModel = new JoinTeamViewModel()
            {
                InvitedUser = invitedUser,
                TeamId = team.Id,
                TeamName = team.Name
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [Route("teams/jointeam/{teamGuid}/{invitedUserId}")]
        public ActionResult JoinTeam(JoinTeamViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var team = db.Teams.Find(viewModel.TeamId);

                if (team == null)
                    return HttpNotFound();

                var invitedUser = team.InvitedUsers
                    .Find(iu => iu.Id == viewModel.InvitedUser.Id);

                if (invitedUser == null)
                    return HttpNotFound();

                var currentUser = GetCurrentUser();

                if (invitedUser.Code == viewModel.InvitedUser.Code 
                    && currentUser.Email == invitedUser.Email
                    && !currentUser.Teams.Contains(team) && !team.Users.Contains(currentUser))
                {
                    team.Users.Add(currentUser);
                    team.InvitedUsers.Remove(invitedUser);

                    var defaultTeamRole = GetDefaultTeamRole();

                    var defaultUserPermission = new UserPermission()
                    {
                        TeamId = team.Id,
                        User = currentUser,
                        TeamRole = defaultTeamRole
                    };

                    team.UserPermissions.Add(defaultUserPermission);
                    currentUser.Teams.Add(team);

                    db.SaveChanges();

                    return Redirect("/ticket/tickets");
                }
                if (invitedUser.Code != viewModel.InvitedUser.Code)
                    ViewBag.InvalidCodeErrorMessage = "Code does not match.";

                if(currentUser.Email != invitedUser.Email)
                    ViewBag.InvalidCodeErrorMessage = "Your current email does not match email which refer to this invitation.";

                if (currentUser.Teams.Contains(team) || team.Users.Contains(currentUser))
                    ViewBag.InvalidCodeErrorMessage = "You are already in team.";

                return View(viewModel);
            }       

            return View(viewModel);
        }

        private TeamRole GetDefaultTeamRole()
        {
            var defaultTeamRole = db.TeamRoles
                        .Where(role => role.Name == DEFAULT_TEAM_ROLE_NAME)
                        .FirstOrDefault();

            if (defaultTeamRole == null)
                defaultTeamRole = CreateDefaultTeamRole();

            return defaultTeamRole;
        }

        private TeamRole CreateDefaultTeamRole()
        {
            return new TeamRole()
            {
                Name = DEFAULT_TEAM_ROLE_NAME,
                Permissions = new TeamPermissions()
                { CanCommentTicket = true, CanCreateTicket = true }
            };
        }

        /*

        // GET: Teams/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TeamGuid,Name,OwnerId,InvitationLink")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Team team = await db.Teams.FindAsync(id);
            db.Teams.Remove(team);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
