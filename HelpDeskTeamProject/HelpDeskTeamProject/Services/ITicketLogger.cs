using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTeamProject.Services
{
    public interface ITicketLogger
    {
        void WriteTicketLog(User user, TicketAction action, Ticket onTicket);
    }
}
