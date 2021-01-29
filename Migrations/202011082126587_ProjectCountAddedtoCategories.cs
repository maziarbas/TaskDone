namespace TaskDone_V2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectCountAddedtoCategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ProjectCount", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "ProjectCount");
        }
    }
}
