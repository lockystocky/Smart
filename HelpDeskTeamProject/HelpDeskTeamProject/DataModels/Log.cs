using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class Log
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime Time { get; set; }

        public int UserId { get; set; }
    }
}