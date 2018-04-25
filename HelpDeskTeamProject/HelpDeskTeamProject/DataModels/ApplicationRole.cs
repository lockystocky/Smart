using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDeskTeamProject.DataModels
{
    public class ApplicationRole
    {

        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Remote("IsApplicationRoleNameAvailable", "Validation", ErrorMessage = "Application role with such name already exists")]
        public string Name { get; set; }

        public virtual ApplicationPermissions Permissions { get; set; }

        public ApplicationRole()
        {

        }

        public ApplicationRole(string name, ApplicationPermissions permissions)
        {
            Name = name;
            Permissions = permissions;
        }
    }
}