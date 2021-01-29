namespace TaskDone_V2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectFeildsUpdated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "UserEmail", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "UserEmail", c => c.String());
        }
    }
}
