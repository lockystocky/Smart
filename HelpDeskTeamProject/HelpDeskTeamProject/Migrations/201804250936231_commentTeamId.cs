namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentTeamId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "TeamId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "TeamId");
        }
    }
}
