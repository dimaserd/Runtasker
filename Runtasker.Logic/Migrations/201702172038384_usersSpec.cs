namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usersSpec : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Specialization", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Specialization");
        }
    }
}
