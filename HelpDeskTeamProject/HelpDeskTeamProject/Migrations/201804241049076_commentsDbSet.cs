namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentsDbSet : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserTeams", newName: "TeamUsers");
            DropPrimaryKey("dbo.TeamUsers");
            AddPrimaryKey("dbo.TeamUsers", new[] { "Team_Id", "User_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TeamUsers");
            AddPrimaryKey("dbo.TeamUsers", new[] { "User_Id", "Team_Id" });
            RenameTable(name: "dbo.TeamUsers", newName: "UserTeams");
        }
    }
}
