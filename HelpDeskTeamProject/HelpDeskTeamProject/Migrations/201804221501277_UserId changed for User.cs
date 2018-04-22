namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdchangedforUser : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserTeams", newName: "TeamUsers");
            DropPrimaryKey("dbo.TeamUsers");
            AddColumn("dbo.UserPermissions", "User_Id", c => c.Int());
            AddPrimaryKey("dbo.TeamUsers", new[] { "Team_Id", "User_Id" });
            CreateIndex("dbo.UserPermissions", "User_Id");
            AddForeignKey("dbo.UserPermissions", "User_Id", "dbo.Users", "Id");
            DropColumn("dbo.UserPermissions", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserPermissions", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserPermissions", "User_Id", "dbo.Users");
            DropIndex("dbo.UserPermissions", new[] { "User_Id" });
            DropPrimaryKey("dbo.TeamUsers");
            DropColumn("dbo.UserPermissions", "User_Id");
            AddPrimaryKey("dbo.TeamUsers", new[] { "User_Id", "Team_Id" });
            RenameTable(name: "dbo.TeamUsers", newName: "UserTeams");
        }
    }
}
