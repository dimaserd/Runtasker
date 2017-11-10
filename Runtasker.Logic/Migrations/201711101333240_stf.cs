namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stf : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE [dbo].[VkGroupMembers] DROP CONSTRAINT [FK_dbo.VkGroupMembers_dbo.VkMen_Man_Id];");
            Sql("ALTER TABLE [dbo].[VkGroupMembers] DROP CONSTRAINT [FK_dbo.VkGroupMembers_dbo.VkGroups_Group_Id];");
            DropForeignKey("dbo.VkGroupMembers", "Man_Id", "dbo.VkMen");
            DropIndex("dbo.VkGroupMembers", new[] { "Man_Id" });
            DropColumn("dbo.VkGroupMembers", "VkManId");
            RenameColumn(table: "dbo.VkGroupMembers", name: "Man_Id", newName: "VkManId");
            DropPrimaryKey("dbo.VkGroupMembers");
            DropPrimaryKey("dbo.VkMen");
            AddColumn("dbo.VkMen", "VkId", c => c.Int(nullable: false));
            AlterColumn("dbo.VkGroupMembers", "VkManId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.VkMen", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.VkGroupMembers", new[] { "VkManId", "VkGroupId" });
            AddPrimaryKey("dbo.VkMen", "Id");
            CreateIndex("dbo.VkGroupMembers", "VkManId");
            AddForeignKey("dbo.VkGroupMembers", "VkManId", "dbo.VkMen", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VkGroupMembers", "VkManId", "dbo.VkMen");
            DropIndex("dbo.VkGroupMembers", new[] { "VkManId" });
            DropPrimaryKey("dbo.VkMen");
            DropPrimaryKey("dbo.VkGroupMembers");
            AlterColumn("dbo.VkMen", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.VkGroupMembers", "VkManId", c => c.Int());
            DropColumn("dbo.VkMen", "VkId");
            AddPrimaryKey("dbo.VkMen", "Id");
            AddPrimaryKey("dbo.VkGroupMembers", new[] { "VkManId", "VkGroupId" });
            RenameColumn(table: "dbo.VkGroupMembers", name: "VkManId", newName: "Man_Id");
            AddColumn("dbo.VkGroupMembers", "VkManId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.VkGroupMembers", "Man_Id");
            AddForeignKey("dbo.VkGroupMembers", "Man_Id", "dbo.VkMen", "Id");
        }
    }
}
