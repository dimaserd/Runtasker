namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentsExt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "Hash", c => c.String());
            AddColumn("dbo.Payments", "PaymentServiceId", c => c.String());
            AddColumn("dbo.Payments", "Confirmed", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "Confirmed");
            DropColumn("dbo.Payments", "PaymentServiceId");
            DropColumn("dbo.Payments", "Hash");
        }
    }
}
