namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResourceFileModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ResourcePath = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        LangCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ResourceStrings",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ResourceKey = c.String(),
                        ResourceValue = c.String(),
                        LastEditedDate = c.DateTime(nullable: false),
                        ResourceFileId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ResourceFileModels", t => t.ResourceFileId)
                .Index(t => t.ResourceFileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResourceStrings", "ResourceFileId", "dbo.ResourceFileModels");
            DropIndex("dbo.ResourceStrings", new[] { "ResourceFileId" });
            DropTable("dbo.ResourceStrings");
            DropTable("dbo.ResourceFileModels");
        }
    }
}
