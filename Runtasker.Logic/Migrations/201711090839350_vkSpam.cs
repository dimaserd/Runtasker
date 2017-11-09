namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vkSpam : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VkGroupMembers",
                c => new
                    {
                        VkManId = c.String(nullable: false, maxLength: 128),
                        VkGroupWithMembersId = c.String(nullable: false, maxLength: 128),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.VkManId, t.VkGroupWithMembersId })
                .ForeignKey("dbo.VkGroupWithMembers", t => t.Group_Id)
                .ForeignKey("dbo.VkMen", t => t.VkManId, cascadeDelete: true)
                .Index(t => t.VkManId)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.VkGroupWithMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Link = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VkMen",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        VkLink = c.String(),
                        IsInformed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VkGroupMembers", "VkManId", "dbo.VkMen");
            DropForeignKey("dbo.VkGroupMembers", "Group_Id", "dbo.VkGroupWithMembers");
            DropIndex("dbo.VkGroupMembers", new[] { "Group_Id" });
            DropIndex("dbo.VkGroupMembers", new[] { "VkManId" });
            DropTable("dbo.VkMen");
            DropTable("dbo.VkGroupWithMembers");
            DropTable("dbo.VkGroupMembers");
        }
    }
}
