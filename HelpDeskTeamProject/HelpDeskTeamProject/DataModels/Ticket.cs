using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public int TeamId { get; set; }

        public User User { get; set; }

        [StringLength(400)]
        public string Description { get; set; }

        public TicketState State { get; set; }

        public string Description { get; set; }

        public virtual TicketType Type { get; set; }

        public DateTime TimeCreated { get; set; }

        public virtual Ticket ParentTicket { get; set; }

        public virtual List<Ticket> ChildTickets { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<TicketLog> Logs { get; set; }

        public Ticket()
        {
            ChildTickets = new List<Ticket>();
            Comments = new List<Comment>();
            Logs = new List<TicketLog>();
        }

        public Ticket(int teamId, User user, string description, TicketType type, DateTime timeCreated, TicketState state, Ticket parent)
        {
            TeamId = teamId;
            User = user;
            Description = description;
            Type = type;
            TimeCreated = timeCreated;
            State = state;
            ParentTicket = parent;
            ChildTickets = new List<Ticket>();
            Comments = new List<Comment>();
            Logs = new List<TicketLog>();
        }

        public Ticket(int teamId, User user, string description, TicketType type, DateTime timeCreated, TicketState state)
        {
            TeamId = teamId;
            User = user;
            Description = description;
            Type = type;
            TimeCreated = timeCreated;
            State = state;
            ChildTickets = new List<Ticket>();
            Comments = new List<Comment>();
            Logs = new List<TicketLog>();
        }
    }
}