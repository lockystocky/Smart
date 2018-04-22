using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class TeamPermissions
    {
        public bool CanCreateTicket { get; set; }

        public bool CanCommentTicket { get; set; }

        public bool CanInviteToTeam { get; set; }

        public bool CanSetUserRoles { get; set; }

        public bool CanDeleteTickets { get; set; }

        public bool CanEditTickets { get; set; }

        public bool CanDeleteComments { get; set; }

        public bool CanEditComments { get; set; }

        public bool CanChangeTicketState { get; set; }
    }
}