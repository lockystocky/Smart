using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTeamProject.Services
{
    public interface ITicketManager
    {
        Task<bool> ChangeState(int? ticketId, int? state, User user);
        Task<Ticket> Edit(int? id, string description, int? type, User user);
        Task<bool> Delete(int? id, User user);
        Task<List<Ticket>> GetTicketsByTeam(int? teamId);
        Task<Ticket> GetTicketById(int? id);
        Task<Ticket> Add(TicketBase newTicket, User user);
        Task<List<Ticket>> GetTicketsByTeamAndType(int? teamId, int? typeId);
        Task<Ticket> GetTicketNoInclude(int? id);
    }
}
