using Runtaker.LocaleBuiders.Entities;
using Runtaker.LocaleBuiders.Interfaces;
using System.Data.Entity;

namespace Runtaker.LocaleBuiders.Contexts
{
    public class ResourceContext : IResourceContext
    {
        public DbSet<ResourceFileModel> ResourceFileModels { get; set; }

        public DbSet<ResourceString> ResourceStrings { get; set; }
    }
}
