using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HelpDeskTeamProject.DataModels;

namespace HelpDeskTeamProject.Services
{
    public class TicketLoggerService : ITicketLogger
    {
        public void WriteTicketLog(User user, TicketAction action, Ticket onTicket)
        {
            if (user != null && onTicket != null)
            {
                TicketLog curLog = new TicketLog()
                {
                    Action = action,
                    TicketId = onTicket.Id,
                    Time = DateTime.Now,
                    User = user
                };
                switch (action)
                {
                    case TicketAction.CreateComment:
                        {
                            curLog.Text = string.Format("{0} {1} left a comment.", user.Name, user.Surname);
                            break;
                        }
                    case TicketAction.DeleteComment:
                        {
                            curLog.Text = string.Format("{0} {1} deleted a comment.", user.Name, user.Surname);
                            break;
                        }
                    case TicketAction.CreateTicket:
                        {
                            curLog.Text = string.Format("{0} {1} created a new ticket.", user.Name, user.Surname);
                            break;
                        }
                    case TicketAction.DeleteTicket:
                        {
                            curLog.Text = string.Format("{0} {1} deleted a ticket.", user.Name, user.Surname);
                            break;
                        }
                    case TicketAction.EditTicket:
                        {
                            curLog.Text = string.Format("{0} {1} edited a ticket.", user.Name, user.Surname);
                            break;
                        }
                    case TicketAction.StateChange:
                        {
                            curLog.Text = string.Format("{0} {1} changed ticket's state.", user.Name, user.Surname);
                            break;
                        }
                }
                onTicket.Logs.Add(curLog);
            }
        }
    }
}