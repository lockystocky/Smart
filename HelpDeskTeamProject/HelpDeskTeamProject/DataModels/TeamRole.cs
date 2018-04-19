using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class TeamRole
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual TeamPermissions Permissions { get; set; }
    }
}