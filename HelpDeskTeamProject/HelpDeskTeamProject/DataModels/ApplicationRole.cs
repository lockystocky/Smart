using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class ApplicationRole
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ApplicationPermissions Permissions { get; set; }

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