namespace TaskDone_V2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfferUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Offers", "UserId");
            AddForeignKey("dbo.Offers", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Offers", new[] { "UserId" });
            DropColumn("dbo.Offers", "UserId");
        }
    }
}
