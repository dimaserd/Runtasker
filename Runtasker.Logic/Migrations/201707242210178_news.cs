namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class news : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleClarifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(),
                        Text = c.String(),
                        ArticleId = c.String(nullable: false, maxLength: 128),
                        LanguageCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ToShow = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ImageCaptionHtml = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArticleClarifications", "ArticleId", "dbo.Articles");
            DropIndex("dbo.ArticleClarifications", new[] { "ArticleId" });
            DropTable("dbo.Articles");
            DropTable("dbo.ArticleClarifications");
        }
    }
}
