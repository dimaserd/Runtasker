using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtasker.Logic.Entities.News
{
    /// <summary>
    /// Сущность описывающая новость
    /// </summary>
    public class Article
    {
        public Article()
        {
            Clarifications = new List<ArticleClarification>();
        }

        #region Свойства
        /// <summary>
        /// Идентификатор новости
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Флаг указывающий на то, стоит ли выводить данную новость
        /// </summary>
        public bool ToShow { get; set; }

        /// <summary>
        /// Дата публикации новости
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Html код картинки (Не рекомендуется использовать в дальнейшем, нужно придумать что-то более умное)
        /// </summary>
        public string ImageCaptionHtml { get; set; }

        /// <summary>
        /// Языковые уточнения (список сущностей содержащие локализованные Свойства новости)
        /// </summary>
        public ICollection<ArticleClarification> Clarifications { get; set; }
        #endregion
    }
}
