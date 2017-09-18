using System;
using System.Collections.Generic;

namespace Runtaker.LocaleBuiders.Entities
{
    public class ResourceFileModel
    {
        public string Id { get; set; }

        public string ResourcePath { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual ICollection<ResourceString> ResourceStrings { get; set; }
    }
}
