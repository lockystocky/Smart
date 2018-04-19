using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HelpDeskTeamProject.DataModels;

namespace HelpDeskTeamProject.Context
{
    public class AppContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamRole> TeamRoles { get; set; }

        public DbSet<ApplicationRole> AppRoles { get; set; }

        public DbSet<AdminLog> AdminLogs { get; set; }
    }
}