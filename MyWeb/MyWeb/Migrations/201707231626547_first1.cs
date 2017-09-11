namespace MyWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        IdCategory = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IdTatoo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCategory)
                .ForeignKey("dbo.Tattoos", t => t.IdTatoo, cascadeDelete: true)
                .Index(t => t.IdTatoo);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        IdImage = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        IdCategory = c.Int(nullable: false),
                        IdTattooStyles = c.Int(nullable: false),
                        IdIllustator = c.Int(nullable: false),
                        IdTatoo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdImage)
                .ForeignKey("dbo.Categories", t => t.IdCategory, cascadeDelete: true)
                .ForeignKey("dbo.Illustrators", t => t.IdIllustator, cascadeDelete: true)
                .ForeignKey("dbo.Tattoos", t => t.IdTatoo, cascadeDelete: true)
                .ForeignKey("dbo.TattooStyles", t => t.IdTattooStyles, cascadeDelete: true)
                .Index(t => t.IdCategory)
                .Index(t => t.IdTattooStyles)
                .Index(t => t.IdIllustator)
                .Index(t => t.IdTatoo);
            
            CreateTable(
                "dbo.Illustrators",
                c => new
                    {
                        IdIllustator = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Sername = c.String(),
                        Email = c.String(),
                        IdTatoo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdIllustator)
                .ForeignKey("dbo.Tattoos", t => t.IdTatoo, cascadeDelete: true)
                .Index(t => t.IdTatoo);
            
            CreateTable(
                "dbo.Tattoos",
                c => new
                    {
                        IdTatoo = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Likes = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IdTatoo);
            
            CreateTable(
                "dbo.TattooStyles",
                c => new
                    {
                        IdTattooStyles = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IdTatoo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdTattooStyles)
                .ForeignKey("dbo.Tattoos", t => t.IdTatoo, cascadeDelete: true)
                .Index(t => t.IdTatoo);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TattooStyles", "IdTatoo", "dbo.Tattoos");
            DropForeignKey("dbo.Images", "IdTattooStyles", "dbo.TattooStyles");
            DropForeignKey("dbo.Images", "IdTatoo", "dbo.Tattoos");
            DropForeignKey("dbo.Illustrators", "IdTatoo", "dbo.Tattoos");
            DropForeignKey("dbo.Categories", "IdTatoo", "dbo.Tattoos");
            DropForeignKey("dbo.Images", "IdIllustator", "dbo.Illustrators");
            DropForeignKey("dbo.Images", "IdCategory", "dbo.Categories");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.TattooStyles", new[] { "IdTatoo" });
            DropIndex("dbo.Illustrators", new[] { "IdTatoo" });
            DropIndex("dbo.Images", new[] { "IdTatoo" });
            DropIndex("dbo.Images", new[] { "IdIllustator" });
            DropIndex("dbo.Images", new[] { "IdTattooStyles" });
            DropIndex("dbo.Images", new[] { "IdCategory" });
            DropIndex("dbo.Categories", new[] { "IdTatoo" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.TattooStyles");
            DropTable("dbo.Tattoos");
            DropTable("dbo.Illustrators");
            DropTable("dbo.Images");
            DropTable("dbo.Categories");
        }
    }
}
