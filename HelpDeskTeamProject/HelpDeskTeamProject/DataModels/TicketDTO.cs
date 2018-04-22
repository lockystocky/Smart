using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class TicketDTO
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public User User { get; set; }

        public TicketState State { get; set; }

        public string Description { get; set; }

        public virtual TicketType Type { get; set; }

        public string TimeCreated { get; set; }

        public int ChildTicketsCount { get; set; }

        public int CommentsCount { get; set; }

        public TicketDTO()
        {

        }

        public TicketDTO(int id, int teamId, User user, string description, TicketType type, string timeCreated, TicketState state, int childCount, int commentsCount)
        {
            Id = id;
            TeamId = teamId;
            User = user;
            Description = description;
            Type = type;
            TimeCreated = timeCreated;
            State = state;
            ChildTicketsCount = childCount;
            CommentsCount = commentsCount;
        }

        public TicketDTO(Ticket ticket)
        {
            Id = ticket.Id;
            TeamId = ticket.TeamId;
            User = ticket.User;
            Description = ticket.Description;
            Type = ticket.Type;
            TimeCreated = ticket.TimeCreated.ToString();
            State = ticket.State;
            ChildTicketsCount = ticket.ChildTickets.Count;
            CommentsCount = ticket.Comments.Count;
        }
    }
}