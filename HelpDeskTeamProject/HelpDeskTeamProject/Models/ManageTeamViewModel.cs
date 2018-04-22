using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDeskTeamProject.Models
{
    public class ManageTeamViewModel
    {
        public Team Team { get; set; }

        public List<TeamMemberInfo> TeamMembers { get; set; }

    }

    public class TeamMemberInfo
    {
        public User TeamMember { get; set; }

        public TeamRole TeamRole { get; set; }

        public SelectList AvailableTeamRoles { get; set; }
    }
}