namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modelvalidationadded : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationRoles", "Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.TeamRoles", "Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Users", "Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Users", "Surname", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Tickets", "Description", c => c.String(nullable: false, maxLength: 400));
            AlterColumn("dbo.Comments", "Text", c => c.String(nullable: false, maxLength: 400));
            AlterColumn("dbo.TicketTypes", "Name", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TicketTypes", "Name", c => c.String(maxLength: 40));
            AlterColumn("dbo.Comments", "Text", c => c.String(maxLength: 400));
            AlterColumn("dbo.Tickets", "Description", c => c.String(maxLength: 400));
            AlterColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Users", "Surname", c => c.String());
            AlterColumn("dbo.Users", "Name", c => c.String());
            AlterColumn("dbo.TeamRoles", "Name", c => c.String(maxLength: 40));
            AlterColumn("dbo.ApplicationRoles", "Name", c => c.String(maxLength: 40));
        }
    }
}
