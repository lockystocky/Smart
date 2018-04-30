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
    public class TicketTypeService : ITicketTypeManager
    {
        IAppContext db;

        public TicketTypeService(IAppContext context)
        {
            db = context;
        }

        public async Task<bool> CreateNew(TicketType newTicketType)
        {
            TicketType type = await db.TicketTypes.SingleOrDefaultAsync(x => x.Name.Equals(newTicketType.Name));
            if (type == null)
            {
                db.TicketTypes.Add(newTicketType);
                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<TicketType>> GetAllTypes()
        {
            List<TicketType> ticketTypes = await db.TicketTypes.ToListAsync();
            return ticketTypes;
        }
    }
}