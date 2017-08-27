using Runtasker.Logic.Entities.Base;
using System.Collections.Generic;

namespace Runtasker.Logic.Entities
{
    public class QuestionAnswer
    {
        #region Конструкторы
        public QuestionAnswer()
        {
            Clarifications = new List<QuestionAnswerLangClarification>();
        }
        #endregion

        public int Id { get; set; }

        public ICollection<QuestionAnswerLangClarification> Clarifications { get; set; }
    }

    public class QuestionAnswerLangClarification : LanguageClarificationBase
    {
        public string Question { get; set; }

        public string Answer { get; set; }

        public bool IsVisible { get; set; }
    }
}
