using System;
using System.Collections.Generic;

namespace Logic.Extensions.Repositories.Interfaces
{
    public interface IRepositoryExtensive<T> : IDisposable
        where T : class
    {
        void AddRange(IEnumerable<T> items);

        void RemoveRange(IEnumerable<T> items);
    }
}
