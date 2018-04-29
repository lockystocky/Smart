namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class logsUserAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdminLogs", "User_Id", c => c.Int());
            AddColumn("dbo.TicketLogs", "User_Id", c => c.Int());
            CreateIndex("dbo.AdminLogs", "User_Id");
            CreateIndex("dbo.TicketLogs", "User_Id");
            AddForeignKey("dbo.TicketLogs", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.AdminLogs", "User_Id", "dbo.Users", "Id");
            DropColumn("dbo.AdminLogs", "UserId");
            DropColumn("dbo.TicketLogs", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TicketLogs", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.AdminLogs", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.AdminLogs", "User_Id", "dbo.Users");
            DropForeignKey("dbo.TicketLogs", "User_Id", "dbo.Users");
            DropIndex("dbo.TicketLogs", new[] { "User_Id" });
            DropIndex("dbo.AdminLogs", new[] { "User_Id" });
            DropColumn("dbo.TicketLogs", "User_Id");
            DropColumn("dbo.AdminLogs", "User_Id");
        }
    }
}
