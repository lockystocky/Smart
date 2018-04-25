using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.Models
{
    public class JoinTeamViewModel
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public InvitedUser InvitedUser { get; set; }
    }
}