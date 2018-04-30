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
    public class TicketManagerService : ITicketManager
    {
        IAppContext db;
        ITicketLogger ticketLogger;

        public TicketManagerService(IAppContext context, ITicketLogger tikLog)
        {
            db = context;
            ticketLogger = tikLog;
        }

        public async Task<Ticket> Add(TicketBase newTicket, User user)
        {
            Ticket baseTicket = null;
            if (newTicket.BaseTicketId != null && newTicket.Description != null && newTicket.BaseTicketId > 0 && newTicket.BaseTeamId > 0)
            {
                baseTicket = await db.Tickets.SingleOrDefaultAsync(x => x.Id == newTicket.BaseTicketId);
            }
            if (newTicket.Description != null && newTicket.BaseTeamId > 0 && user != null && newTicket.TypeId > 0)
            {
                TicketType ticketType = await db.TicketTypes.SingleOrDefaultAsync(x => x.Id == newTicket.TypeId);
                if (ticketType != null)
                {
                    Ticket ticket = new Ticket(newTicket.BaseTeamId, user, newTicket.Description, ticketType, DateTime.Now, TicketState.New, baseTicket);
                    Ticket ticketFromDb = db.Tickets.Add(ticket);
                    ticketLogger.WriteTicketLog(user, TicketAction.CreateTicket, baseTicket);
                    await db.SaveChangesAsync();
                    return ticketFromDb;
                }
            }
            throw new ArgumentNullException();
        }

        public async Task<bool> ChangeState(int? ticketId, int? state, User user)
        {
            if (ticketId != null && state != null && ticketId > 0 && state >= 0 && state <= 3 && user != null)
            {
                Ticket ticket = await db.Tickets.SingleOrDefaultAsync(x => x.Id == ticketId);
                if (ticket != null)
                {
                    ticket.State = (TicketState)state;
                    ticketLogger.WriteTicketLog(user, TicketAction.StateChange, ticket);
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            throw new ArgumentNullException();
        }

        public async Task<bool> Delete(int? id, User user)
        {
            if (id != null && user != null)
            {
                Ticket ticket = await db.Tickets.Include(z => z.ParentTicket).SingleOrDefaultAsync(x => x.Id == id);
                if (ticket != null)
                {
                    if (ticket.Comments.Count > 0)
                    {
                        db.Comments.RemoveRange(ticket.Comments);
                    }
                    if (ticket.ChildTickets.Count > 0)
                    {
                        db.Tickets.RemoveRange(ticket.ChildTickets);
                    }
                    if (ticket.Logs.Count > 0)
                    {
                        db.TicketLogs.RemoveRange(ticket.Logs);
                    }
                    ticketLogger.WriteTicketLog(user, TicketAction.DeleteTicket, ticket.ParentTicket);
                    db.Tickets.Remove(ticket);
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            throw new ArgumentNullException();
        }

        public async Task<Ticket> Edit(int? id, string description, int? type, User user)
        {
            if (id != null && description != null && type != null && description != "" && user != null)
            {
                Ticket ticket = await db.Tickets.Include(y => y.User).SingleOrDefaultAsync(x => x.Id == id);
                TicketType newType = await db.TicketTypes.SingleOrDefaultAsync(x => x.Id == type);
                if (ticket != null && newType != null)
                {
                    ticket.Description = description;
                    ticket.Type = newType;
                    ticketLogger.WriteTicketLog(user, TicketAction.EditTicket, ticket);
                    await db.SaveChangesAsync();
                    return ticket;
                }
            }
            throw new ArgumentNullException();
        }

        public async Task<Ticket> GetTicketById(int? id)
        {
            if (id != null)
            {
                Ticket ticket = await db.Tickets.Include(y => y.User)
                    .Include(s => s.ChildTickets)
                    .SingleOrDefaultAsync(x => x.Id == id);
                if (ticket != null)
                {
                    ticket.ChildTickets = await db.Tickets.Include(z => z.User)
                       .Include(e => e.Type)
                       .Include(y => y.ChildTickets)
                       .Include(w => w.Comments)
                       .Where(x => x.ParentTicket.Id == ticket.Id)
                       .ToListAsync();
                    return ticket;
                }
            }
            throw new ArgumentNullException();
        }

        public async Task<Ticket> GetTicketNoInclude(int? id)
        {
            if (id != null)
            {
                Ticket ticket = await db.Tickets.Include(t => t.User).SingleOrDefaultAsync(x => x.Id == id);
                return ticket;
            }
            throw new ArgumentNullException();
        }

        public async Task<List<Ticket>> GetTicketsByTeam(int? teamId)
        {
            if (teamId != null)
            {
                Team curTeam = await db.Teams.Include(x => x.Tickets).SingleOrDefaultAsync(y => y.Id == teamId);
                if (curTeam != null)
                {
                    List<Ticket> curTickets = await db.Tickets.Include(x => x.ChildTickets).Include(y => y.Comments).Include(z => z.User)
                        .Where(s => s.ParentTicket == null).Where(q => q.TeamId == curTeam.Id).ToListAsync();
                    if (curTickets != null)
                    {
                        return curTickets;
                    }
                }
            }
            throw new ArgumentNullException();
        }

        public async Task<List<Ticket>> GetTicketsByTeamAndType(int? teamId, int? typeId)
        {
            if (teamId != null && typeId != null && teamId > 0 && typeId > 0)
            {
                TicketType curType = await db.TicketTypes.SingleOrDefaultAsync(x => x.Id == typeId);
                Team curTeam = await db.Teams.SingleOrDefaultAsync(x => x.Id == teamId);
                if (curTeam != null && curType != null)
                {
                    List<Ticket> ticketsList = await db.Tickets.Where(x => x.TeamId == curTeam.Id).Where(y => y.Type.Id == typeId).ToListAsync();
                    if (ticketsList != null)
                    {
                        return ticketsList;
                    }
                }
            }
            throw new ArgumentNullException();
        }
    }
}