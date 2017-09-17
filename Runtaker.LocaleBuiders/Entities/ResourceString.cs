using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runtaker.LocaleBuiders.Entities
{
    public class ResourceString
    {
        [Key]
        public string Id { get; set; }

        public string ResourceKey { get; set; }

        public string ResourceValue { get; set; }

        public DateTime LastEditedDate { get; set; }

        #region Свойства отношений
        [ForeignKey("ResourceFile")]
        public string ResourceFileId { get; set; }

        public virtual ResourceFileModel ResourceFile { get; set; }
        #endregion
    }
}
