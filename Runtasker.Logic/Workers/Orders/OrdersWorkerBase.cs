
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using System;
using System.Linq;

namespace Runtasker.Logic.Workers.Orders
{
    public class OrdersWorkerBase : IDisposable
    {
        #region Конструктор
        public OrdersWorkerBase(IMyDbContext context, string userGuid)
        {
            Context = context;
            UserGuid = userGuid;
            
        }

        
        #endregion

        #region Свойства
        protected string UserGuid { get; set; }

        protected IMyDbContext Context { get; set; }
        #endregion

        

        #region IDisposable

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
