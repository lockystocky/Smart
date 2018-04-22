using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskTeamProject.DataModels
{
    public class Role
    {
        public int Id { get; set; }

        [StringLength(40)]
        public string Name { get; set; }
    }
}