namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedAttributes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationRoles", "Name", c => c.String());
            AlterColumn("dbo.Users", "Name", c => c.String());
            AlterColumn("dbo.Users", "Surname", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.TeamRoles", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TeamRoles", "Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Surname", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Users", "Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.ApplicationRoles", "Name", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
