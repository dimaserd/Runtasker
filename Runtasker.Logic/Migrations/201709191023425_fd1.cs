namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fd1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResourceStringTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ResourceValue = c.String(),
                        LangCode = c.Int(nullable: false),
                        ResourceStringId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ResourceStrings", t => t.ResourceStringId)
                .Index(t => t.ResourceStringId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResourceStringTypes", "ResourceStringId", "dbo.ResourceStrings");
            DropIndex("dbo.ResourceStringTypes", new[] { "ResourceStringId" });
            DropTable("dbo.ResourceStringTypes");
        }
    }
}
