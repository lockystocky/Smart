namespace HelpDeskTeamProject.Migrations
{
    using HelpDeskTeamProject.DataModels;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HelpDeskTeamProject.Context.AppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "HelpDeskTeamProject.Context.AppContext";
        }

        protected override void Seed(HelpDeskTeamProject.Context.AppContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var teamMember1 = new User()
            {
                Email = "fgh@gmail.com",
                IsBanned = false,
                Name = "Jon",
                Surname = "Doe"
            };

            var teamMember2 = new User()
            {
                Email = "fff@gmail.com",
                IsBanned = false,
                Name = "David",
                Surname = "Lynch"
            };

            var teamMember3 = new User()
            {
                Email = "lll@gmail.com",
                IsBanned = false,
                Name = "Sara",
                Surname = "Parker"
            };

            var team1 = new Team()
            {
                TeamGuid = Guid.NewGuid(),
                OwnerId = 2,
                Name = "Team 666",
                Users = new System.Collections.Generic.List<User>(),
                UserPermissions = new System.Collections.Generic.List<UserPermission>()
            };

            team1.Users.Add(teamMember1);
            team1.Users.Add(teamMember2);
            team1.Users.Add(teamMember3);

            var teamRole1 = new TeamRole()
            {
                Name = "Customer",
                Permissions = new TeamPermissions()
                { CanCreateTicket = true, CanCommentTicket = true }
            };

            var teamRole2 = new TeamRole()
            {
                Name = "Technical",
                Permissions = new TeamPermissions()
                { CanCommentTicket = true, CanEditComments = true }
            };

            var perm1 = new UserPermission()
            {
                TeamId = team1.Id,
                User = teamMember1,
                TeamRole = teamRole1
                //, TeamRole = 
            };

            var perm2 = new UserPermission()
            {
                TeamId = team1.Id,
                User = teamMember2,
                TeamRole = teamRole1
                //, TeamRole = 
            };

            var perm3 = new UserPermission()
            {
                TeamId = team1.Id,
                User = teamMember3,
                TeamRole = teamRole2
                //, TeamRole = 
            };

            team1.UserPermissions.Add(perm1);
            team1.UserPermissions.Add(perm2);
            team1.UserPermissions.Add(perm3);

            context.Teams.Add(team1);
            context.SaveChanges();
        }
    }
}
