namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tickets", "Team_Id", "dbo.Teams");
            DropIndex("dbo.Tickets", new[] { "Team_Id" });
            RenameColumn(table: "dbo.Tickets", name: "Team_Id", newName: "TeamId");
            AddColumn("dbo.Tickets", "User_Id", c => c.Int());
            AddColumn("dbo.Comments", "User_Id", c => c.Int());
            AlterColumn("dbo.Tickets", "TeamId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tickets", "TeamId");
            CreateIndex("dbo.Tickets", "User_Id");
            CreateIndex("dbo.Comments", "User_Id");
            AddForeignKey("dbo.Comments", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Tickets", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Tickets", "TeamId", "dbo.Teams", "Id", cascadeDelete: true);
            DropColumn("dbo.Tickets", "UserId");
            DropColumn("dbo.Comments", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Tickets", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Tickets", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Tickets", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Comments", "User_Id", "dbo.Users");
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropIndex("dbo.Tickets", new[] { "User_Id" });
            DropIndex("dbo.Tickets", new[] { "TeamId" });
            AlterColumn("dbo.Tickets", "TeamId", c => c.Int());
            DropColumn("dbo.Comments", "User_Id");
            DropColumn("dbo.Tickets", "User_Id");
            RenameColumn(table: "dbo.Tickets", name: "TeamId", newName: "Team_Id");
            CreateIndex("dbo.Tickets", "Team_Id");
            AddForeignKey("dbo.Tickets", "Team_Id", "dbo.Teams", "Id");
        }
    }
}
