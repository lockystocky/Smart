namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invitationmodelemailchanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InvitedUsers", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InvitedUsers", "Email", c => c.String());
        }
    }
}
