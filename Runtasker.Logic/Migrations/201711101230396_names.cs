namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class names : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VkMen", "FirstName", c => c.String());
            AddColumn("dbo.VkMen", "LastName", c => c.String());
            DropColumn("dbo.VkMen", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VkMen", "Name", c => c.String());
            DropColumn("dbo.VkMen", "LastName");
            DropColumn("dbo.VkMen", "FirstName");
        }
    }
}
