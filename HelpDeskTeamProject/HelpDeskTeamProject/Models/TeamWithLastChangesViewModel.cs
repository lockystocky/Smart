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

        public Ticket LastTicket { get; set; }

    }
}