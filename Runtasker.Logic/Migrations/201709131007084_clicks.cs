namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clicks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clicks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PreviousUrl = c.String(),
                        Info = c.String(),
                        ClickDate = c.DateTime(nullable: false),
                        CountingClickLinkId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CountingClickLinks", t => t.CountingClickLinkId)
                .Index(t => t.CountingClickLinkId);
            
            CreateTable(
                "dbo.CountingClickLinks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ClickName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clicks", "CountingClickLinkId", "dbo.CountingClickLinks");
            DropIndex("dbo.Clicks", new[] { "CountingClickLinkId" });
            DropTable("dbo.CountingClickLinks");
            DropTable("dbo.Clicks");
        }
    }
}
