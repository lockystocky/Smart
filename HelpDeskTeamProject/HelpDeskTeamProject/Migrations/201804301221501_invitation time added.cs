namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invitationtimeadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvitedUsers", "TimeOfLastInvitation", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Teams", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Teams", "Name", c => c.String(maxLength: 30));
            DropColumn("dbo.InvitedUsers", "TimeOfLastInvitation");
        }
    }
}
