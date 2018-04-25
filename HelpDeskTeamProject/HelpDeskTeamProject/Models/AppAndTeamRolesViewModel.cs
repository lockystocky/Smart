using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.Models
{
    public class AppAndTeamRolesViewModel
    {
        public List<ApplicationRole> ApplicationRoles { get; set; }

        public List<TeamRole> TeamRoles { get; set; }
    }
}