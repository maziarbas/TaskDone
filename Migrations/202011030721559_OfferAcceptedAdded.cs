namespace TaskDone_V2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfferAcceptedAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "Accepted", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "Accepted");
        }
    }
}
