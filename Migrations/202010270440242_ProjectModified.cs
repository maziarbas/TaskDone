namespace TaskDone_V2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectModified : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Offers", "OffererId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Offers", new[] { "OffererId" });
            DropIndex("dbo.Projects", new[] { "ApplicationUserId" });
            DropColumn("dbo.Offers", "OffererId");
            DropColumn("dbo.Projects", "ApplicationUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "ApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Offers", "OffererId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Projects", "ApplicationUserId");
            CreateIndex("dbo.Offers", "OffererId");
            AddForeignKey("dbo.Projects", "ApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Offers", "OffererId", "dbo.AspNetUsers", "Id");
        }
    }
}
