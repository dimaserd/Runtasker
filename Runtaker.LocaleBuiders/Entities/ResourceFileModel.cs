using Runtasker.LocaleBuilders.Enumerations;
using System;
using System.Collections.Generic;

namespace Runtaker.LocaleBuiders.Entities
{
    public class ResourceFileModel
    {
        public ResourceFileModel()
        {
            ResourceStrings = new List<ResourceString>();
        }

        public string Id { get; set; }

        public string ResourcePath { get; set; }

        public DateTime CreateDate { get; set; }

        public Lang LangCode { get; set; }

        public virtual ICollection<ResourceString> ResourceStrings { get; set; }
    }
}
