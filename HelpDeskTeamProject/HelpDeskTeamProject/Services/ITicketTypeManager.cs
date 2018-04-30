using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HelpDeskTeamProject.DataModels;

namespace HelpDeskTeamProject.Services
{
    public interface ITicketTypeManager
    {
        Task<List<TicketType>> GetAllTypes();
        Task<bool> CreateNew(TicketType newTicketType);
    }
}