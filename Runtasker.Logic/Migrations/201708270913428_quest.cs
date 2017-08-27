namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class quest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionAnswerLangClarifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                        Answer = c.String(),
                        LanguageCode = c.Int(nullable: false),
                        QuestionAnswer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionAnswers", t => t.QuestionAnswer_Id)
                .Index(t => t.QuestionAnswer_Id);
            
            CreateTable(
                "dbo.QuestionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionAnswerLangClarifications", "QuestionAnswer_Id", "dbo.QuestionAnswers");
            DropIndex("dbo.QuestionAnswerLangClarifications", new[] { "QuestionAnswer_Id" });
            DropTable("dbo.QuestionAnswers");
            DropTable("dbo.QuestionAnswerLangClarifications");
        }
    }
}
