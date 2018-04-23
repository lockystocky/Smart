using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string UserSurname { get; set; }

        public string TimeCreated { get; set; }

        public string Text { get; set; }

        public CommentDTO()
        {

        }

        public CommentDTO(int id, string text, User user, string time)
        {
            Id = id;
            Text = text;
            UserName = user.Name;
            UserSurname = user.Surname;
            TimeCreated = time;
        }
    }
}