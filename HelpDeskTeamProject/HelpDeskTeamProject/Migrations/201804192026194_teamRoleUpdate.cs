namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teamRoleUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserPermissions", "TeamRole_Id", c => c.Int());
            CreateIndex("dbo.UserPermissions", "TeamRole_Id");
            AddForeignKey("dbo.UserPermissions", "TeamRole_Id", "dbo.TeamRoles", "Id");
            DropColumn("dbo.UserPermissions", "TeamRoleId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserPermissions", "TeamRoleId", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserPermissions", "TeamRole_Id", "dbo.TeamRoles");
            DropIndex("dbo.UserPermissions", new[] { "TeamRole_Id" });
            DropColumn("dbo.UserPermissions", "TeamRole_Id");
        }
    }
}
