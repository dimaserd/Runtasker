namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ret : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coupons",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Count = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        BonusMoney = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserCoupons",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        CouponId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.CouponId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .ForeignKey("dbo.Coupons", t => t.CouponId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CouponId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCoupons", "CouponId", "dbo.Coupons");
            DropForeignKey("dbo.UserCoupons", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserCoupons", new[] { "CouponId" });
            DropIndex("dbo.UserCoupons", new[] { "UserId" });
            DropTable("dbo.UserCoupons");
            DropTable("dbo.Coupons");
        }
    }
}
