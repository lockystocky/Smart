namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedMoreValidations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TicketTypes", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TicketTypes", "Name", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
