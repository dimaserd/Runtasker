namespace Runtasker.Logic.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class visible : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionAnswerLangClarifications", "IsVisible", c => c.Boolean(nullable: false));
            DropColumn("dbo.QuestionAnswerLangClarifications", "Show");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestionAnswerLangClarifications", "Show", c => c.Boolean(nullable: false));
            DropColumn("dbo.QuestionAnswerLangClarifications", "IsVisible");
        }
    }
}
