using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public enum TicketAction
    {
        Create,
        Delete,
        Edit,
        Comment,
        Resolve
    }

    public class TicketLog : Log
    {
        public int TicketId { get; set; }

        public virtual TicketAction Action { get; set; }
    }
}