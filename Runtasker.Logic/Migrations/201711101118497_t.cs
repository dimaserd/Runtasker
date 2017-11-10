namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VkMen", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VkMen", "Name");
        }
    }
}
