using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HelpDeskTeamProject.Services
{
    public interface ICommentManager
    {
        Task<bool> Delete(int? id, User user, Ticket ticket);
        Task<Comment> Add(User user, Ticket baseTicket, string text);
    }
}