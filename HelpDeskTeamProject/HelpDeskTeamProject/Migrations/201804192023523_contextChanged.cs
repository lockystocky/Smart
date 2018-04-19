namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contextChanged : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Team_Id", "dbo.Teams");
            DropIndex("dbo.Users", new[] { "Team_Id" });
            CreateTable(
                "dbo.UserTeams",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Team_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Team_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.Team_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Team_Id);
            
            DropColumn("dbo.Users", "Team_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Team_Id", c => c.Int());
            DropForeignKey("dbo.UserTeams", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.UserTeams", "User_Id", "dbo.Users");
            DropIndex("dbo.UserTeams", new[] { "Team_Id" });
            DropIndex("dbo.UserTeams", new[] { "User_Id" });
            DropTable("dbo.UserTeams");
            CreateIndex("dbo.Users", "Team_Id");
            AddForeignKey("dbo.Users", "Team_Id", "dbo.Teams", "Id");
        }
    }
}
