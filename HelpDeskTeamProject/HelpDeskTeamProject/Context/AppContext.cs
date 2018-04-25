using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.Context
{
    public class AppContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public AppContext() : base("name=AppContext")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamRole> TeamRoles { get; set; }

        public DbSet<ApplicationRole> AppRoles { get; set; }

        public DbSet<AdminLog> AdminLogs { get; set; }


        public DbSet<UserPermission> Permissions { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketType> TicketTypes { get; set; }

        public DbSet<Comment> Comments { get; set; }

    }
}
