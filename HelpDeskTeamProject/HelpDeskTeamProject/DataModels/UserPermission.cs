using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class UserPermission
    {
        public int Id { get; set; }

        public int TeamRoleId { get; set; }

        public int TeamId { get; set; }

        public int UserId { get; set; }
    }
}