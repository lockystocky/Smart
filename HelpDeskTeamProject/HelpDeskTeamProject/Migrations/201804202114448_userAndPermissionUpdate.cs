namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userAndPermissionUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamRoles", "Permissions_CanChangeTicketState", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tickets", "State", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Name", c => c.String());
            AddColumn("dbo.Users", "Surname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Surname");
            DropColumn("dbo.Users", "Name");
            DropColumn("dbo.Tickets", "State");
            DropColumn("dbo.TeamRoles", "Permissions_CanChangeTicketState");
        }
    }
}
