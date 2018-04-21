namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userIdToUserClassChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "Description", c => c.String());
            AddColumn("dbo.Tickets", "User_Id", c => c.Int());
            AddColumn("dbo.Comments", "User_Id", c => c.Int());
            CreateIndex("dbo.Tickets", "User_Id");
            CreateIndex("dbo.Comments", "User_Id");
            AddForeignKey("dbo.Comments", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Tickets", "User_Id", "dbo.Users", "Id");
            DropColumn("dbo.Tickets", "UserId");
            DropColumn("dbo.Comments", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Tickets", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Tickets", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Comments", "User_Id", "dbo.Users");
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropIndex("dbo.Tickets", new[] { "User_Id" });
            DropColumn("dbo.Comments", "User_Id");
            DropColumn("dbo.Tickets", "User_Id");
            DropColumn("dbo.Tickets", "Description");
        }
    }
}
