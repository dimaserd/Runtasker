using Runtaker.LocaleBuiders.Entities;
using System.Data.Entity;

namespace Runtaker.LocaleBuiders.Interfaces
{
    public interface IResourceContext
    {
        DbSet<ResourceFileModel> ResourceFileModels { get; }

        DbSet<ResourceString> ResourceStrings { get; }

        DbSet<ResourceStringType> ResourceStringTypes { get; }
    }
}
