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
            //Ticket t = context.Tickets.SingleOrDefault(x => x.Id == 1);
            //Team team = new Team("someteam", Guid.NewGuid(), 111);
            //t.Team = team;
            
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

            //Ticket tk = new Ticket();
            //User usr = new User();
            //usr.IsBanned = false;
            //usr.Name = "Vasya";
            //usr.Surname = "Petrenko";
            //TicketType type = new TicketType();
            //type.Name = "Database problems";
            //Comment com = new Comment();
            //com.User = usr;
            //com.Text = "I decided to write here something just to test my app.";
            //com.TimeCreated = DateTime.Now;
            //tk.User = usr;
            //tk.Type = type;
            //tk.Comments = new System.Collections.Generic.List<Comment>();
            //tk.Comments.Add(com);
            //tk.Description = "Подскажите, пожалуйста. Когда делаю git merge, появляются конфликты в файле .csproj. Оставляю лишь свои изменения, после этого этот файл становится недоступным. Как правильно делать git merge для файла .csproj?";
            //tk.State = TicketState.New;
            //tk.TimeCreated = DateTime.Now;
            //context.Tickets.Add(tk);

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
