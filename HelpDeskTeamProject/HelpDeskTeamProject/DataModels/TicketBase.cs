using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class TicketBase
    {
        public string Description { get; set; }

        public int TypeId { get; set; }

        public int? BaseTicketId { get; set; }

        public int BaseTeamId { get; set; }
    }
}