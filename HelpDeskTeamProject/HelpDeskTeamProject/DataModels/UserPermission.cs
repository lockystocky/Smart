using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class UserPermission
    {
        public int Id { get; set; }

        public virtual TeamRole TeamRole { get; set; }

        public int TeamId { get; set; }

        public virtual User User { get; set; }
    }
}