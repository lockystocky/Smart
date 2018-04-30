using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTeamProject.Services
{
    public interface IDtoConverter
    {
        TicketDTO ConvertTicket(Ticket ticket, User user, TeamPermissions permissions);
        CommentDTO ConvertComment(Comment comment, User user, TeamPermissions permissions);
        TicketLogDTO ConvertTicketLog(TicketLog log);
        List<TicketLogDTO> ConvertTicketLogList(List<TicketLog> logs);
        List<TicketDTO> ConvertTicketList(List<Ticket> tickets, User user, TeamPermissions permissions);
        List<CommentDTO> ConvertCommentList(List<Comment> comments, User user, TeamPermissions permissions);
    }
}
