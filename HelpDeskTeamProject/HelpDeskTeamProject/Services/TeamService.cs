using HelpDeskTeamProject.Context;
using HelpDeskTeamProject.DataModels;
using HelpDeskTeamProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace HelpDeskTeamProject.Services
{
    public class TeamService
    {
        private AppContext db = new AppContext();
        private string TEAM_OWNER_ROLE_NAME = WebConfigurationManager.AppSettings["TeamOwnerRoleName"];
        private string DEFAULT_TEAM_ROLE_NAME = WebConfigurationManager.AppSettings["DefaultTeamRoleName"];
        private const string SITE_LINK = "http://localhost:50244/";

        public List<TeamMenuItem> GetUserTeamsList(User user)
        {
            var userTeamsList = db.Teams
               .Where(t => t.Users.Select(u => u.Id).Contains(user.Id))
               .ToList();

            var teamMenu = new List<TeamMenuItem>();

            foreach(var team in userTeamsList)
            {
                var teamMenuItem = new TeamMenuItem()
                {
                    TeamId = team.Id,
                    TeamName = team.Name
                };
            }

            return teamMenu;
        }

        public Team CreateTeamWithOwner(string teamName, User ownerOfTeam)
        {
            Team createdTeam = CreateTeam(teamName, ownerOfTeam.Id);
            createdTeam.Users.Add(ownerOfTeam);
            db.Teams.Add(createdTeam);
            db.SaveChanges();

            return createdTeam;
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
                InvitationLink = SITE_LINK + "/teams/jointeam/" + teamGuid.ToString() + "/",
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

        public int GetTeamOwnerId(int teamId)
        {
            var team = db.Teams.Find(teamId);
            return team.OwnerId;
        }

        public ManageTeamViewModel CreateManageTeamViewModel(int teamId)
        {
            var team = db.Teams.Find(teamId);

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
            return viewModel;
        }

        public bool ChangeUserRoleInTeam(int userId, int teamId, int newRoleId)
        {
            var team = db.Teams.Find(teamId);

            var user = db.Users.Find(userId);

            var role = db.TeamRoles.Find(newRoleId);

            if (team == null || user == null || role == null)
                return false;

            var userPermission = team.UserPermissions
                .Where(up => up.User == user && up.TeamId == team.Id)
                .FirstOrDefault();

            if (userPermission == null)
                return false;

            if (userPermission.TeamRole != role)
                userPermission.TeamRole = role;

            db.SaveChanges();

            return true;
        }

        public bool DeleteUserFromTeam(int userId, int teamId)
        {
            var team = db.Teams.Find(teamId);

            var user = db.Users.Find(userId);

            if (team == null || user == null)
                return false;

            var userPermission = team.UserPermissions
                .Where(up => up.User == user && up.TeamId == team.Id)
                .FirstOrDefault();

            team.UserPermissions.Remove(userPermission);
            user.Teams.Remove(team);
            team.Users.Remove(user);
            var userTickets = team.Tickets.Where(t => t.User == user).ToList();
            team.Tickets.RemoveAll(ticket => userTickets.Contains(ticket));
            var teamTickets = team.Tickets.ToList();
            foreach (var ticket in teamTickets)
            {
                var userComments = ticket.Comments.Where(comment => comment.User == user);
                ticket.Comments.RemoveAll(t => userComments.Contains(t));
            }
            db.Permissions.Remove(userPermission);
            db.SaveChanges();

            return true;
        }

        public bool IsUserAbleToManageTeam(int teamId, User user)
        {
            var team = db.Teams.Find(teamId);

            if (team == null)
                return false;

            if (team.OwnerId != user.Id)
                return false;

            return true;
        }

        public string InviteUserToTeam(int teamId, string email)
        {
            var team = db.Teams.Find(teamId);

            bool isUserAlreadyInvited = team.InvitedUsers.Where(user => user.Email == email).Count() > 0;
            bool isUserAlreadyTeamMember = team.Users.Where(user => user.Email == email).Count() > 0;

            if (!isUserAlreadyInvited && !isUserAlreadyTeamMember)
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

                return "";
            }

            if (isUserAlreadyInvited)
                return "User with this email address is already invited to team.";

            return "User with this email address is already in team.";
        }

        private void SendInvitationEmail(string emailTo, string emailText)
        {
            string emailServiceLogin = WebConfigurationManager.AppSettings["EmailServiceLogin"];
            string emailServicePassword = WebConfigurationManager.AppSettings["EmailServicePassword"];

            var client = new SmtpClient("smtp.gmail.com", 587)
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

        public InviteUserToTeamViewModel CreateInviteUserToTeamViewModel(int teamId, User currentUser)
        {
            var team = db.Teams.Find(teamId);

            if (team == null || team.OwnerId != currentUser.Id)
                return null;

            var viewModel = new InviteUserToTeamViewModel()
            {
                TeamToInvite = team
            };

            return viewModel;
        }

        public JoinTeamViewModel CreateJoinTeamViewModel(Guid teamGuid, int invitedUserId)
        {
            var team = db.Teams
               .Where(t => t.TeamGuid == teamGuid)
               .FirstOrDefault();

            if (team == null)
                return null;

            var invitedUser = team.InvitedUsers.Find(iu => iu.Id == invitedUserId);

            if (invitedUser == null)
                return null;

            invitedUser.Code = 0;

            var viewModel = new JoinTeamViewModel()
            {
                InvitedUser = invitedUser,
                TeamId = team.Id,
                TeamName = team.Name
            };

            return viewModel;
        }

        public List<Team> GetAllTeams()
        {
            return db.Teams.ToList();
        }

        public string JoinTeam(int teamId, int invitedUserId, int enteredCode, User currentUser)
        {
            var team = db.Teams.Find(teamId);

            var invitedUser = team.InvitedUsers
                    .Find(iu => iu.Id == invitedUserId);

            if (invitedUser == null)
                return "User is not invited to team.";

            if (invitedUser.Code == enteredCode
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

                return "";
            }

            if (invitedUser.Code != enteredCode)
                return "Code does not match.";

            if (currentUser.Email != invitedUser.Email)
                return "Your current email does not match email which refer to this invitation.";

            if (currentUser.Teams.Contains(team) || team.Users.Contains(currentUser))
                return "You are already in team.";

            return "Unknown error";

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

        public User GetCurrentUser(string currentAppUserId)
        {
            var currentUser = db.Users
                .Where(user => user.AppId == currentAppUserId)
                .FirstOrDefault();

            return currentUser;
        }

        public bool TeamExists(int teamId)
        {
            var team = db.Teams.Find(teamId);

            return team != null;                
        }


    }

}