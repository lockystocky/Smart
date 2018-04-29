using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public enum TicketAction
    {
        CreateComment,
        DeleteComment,
        CreateTicket,
        DeleteTicket,
        EditTicket,
        StateChange
    }

    public class TicketLog : Log
    {
        public int TicketId { get; set; }

        public virtual TicketAction Action { get; set; }
    }
}