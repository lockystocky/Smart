namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialMigration : DbMigration
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
                        Name = c.String(),
                        Permissions_IsAdmin = c.Boolean(nullable: false),
                        Permissions_CanCreateUser = c.Boolean(nullable: false),
                        Permissions_CanManageUserRoles = c.Boolean(nullable: false),
                        Permissions_CanSeeAllTeams = c.Boolean(nullable: false),
                        Permissions_CanSeeListOfUsers = c.Boolean(nullable: false),
                        Permissions_CanBlockUsers = c.Boolean(nullable: false),
                        Permissions_CanCreateTeams = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeamRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Permissions_CanCreateTicket = c.Boolean(nullable: false),
                        Permissions_CanCommentTicket = c.Boolean(nullable: false),
                        Permissions_CanInviteToTeam = c.Boolean(nullable: false),
                        Permissions_CanSetUserRoles = c.Boolean(nullable: false),
                        Permissions_CanDeleteTickets = c.Boolean(nullable: false),
                        Permissions_CanEditTickets = c.Boolean(nullable: false),
                        Permissions_CanDeleteComments = c.Boolean(nullable: false),
                        Permissions_CanEditComments = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamGuid = c.Guid(nullable: false),
                        Name = c.String(),
                        OwnerId = c.Int(nullable: false),
                        InvitationLink = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TimeCreated = c.DateTime(nullable: false),
                        ParentTicket_Id = c.Int(),
                        Type_Id = c.Int(),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.ParentTicket_Id)
                .ForeignKey("dbo.TicketTypes", t => t.Type_Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.ParentTicket_Id)
                .Index(t => t.Type_Id)
                .Index(t => t.Team_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TimeCreated = c.DateTime(nullable: false),
                        Text = c.String(),
                        Ticket_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.Ticket_Id)
                .Index(t => t.Ticket_Id);
            
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
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserPermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamRoleId = c.Int(nullable: false),
                        TeamId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        IsBanned = c.Boolean(nullable: false),
                        AppRole_Id = c.Int(),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationRoles", t => t.AppRole_Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.AppRole_Id)
                .Index(t => t.Team_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.Users", "AppRole_Id", "dbo.ApplicationRoles");
            DropForeignKey("dbo.UserPermissions", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Tickets", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.Tickets", "Type_Id", "dbo.TicketTypes");
            DropForeignKey("dbo.TicketLogs", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.Comments", "Ticket_Id", "dbo.Tickets");
            DropForeignKey("dbo.Tickets", "ParentTicket_Id", "dbo.Tickets");
            DropIndex("dbo.Users", new[] { "Team_Id" });
            DropIndex("dbo.Users", new[] { "AppRole_Id" });
            DropIndex("dbo.UserPermissions", new[] { "TeamId" });
            DropIndex("dbo.TicketLogs", new[] { "TicketId" });
            DropIndex("dbo.Comments", new[] { "Ticket_Id" });
            DropIndex("dbo.Tickets", new[] { "Team_Id" });
            DropIndex("dbo.Tickets", new[] { "Type_Id" });
            DropIndex("dbo.Tickets", new[] { "ParentTicket_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.UserPermissions");
            DropTable("dbo.TicketTypes");
            DropTable("dbo.TicketLogs");
            DropTable("dbo.Comments");
            DropTable("dbo.Tickets");
            DropTable("dbo.Teams");
            DropTable("dbo.TeamRoles");
            DropTable("dbo.ApplicationRoles");
            DropTable("dbo.AdminLogs");
        }
    }
}
