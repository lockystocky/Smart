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

        public bool CanDelete { get; set; }

        public bool CanEdit { get; set; }

        public string UserName { get; set; }

        public string UserSurname { get; set; }

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
            UserName = user.Name;
            UserSurname = user.Surname;
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
            UserName = ticket.User.Name;
            UserSurname = ticket.User.Surname;
            Description = ticket.Description;
            Type = ticket.Type;
            TimeCreated = ticket.TimeCreated.ToString();
            State = ticket.State;
            ChildTicketsCount = ticket.ChildTickets.Count;
            CommentsCount = ticket.Comments.Count;
        }
    }
}