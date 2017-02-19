using Runtasker.Logic.Entities;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Runtasker.Logic.Contexts.Interfaces
{
    public interface IApplicationDbContext
    {
        
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

}
