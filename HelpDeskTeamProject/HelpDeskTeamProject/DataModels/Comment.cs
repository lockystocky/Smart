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

        public Comment()
        {

        }

        public Comment(string text, User user, DateTime time)
        {
            Text = text;
            User = user;
            TimeCreated = time;
        }
    }
}