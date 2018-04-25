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
            ContextKey = "HelpDeskTeamProject.Context.AppContext";
        }

        protected override void Seed(HelpDeskTeamProject.Context.AppContext context)
        {
            //TeamRole role = context.TeamRoles.SingleOrDefault(x => x.Id == 1);
            //Team team = context.Teams.SingleOrDefault(x => x.Id == 1);
            //User user = context.Users.SingleOrDefault(x => x.Email.ToLower().Equals("K2@gmail.com".ToLower()));
            //UserPermission perms = new UserPermission();
            //perms.TeamId = team.Id;
            //perms.UserId = user.Id;
            //perms.TeamRole = role;
            //team.UserPermissions.Add(perms);
            //team.Users.Add(user);

            //TicketType t1 = new TicketType();
            //t1.Name = "Database problems";
            //TicketType t2 = new TicketType();
            //t2.Name = "Testing problems";
            //TicketType t3 = new TicketType();
            //t3.Name = "Global problems";
            //TicketType t4 = new TicketType();
            //t4.Name = "Version control problems";
            //context.TicketTypes.Add(t1);
            //context.TicketTypes.Add(t2);
            //context.TicketTypes.Add(t3);
            //context.TicketTypes.Add(t4);

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
