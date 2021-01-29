namespace TaskDone_V2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserEmailAddedToProjectAndOffer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Offers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Offers", new[] { "UserId" });
            AddColumn("dbo.Offers", "UserEmail", c => c.String());
            AddColumn("dbo.Projects", "UserEmail", c => c.String());
            DropColumn("dbo.Offers", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Offers", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.Projects", "UserEmail");
            DropColumn("dbo.Offers", "UserEmail");
            CreateIndex("dbo.Offers", "UserId");
            AddForeignKey("dbo.Offers", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
