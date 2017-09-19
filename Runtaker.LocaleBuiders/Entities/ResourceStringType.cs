using Runtasker.LocaleBuilders.Enumerations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runtaker.LocaleBuiders.Entities
{
    public class ResourceStringType
    {
        public string Id { get; set; }

        public string ResourceValue { get; set; }

        public Lang LangCode { get; set; }


        #region Свойства отношений
        [ForeignKey("ResourceString")]
        public string ResourceStringId { get; set; }

        public virtual ResourceString ResourceString { get; set; }
        #endregion
    }
}
