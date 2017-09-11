namespace Art.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adasd1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "WhenAdded", c => c.DateTime());
            AlterColumn("dbo.Categories", "WhenModified", c => c.DateTime());
            AlterColumn("dbo.Categories", "WhenDeleted", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "WhenDeleted", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Categories", "WhenModified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Categories", "WhenAdded", c => c.DateTime(nullable: false));
        }
    }
}
