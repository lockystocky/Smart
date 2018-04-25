using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class ApplicationPermissions
    {
        public bool IsAdmin { get; set; }

        public bool CanCreateUser { get; set; }

        public bool CanManageUserRoles { get; set; }

        public bool CanSeeAllTeams { get; set; }

        public bool CanSeeListOfUsers { get; set; }

        public bool CanBlockUsers { get; set; }

        public bool CanCreateTeams { get; set; }

        public bool CanManageTicketTypes { get; set; }

        public ApplicationPermissions()
        {

        }

        public ApplicationPermissions(bool isAdmin, bool canCreateUser, bool canManageUserRoles, bool canSeeAllTeams, bool canSeeListOfUsers, bool canBlockUsers, bool canCreateTeams, bool canManageTicketTypes)
        {
            IsAdmin = isAdmin;
            CanCreateTeams = canCreateTeams;
            CanCreateUser = canCreateUser;
            CanManageUserRoles = canManageUserRoles;
            CanSeeAllTeams = canSeeAllTeams;
            CanSeeListOfUsers = canSeeListOfUsers;
            CanBlockUsers = canBlockUsers;
            CanManageTicketTypes = canManageTicketTypes;
        }
    }
}