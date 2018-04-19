namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userAppIdAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "AppId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "AppId");
        }
    }
}
