using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HelpDeskTeamProject.Context;
using HelpDeskTeamProject.DataModels;

namespace HelpDeskTeamProject.Services
{
    public class CommentService : ICommentManager
    {
        IAppContext db;
        ITicketLogger ticketLogger;

        public CommentService(IAppContext context, ITicketLogger tikLog)
        {
            db = context;
            ticketLogger = tikLog;
        }

        public async Task<Comment> Add(User user, Ticket baseTicket, string text)
        {
            if (user != null && baseTicket != null && text != null && text != "")
            {
                Comment newComment = new Comment(text, user, DateTime.Now, baseTicket.TeamId, baseTicket.Id);
                baseTicket.Comments.Add(newComment);
                ticketLogger.WriteTicketLog(user, TicketAction.CreateComment, baseTicket);
                await db.SaveChangesAsync();
                return newComment;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public async Task<bool> Delete(int? id, User user, Ticket ticket)
        {
            if (id != null && user != null && ticket != null)
            {
                Comment delComment = await db.Comments.SingleOrDefaultAsync(x => x.Id == id);
                db.Comments.Remove(delComment);
                ticketLogger.WriteTicketLog(user, TicketAction.DeleteComment, ticket);
                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}