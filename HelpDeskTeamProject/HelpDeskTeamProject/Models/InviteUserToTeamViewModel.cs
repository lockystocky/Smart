using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.Models
{
    public class InviteUserToTeamViewModel
    {
        public Team TeamToInvite { get; set; }

        [EmailAddress]
        public string EmailOfInvitedUser { get; set; }
    }
}