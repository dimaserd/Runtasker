namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "LastStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "LastStatus");
        }
    }
}
