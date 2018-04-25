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
            //TeamRole role = context.TeamRoles.SingleOrDefault(x => x.Id == 1);
            //Team team = context.Teams.SingleOrDefault(x => x.Id == 1);
            //User user = context.Users.SingleOrDefault(x => x.Email.ToLower().Equals("K2@gmail.com".ToLower()));
            //User user1 = context.Users.SingleOrDefault(x => x.Email.ToLower().Equals("K1@gmail.com".ToLower()));
            //team.Users.Add(user);
            //team.Users.Add(user1);
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

            //var teamMember1 = new User()
            //{
            //    Email = "fgh@gmail.com",
            //    IsBanned = false,
            //    Name = "Jon",
            //    Surname = "Doe"
            //};

            //var teamMember2 = new User()
            //{
            //    Email = "fff@gmail.com",
            //    IsBanned = false,
            //    Name = "David",
            //    Surname = "Lynch"
            //};

            //var teamMember3 = new User()
            //{
            //    Email = "lll@gmail.com",
            //    IsBanned = false,
            //    Name = "Sara",
            //    Surname = "Parker"
            //};

            //var team1 = new Team()
            //{
            //    TeamGuid = Guid.NewGuid(),
            //    OwnerId = 1,
            //    Name = "Team 666",
            //    Users = new System.Collections.Generic.List<User>(),
            //    UserPermissions = new System.Collections.Generic.List<UserPermission>()
            //};

            //team1.Users.Add(teamMember1);
            //team1.Users.Add(teamMember2);
            //team1.Users.Add(teamMember3);

            //var teamRole1 = new TeamRole()
            //{
            //    Name = "Customer",
            //    Permissions = new TeamPermissions()
            //    { CanCreateTicket = true, CanCommentTicket = true }
            //};

            //var teamRole2 = new TeamRole()
            //{
            //    Name = "Technical",
            //    Permissions = new TeamPermissions()
            //    { CanCommentTicket = true, CanEditComments = true }
            //};

            //var perm1 = new UserPermission()
            //{
            //    TeamId = team1.Id,
            //    User = teamMember1,
            //    TeamRole = teamRole1
            //    //, TeamRole = 
            //};

            //var perm2 = new UserPermission()
            //{
            //    TeamId = team1.Id,
            //    User = teamMember2,
            //    TeamRole = teamRole1
            //    //, TeamRole = 
            //};

            //var perm3 = new UserPermission()
            //{
            //    TeamId = team1.Id,
            //    User = teamMember3,
            //    TeamRole = teamRole2
            //    //, TeamRole = 
            //};

            //team1.UserPermissions.Add(perm1);
            //team1.UserPermissions.Add(perm2);
            //team1.UserPermissions.Add(perm3);

            //context.Teams.Add(team1);
            //context.SaveChanges();
        }
    }
}
