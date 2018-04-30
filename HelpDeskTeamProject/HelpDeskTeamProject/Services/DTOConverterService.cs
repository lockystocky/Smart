using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HelpDeskTeamProject.DataModels;

namespace HelpDeskTeamProject.Services
{
    public class DTOConverterService : IDtoConverter
    {
        public CommentDTO ConvertComment(Comment comment, User user, TeamPermissions permissions)
        {
            if (comment != null && user != null && permissions != null)
            {
                CommentDTO dto = new CommentDTO(comment);
                if (comment.User.Id == user.Id || user.AppRole.Permissions.IsAdmin || permissions.CanDeleteComments)
                    dto.CanDelete = true;
                else
                    dto.CanDelete = false;
                return dto;
            }
            throw new ArgumentNullException();
        }

        public List<CommentDTO> ConvertCommentList(List<Comment> comments, User user, TeamPermissions permissions)
        {
            if (comments != null && user != null && permissions != null)
            {
                List<CommentDTO> returnList = new List<CommentDTO>();
                foreach (Comment value in comments)
                {
                    returnList.Add(ConvertComment(value, user, permissions));
                }
                return returnList;
            }
            throw new ArgumentNullException();
        }

        public TicketDTO ConvertTicket(Ticket ticket, User user, TeamPermissions permissions)
        {
            if (ticket != null && user != null && permissions != null)
            {
                TicketDTO dto = new TicketDTO(ticket);
                if (ticket.User.Id == user.Id || user.AppRole.Permissions.IsAdmin)
                {
                    dto.CanDelete = true;
                    dto.CanEdit = true;
                }
                else
                {
                    dto.CanDelete = permissions.CanDeleteTickets;
                    dto.CanEdit = permissions.CanEditTickets;
                }
                return dto;
            }
            throw new ArgumentNullException();
        }

        public List<TicketDTO> ConvertTicketList(List<Ticket> tickets, User user, TeamPermissions permissions)
        {
            if (tickets != null && user != null && permissions != null)
            {
                List<TicketDTO> returnList = new List<TicketDTO>();
                foreach (Ticket value in tickets)
                {
                    returnList.Add(ConvertTicket(value, user, permissions));
                }
                return returnList;
            }
            throw new ArgumentNullException();
        }

        public TicketLogDTO ConvertTicketLog(TicketLog log)
        {
            if (log != null)
            {
                TicketLogDTO dto = new TicketLogDTO(log);
                return dto;
            }
            throw new ArgumentNullException();
        }

        public List<TicketLogDTO> ConvertTicketLogList(List<TicketLog> logs)
        {
            if (logs != null)
            {
                List<TicketLogDTO> returnList = new List<TicketLogDTO>();
                foreach (TicketLog value in logs)
                {
                    returnList.Add(ConvertTicketLog(value));
                }
                return returnList;
            }
            throw new ArgumentNullException();
        }
    }
}