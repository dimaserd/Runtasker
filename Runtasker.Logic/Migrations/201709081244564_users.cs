namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class users : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ShouldBeNotifictedInVk", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ShouldBeNotifictedInVk");
        }
    }
}
