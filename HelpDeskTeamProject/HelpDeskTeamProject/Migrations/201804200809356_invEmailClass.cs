namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invEmailClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvitationEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.Team_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvitationEmails", "Team_Id", "dbo.Teams");
            DropIndex("dbo.InvitationEmails", new[] { "Team_Id" });
            DropTable("dbo.InvitationEmails");
        }
    }
}
