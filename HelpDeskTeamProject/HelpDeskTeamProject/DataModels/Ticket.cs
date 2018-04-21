using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public enum TicketState
    {
        New,
        InProgress,
        Done,
        Rejected
    }

    public enum StatusColors
    {
        orangered,
        darkorange,
        limegreen,
        dimgrey
    }

    public class Ticket
    {
        public int Id { get; set; }

        public Team Team { get; set; }

        public User User { get; set; }

        public TicketState State { get; set; }

        public string Description { get; set; }

        public virtual TicketType Type { get; set; }

        public DateTime TimeCreated { get; set; }

        public virtual Ticket ParentTicket { get; set; }

        public virtual List<Ticket> ChildTickets { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<TicketLog> Logs { get; set; }
    }
}