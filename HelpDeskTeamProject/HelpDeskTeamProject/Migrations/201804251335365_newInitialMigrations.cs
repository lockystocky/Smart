namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newInitialMigrations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Time = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Permissions_IsAdmin = c.Boolean(nullable: false),
                        Permissions_CanCreateUser = c.Boolean(nullable: false),
                        Permissions_CanManageUserRoles = c.Boolean(nullable: false),
                        Permissions_CanSeeAllTeams = c.Boolean(nullable: false),
                        Permissions_CanSeeListOfUsers = c.Boolean(nullable: false),
                        Permissions_CanBlockUsers = c.Boolean(nullable: false),
                        Permissions_CanCreateTeams = c.Boolean(nullable: false),
                        Permissions_CanManageTicketTypes = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeCreated = c.DateTime(nullable: false),
                        Text = c.String(nullable: false, maxLength: 400),
                        TeamId = c.Int(nullable: false),
                        Ticket_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.Ticket_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Ticket_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Surname = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false),
                        AppId = c.String(),
                        IsBanned = c.Boolean(nullable: false),
                        AppRole_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationRoles", t => t.AppRole_Id)
                .Index(t => t.AppRole_Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamGuid = c.Guid(nullable: false),
                        Name = c.String(maxLength: 30),
                        OwnerId = c.Int(nullable: false),
                        InvitationLink = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InvitedUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Code = c.Int(nullable: false),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.Team_Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 400),
                        State = c.Int(nullable: false),
                        TimeCreated = c.DateTime(nullable: false),
                        ParentTicket_Id = c.Int(),
                        Type_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.ParentTicket_Id)
                .ForeignKey("dbo.TicketTypes", t => t.Type_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.ParentTicket_Id)
                .Index(t => t.Type_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.TicketLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketId = c.Int(nullable: false),
                        Action = c.Int(nullable: false),
                        Text = c.String(),
                        Time = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: true)
                .Index(t => t.TicketId);
            
            CreateTable(
                "dbo.TicketTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserPermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        TeamRole_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TeamRoles", t => t.TeamRole_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.TeamRole_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.TeamRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Permissions_CanCreateTicket = c.Boolean(nullable: false),
                        Permissions_CanCommentTicket = c.Boolean(nullable: false),
                        Permissions_CanInviteToTeam = c.Boolean(nullable: false),
                        Permissions_CanSetUserRoles = c.Boolean(nullable: false),
                        Permissions_CanDeleteTickets = c.Boolean(nullable: false),
                        Permissions_CanEditTickets = c.Boolean(nullable: false),
                        Permissions_CanDeleteComments = c.Boolean(nullable: false),
                        Permissions_CanEditComments = c.Boolean(nullable: false),
                        Permissions_CanChangeTicketState = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeamUsers",
                c => new
                    {
                        Team_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team_Id, t.User_Id })
                .ForeignKey("dbo.Teams", t => t.Team_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Team_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "User_Id", "dbo.Users");
            DropForeignKey("dbo.TeamUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.TeamUsers", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.UserPermissions", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.UserPermissions", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserPermissions", "TeamRole_Id", "dbo.TeamRoles");
            DropForeignKey("dbo.Tickets", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Tickets", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Tickets", "Type_Id", "dbo.TicketTypes");
            DropForeignKey("dbo.TicketLogs", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.Comments", "Ticket_Id", "dbo.Tickets");
            DropForeignKey("dbo.Tickets", "ParentTicket_Id", "dbo.Tickets");
            DropForeignKey("dbo.InvitedUsers", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.Users", "AppRole_Id", "dbo.ApplicationRoles");
            DropIndex("dbo.TeamUsers", new[] { "User_Id" });
            DropIndex("dbo.TeamUsers", new[] { "Team_Id" });
            DropIndex("dbo.UserPermissions", new[] { "User_Id" });
            DropIndex("dbo.UserPermissions", new[] { "TeamRole_Id" });
            DropIndex("dbo.UserPermissions", new[] { "TeamId" });
            DropIndex("dbo.TicketLogs", new[] { "TicketId" });
            DropIndex("dbo.Tickets", new[] { "User_Id" });
            DropIndex("dbo.Tickets", new[] { "Type_Id" });
            DropIndex("dbo.Tickets", new[] { "ParentTicket_Id" });
            DropIndex("dbo.Tickets", new[] { "TeamId" });
            DropIndex("dbo.InvitedUsers", new[] { "Team_Id" });
            DropIndex("dbo.Users", new[] { "AppRole_Id" });
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropIndex("dbo.Comments", new[] { "Ticket_Id" });
            DropTable("dbo.TeamUsers");
            DropTable("dbo.TeamRoles");
            DropTable("dbo.UserPermissions");
            DropTable("dbo.TicketTypes");
            DropTable("dbo.TicketLogs");
            DropTable("dbo.Tickets");
            DropTable("dbo.InvitedUsers");
            DropTable("dbo.Teams");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
            DropTable("dbo.ApplicationRoles");
            DropTable("dbo.AdminLogs");
        }
    }
}
