namespace TaskDone_V2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rateFieldUpdated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Rate", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Rate", c => c.Single(nullable: false));
        }
    }
}
