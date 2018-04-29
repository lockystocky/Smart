namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentsTicketIdAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "BaseTicketId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "BaseTicketId");
        }
    }
}
