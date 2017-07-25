using Newtonsoft.Json;
using Runtasker.Logic.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runtasker.Logic.Entities.News
{
    /// <summary>
    /// Сущность описывающая языковое уточнение по данной новости
    /// </summary>
    public class ArticleClarification : LanguageClarificationBase
    {
        /// <summary>
        /// Заголовок новости
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Текст новости
        /// </summary>
        public string Text { get; set; }

        #region Свойства отношений

        [Required]
        [StringLength(128)]
        [ForeignKey("FromArticle")]
        public string ArticleId { get; set; }

        [JsonIgnore]
        public virtual Article FromArticle { get; set; }

        #endregion
    }
}
