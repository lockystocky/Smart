using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class ApplicationRole : Role
    {
        
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