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

        public int UserId { get; set; }

        public DateTime TimeCreated { get; set; }

        [StringLength(400)]
        public string Text { get; set; }
    }
}