namespace Art.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdsad : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        IdImages = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        IdWork = c.Int(),
                        IdArticle = c.Int(),
                        IdCategory = c.Int(),
                    })
                .PrimaryKey(t => t.IdImages)
                .ForeignKey("dbo.Articles", t => t.IdArticle)
                .ForeignKey("dbo.Categories", t => t.IdCategory)
                .ForeignKey("dbo.Works", t => t.IdWork)
                .Index(t => t.IdWork)
                .Index(t => t.IdArticle)
                .Index(t => t.IdCategory);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        IdArticle = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.IdArticle);
            
            CreateTable(
                "dbo.Works",
                c => new
                    {
                        IdWork = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Place = c.String(),
                    })
                .PrimaryKey(t => t.IdWork);
            
            AddColumn("dbo.Categories", "IdWork", c => c.Int());
            AddColumn("dbo.Clients", "Url", c => c.String());
            CreateIndex("dbo.Categories", "IdWork");
            AddForeignKey("dbo.Categories", "IdWork", "dbo.Works", "IdWork");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "IdWork", "dbo.Works");
            DropForeignKey("dbo.Categories", "IdWork", "dbo.Works");
            DropForeignKey("dbo.Images", "IdCategory", "dbo.Categories");
            DropForeignKey("dbo.Images", "IdArticle", "dbo.Articles");
            DropIndex("dbo.Images", new[] { "IdCategory" });
            DropIndex("dbo.Images", new[] { "IdArticle" });
            DropIndex("dbo.Images", new[] { "IdWork" });
            DropIndex("dbo.Categories", new[] { "IdWork" });
            DropColumn("dbo.Clients", "Url");
            DropColumn("dbo.Categories", "IdWork");
            DropTable("dbo.Works");
            DropTable("dbo.Articles");
            DropTable("dbo.Images");
        }
    }
}
