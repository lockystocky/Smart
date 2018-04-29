using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class TicketLogDTO
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Time { get; set; }

        public string UserName { get; set; }

        public string UserSurname { get; set; }

        public TicketLogDTO()
        {

        }

        public TicketLogDTO(TicketLog ticketLog)
        {
            Id = ticketLog.Id;
            Text = ticketLog.Text;
            Time = ticketLog.Time.ToString();
            UserName = ticketLog.User.Name;
            UserSurname = ticketLog.User.Surname;
        }
    }
}