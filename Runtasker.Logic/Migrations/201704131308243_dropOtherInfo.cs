namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropOtherInfo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OtherUserInfoes", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.OtherUserInfoes", new[] { "Id" });
            AddColumn("dbo.AspNetUsers", "VkDomain", c => c.String());
            AddColumn("dbo.AspNetUsers", "VkId", c => c.String());
            AddColumn("dbo.AspNetUsers", "Specialization", c => c.String());
            DropTable("dbo.OtherUserInfoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OtherUserInfoes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        VkDomain = c.String(),
                        VkId = c.String(),
                        Specialization = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.AspNetUsers", "Specialization");
            DropColumn("dbo.AspNetUsers", "VkId");
            DropColumn("dbo.AspNetUsers", "VkDomain");
            CreateIndex("dbo.OtherUserInfoes", "Id");
            AddForeignKey("dbo.OtherUserInfoes", "Id", "dbo.AspNetUsers", "Id");
        }
    }
}
