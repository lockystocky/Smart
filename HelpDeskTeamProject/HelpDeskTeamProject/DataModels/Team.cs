using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class Team
    {
        public int Id { get; set; }

        public Guid TeamGuid { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        public int OwnerId { get; set; }

        public virtual List<User> Users { get; set; }

        public virtual List<UserPermission> UserPermissions { get; set; }

        public virtual List<InvitationEmail> InvitedEmails { get; set; }

        public string InvitationLink { get; set; }

        public virtual List<Ticket> Tickets { get; set; }

        public Team()
        {

        }

        public Team(string name, Guid teamGuid, int ownerId)
        {
            Name = name;
            TeamGuid = teamGuid;
            OwnerId = ownerId;
        }
    }
}