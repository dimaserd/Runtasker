using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runtaker.LocaleBuiders.Entities
{
    public class ResourceString
    {
        public ResourceString()
        {
            ResourceStringTypes = new List<ResourceStringType>();
        }

        #region Свойства

        [Key]
        public string Id { get; set; }

        public string ResourceKey { get; set; }

        /// <summary>
        /// Какое то значение по умолчанию
        /// </summary>
        public string ResourceValue { get; set; }

        public DateTime LastEditedDate { get; set; }

        #region Свойства отношений
        [ForeignKey("ResourceFile")]
        public string ResourceFileId { get; set; }

        public virtual ResourceFileModel ResourceFile { get; set; }

        public virtual ICollection<ResourceStringType> ResourceStringTypes { get; set; }
        #endregion

        #endregion
    }
}
