using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDeskTeamProject.DataModels
{
    public class TicketType
    {
        public int Id { get; set; }

        //[Required]
        //[StringLength(20)]
        //[Remote("IsTicketNameAvailable", "Validation", ErrorMessage = "Ticket type with such name already exists")]
        public string Name { get; set; }
    }
}