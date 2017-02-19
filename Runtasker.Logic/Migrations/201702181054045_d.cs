namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class d : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OtherUserInfoes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        VkLink = c.String(),
                        Specialization = c.String(),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            DropColumn("dbo.AspNetUsers", "Specialization");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Specialization", c => c.String());
            DropForeignKey("dbo.OtherUserInfoes", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.OtherUserInfoes", new[] { "UserId" });
            DropTable("dbo.OtherUserInfoes");
        }
    }
}
