namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teamIdInTicket : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tickets", "Team_Id", "dbo.Teams");
            DropIndex("dbo.Tickets", new[] { "Team_Id" });
            RenameColumn(table: "dbo.Tickets", name: "Team_Id", newName: "TeamId");
            AlterColumn("dbo.Tickets", "TeamId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tickets", "TeamId");
            AddForeignKey("dbo.Tickets", "TeamId", "dbo.Teams", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "TeamId", "dbo.Teams");
            DropIndex("dbo.Tickets", new[] { "TeamId" });
            AlterColumn("dbo.Tickets", "TeamId", c => c.Int());
            RenameColumn(table: "dbo.Tickets", name: "TeamId", newName: "Team_Id");
            CreateIndex("dbo.Tickets", "Team_Id");
            AddForeignKey("dbo.Tickets", "Team_Id", "dbo.Teams", "Id");
        }
    }
}
