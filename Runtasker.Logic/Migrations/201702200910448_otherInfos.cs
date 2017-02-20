namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class otherInfos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OtherUserInfoes", "VkDomain", c => c.String());
            AddColumn("dbo.OtherUserInfoes", "VkId", c => c.String());
            DropColumn("dbo.OtherUserInfoes", "VkLink");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OtherUserInfoes", "VkLink", c => c.String());
            DropColumn("dbo.OtherUserInfoes", "VkId");
            DropColumn("dbo.OtherUserInfoes", "VkDomain");
        }
    }
}
