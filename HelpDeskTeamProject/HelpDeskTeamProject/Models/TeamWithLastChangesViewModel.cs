using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.Models
{
    public class TeamWithLastChangesViewModel
    {
        public Team Team { get; set; }

        public string LastTicketText { get; set; }

        public string LastTicketAuthor { get; set; }

        public string LastTicketTime { get; set; }

    }
}