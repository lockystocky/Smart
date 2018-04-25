using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Metadata;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskTeamProject.DataModels
{
    public class User
    {
        public int Id { get; set; }

        //[Required]
        //[StringLength(20)]
        public string Name { get; set; }

        //[Required]
        //[StringLength(20)]
        public string Surname { get; set; }

        //[Required]
        //[EmailAddress]
        public string Email { get; set; }

        public string AppId { get; set; }

        public bool IsBanned { get; set; }

        public virtual ApplicationRole AppRole { get; set; }

        public virtual List<Team> Teams { get; set; }

        public User()
        {
            Teams = new List<Team>();
        }

        public User(string email, string appId, ApplicationRole appRole)
        {
            Teams = new List<Team>();
            AppId = appId;
            Email = email;
            AppRole = appRole;
            IsBanned = false;
        }

        public User(string name, string surname, string email, string appId, ApplicationRole appRole)
        {
            Name = name;
            Surname = surname;
            Teams = new List<Team>();
            AppId = appId;
            Email = email;
            AppRole = appRole;
            IsBanned = false;
        }
    }
}