namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionAnswerLangClarifications", "Show", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionAnswerLangClarifications", "Show");
        }
    }
}
