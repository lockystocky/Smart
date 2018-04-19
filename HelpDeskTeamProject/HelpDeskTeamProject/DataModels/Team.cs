using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class Team
    {
        public int Id { get; set; }

        public Guid TeamGuid { get; set; }

        public string Name { get; set; }

        public int OwnerId { get; set; }

        public List<User> Users { get; set; }

        public List<UserPermission> UserPermissions { get; set; }

        public List<string> InvitedEmails { get; set; }

        public string InvitationLink { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}