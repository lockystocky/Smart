namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stringlengthlimitset : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationRoles", "Name", c => c.String(maxLength: 40));
            AlterColumn("dbo.TeamRoles", "Name", c => c.String(maxLength: 40));
            AlterColumn("dbo.Teams", "Name", c => c.String(maxLength: 30));
            AlterColumn("dbo.Tickets", "Description", c => c.String(maxLength: 400));
            AlterColumn("dbo.Comments", "Text", c => c.String(maxLength: 400));
            AlterColumn("dbo.TicketTypes", "Name", c => c.String(maxLength: 40));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TicketTypes", "Name", c => c.String());
            AlterColumn("dbo.Comments", "Text", c => c.String());
            AlterColumn("dbo.Tickets", "Description", c => c.String());
            AlterColumn("dbo.Teams", "Name", c => c.String());
            AlterColumn("dbo.TeamRoles", "Name", c => c.String());
            AlterColumn("dbo.ApplicationRoles", "Name", c => c.String());
        }
    }
}
