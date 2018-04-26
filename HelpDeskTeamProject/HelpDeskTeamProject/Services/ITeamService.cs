using HelpDeskTeamProject.DataModels;
using HelpDeskTeamProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTeamProject.Services
{
    public interface ITeamService
    {
        List<TeamMenuItem> GetUserTeamsList(User user);

        Team CreateTeamWithOwner(string teamName, User ownerOfTeam);

        int GetTeamOwnerId(int teamId);

        ManageTeamViewModel CreateManageTeamViewModel(int teamId);

        bool ChangeUserRoleInTeam(int userId, int teamId, int newRoleId);

        bool DeleteUserFromTeam(int userId, int teamId);

        bool IsUserAbleToManageTeam(int teamId, User user);

        string InviteUserToTeam(int teamId, string email);

        InviteUserToTeamViewModel CreateInviteUserToTeamViewModel(int teamId, User currentUser);

        JoinTeamViewModel CreateJoinTeamViewModel(Guid teamGuid, int invitedUserId);

        List<Team> GetAllTeams();

        string JoinTeam(int teamId, int invitedUserId, int enteredCode, User currentUser);

        User GetCurrentUser(string currentAppUserId);

        bool TeamExists(int teamId);
    }
}
