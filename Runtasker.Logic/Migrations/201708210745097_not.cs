namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class not : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "CreationDate");
        }
    }
}
