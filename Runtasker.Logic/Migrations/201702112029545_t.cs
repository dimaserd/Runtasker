namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VkPostLookUps", "VkPerformerGuid", "dbo.AspNetUsers");
            DropIndex("dbo.VkPostLookUps", new[] { "VkPerformerGuid" });
            AlterColumn("dbo.VkPostLookUps", "VkPerformerGuid", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VkPostLookUps", "VkPerformerGuid", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.VkPostLookUps", "VkPerformerGuid");
            AddForeignKey("dbo.VkPostLookUps", "VkPerformerGuid", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
