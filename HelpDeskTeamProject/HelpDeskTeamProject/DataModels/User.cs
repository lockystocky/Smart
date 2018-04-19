using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Metadata;

namespace HelpDeskTeamProject.DataModels
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public bool IsBanned { get; set; }

        public ApplicationRole AppRole { get; set; }

        public List<Team> Teams { get; set; }

        public User()
        {
            Teams = new List<Team>();
        }

        public User(string email, ApplicationRole appRole)
        {
            Teams = new List<Team>();
            Email = email;
            AppRole = appRole;
            IsBanned = false;
        }
    }
}