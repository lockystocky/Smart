using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDeskTeamProject.DataModels
{
    public class TeamRole : Role
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Remote("IsTeamRoleNameAvailable", "Validation", ErrorMessage = "Team role with such name already exists")]
        public string Name { get; set; }

        public virtual TeamPermissions Permissions { get; set; }

        
        public TeamRole()
        {

        }

        public TeamRole(string name, TeamPermissions permissions)
        {
            Name = name;
            Permissions = permissions;
        }
    }
}