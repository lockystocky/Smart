using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public User User { get; set; }

        public string TimeCreated { get; set; }

        public string Text { get; set; }

        public CommentDTO()
        {

        }

        public CommentDTO(string text, User user, string time)
        {
            Text = text;
            User = user;
            TimeCreated = time;
        }
    }
}