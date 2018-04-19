using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public bool IsBanned { get; set; }

        ApplicationRole AppRole { get; set; }

        List<Team> Teams { get; set; }
    }
}