using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class Comment
    {
        public int Id { get; set; }

        public User User { get; set; }

        public DateTime TimeCreated { get; set; }

        [Required]
        [StringLength(400)]
        public string Text { get; set; }

        public int TeamId { get; set; }

        public int BaseTicketId { get; set; }

        public Comment()
        {

        }

        public Comment(string text, User user, DateTime time, int teamId, int baseTicketId)
        {
            Text = text;
            User = user;
            TimeCreated = time;
            TeamId = teamId;
            BaseTicketId = baseTicketId;
        }
    }
}