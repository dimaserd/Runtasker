namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vkd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VkGroupMembers", "VkManId", "dbo.VkMen");
            DropIndex("dbo.VkGroupMembers", new[] { "VkManId" });
            DropPrimaryKey("dbo.VkMen");
            AddColumn("dbo.VkGroupMembers", "Man_Id", c => c.Int());
            AlterColumn("dbo.VkMen", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.VkMen", "Id");
            CreateIndex("dbo.VkGroupMembers", "Man_Id");
            AddForeignKey("dbo.VkGroupMembers", "Man_Id", "dbo.VkMen", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VkGroupMembers", "Man_Id", "dbo.VkMen");
            DropIndex("dbo.VkGroupMembers", new[] { "Man_Id" });
            DropPrimaryKey("dbo.VkMen");
            AlterColumn("dbo.VkMen", "Id", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.VkGroupMembers", "Man_Id");
            AddPrimaryKey("dbo.VkMen", "Id");
            CreateIndex("dbo.VkGroupMembers", "VkManId");
            AddForeignKey("dbo.VkGroupMembers", "VkManId", "dbo.VkMen", "Id", cascadeDelete: true);
        }
    }
}
