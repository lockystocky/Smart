using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class Ticket
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public TicketType Type { get; set; }

        public DateTime TimeCreated { get; set; }

        public Ticket ParentTicket { get; set; }

        public List<Ticket> ChildTickets { get; set; }

        public List<Comment> Comments { get; set; }

        public List<TicketLog> Logs { get; set; }
    }
}