namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vkSpam2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.VkGroupMembers");
            AddColumn("dbo.VkGroupMembers", "VkGroupId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.VkGroupMembers", new[] { "VkManId", "VkGroupId" });
            DropColumn("dbo.VkGroupMembers", "VkGroupWithMembersId");
            DropTable("dbo.VkGroupWithMembers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.VkGroupWithMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Link = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.VkGroupMembers", "VkGroupWithMembersId", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.VkGroupMembers");
            DropColumn("dbo.VkGroupMembers", "VkGroupId");
            AddPrimaryKey("dbo.VkGroupMembers", new[] { "VkManId", "VkGroupWithMembersId" });
        }
    }
}
