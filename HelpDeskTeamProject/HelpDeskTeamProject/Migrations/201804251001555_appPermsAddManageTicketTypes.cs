namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appPermsAddManageTicketTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationRoles", "Permissions_CanManageTicketTypes", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationRoles", "Permissions_CanManageTicketTypes");
        }
    }
}
