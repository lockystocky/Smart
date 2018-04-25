namespace HelpDeskTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invitationmodelchanged : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InvitationEmails", "Team_Id", "dbo.Teams");
            DropIndex("dbo.InvitationEmails", new[] { "Team_Id" });
            CreateTable(
                "dbo.InvitedUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Code = c.Int(nullable: false),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.Team_Id);
            
            DropTable("dbo.InvitationEmails");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.InvitationEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.InvitedUsers", "Team_Id", "dbo.Teams");
            DropIndex("dbo.InvitedUsers", new[] { "Team_Id" });
            DropTable("dbo.InvitedUsers");
            CreateIndex("dbo.InvitationEmails", "Team_Id");
            AddForeignKey("dbo.InvitationEmails", "Team_Id", "dbo.Teams", "Id");
        }
    }
}
