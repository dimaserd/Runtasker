
using Runtasker.Logic.Entities;
using System;
using System.Linq;

namespace Runtasker.Logic.Workers.Orders
{
    public class OrdersWorkerBase : IDisposable
    {
        #region Конструктор
        public OrdersWorkerBase(MyDbContext context, string userGuid)
        {
            Context = context;
            UserGuid = userGuid;
            
        }

        void Construct(string userGuid)
        {
            
        }
        #endregion

        #region Свойства
        protected string UserGuid { get; set; }

        protected MyDbContext Context { get; set; }
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
