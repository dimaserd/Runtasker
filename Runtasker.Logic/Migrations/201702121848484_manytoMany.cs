namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manytoMany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VkKeyWordFoundPosts",
                c => new
                    {
                        VkKeyWordId = c.Int(nullable: false),
                        VkFoundPostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VkKeyWordId, t.VkFoundPostId })
                .ForeignKey("dbo.VkKeyWords", t => t.VkKeyWordId, cascadeDelete: true)
                .ForeignKey("dbo.VkFoundPosts", t => t.VkFoundPostId, cascadeDelete: true)
                .Index(t => t.VkKeyWordId)
                .Index(t => t.VkFoundPostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VkKeyWordFoundPosts", "VkFoundPostId", "dbo.VkFoundPosts");
            DropForeignKey("dbo.VkKeyWordFoundPosts", "VkKeyWordId", "dbo.VkKeyWords");
            DropIndex("dbo.VkKeyWordFoundPosts", new[] { "VkFoundPostId" });
            DropIndex("dbo.VkKeyWordFoundPosts", new[] { "VkKeyWordId" });
            DropTable("dbo.VkKeyWordFoundPosts");
        }
    }
}
