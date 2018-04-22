using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.Models
{
    public class ManageTeamViewModel
    {
        public int TeamId { get; set; }

        public List<User> TeamUsers { get; set; }

        public List<UserPermission> UserPermissions { get; set; }
    }
}