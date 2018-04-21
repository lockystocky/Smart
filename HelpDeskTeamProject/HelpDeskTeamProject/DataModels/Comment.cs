using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class Comment
    {
        public int Id { get; set; }

        public User User { get; set; }

        public DateTime TimeCreated { get; set; }

        public string Text { get; set; }
    }
}