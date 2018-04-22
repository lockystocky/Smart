﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.DataModels
{
    public class TeamRole : Role
    {
        public virtual TeamPermissions Permissions { get; set; }

        [StringLength(40)]
        public string Name { get; set; }

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