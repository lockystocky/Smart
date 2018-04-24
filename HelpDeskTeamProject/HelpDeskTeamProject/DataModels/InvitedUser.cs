using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskTeamProject.DataModels
{
    public class InvitedUser
    {
        public int Id { get; set; }
                
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(0, 999999)]
        public int Code { get; set; }
    }
}