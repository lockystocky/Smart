namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using HelpDeskTeamProject.DataModels;
    

    internal sealed class Configuration : DbMigrationsConfiguration<HelpDeskTeamProject.Context.AppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HelpDeskTeamProject.Context.AppContext context)
        {
            //ApplicationRole defaultRole = new ApplicationRole("Default User", new ApplicationPermissions(false, false, false, false, false, false, true));
            //context.AppRoles.Add(defaultRole);
            //context.SaveChanges();

            //HelpDeskTeamProject.DataModels.TeamRole role = new DataModels.TeamRole();
            //role.Permissions = new DataModels.TeamPermissions();
            //role.Permissions.CanCommentTicket = true;
            //role.Name = "somerole";
            //context.TeamRoles.Add(role);
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
