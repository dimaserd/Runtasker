
using Runtasker.Logic.Entities;
using System;
using System.Linq;

namespace Runtasker.Logic.Workers.Orders
{
    public class OrdersWorkerBase : IDisposable
    {
        #region Contructors
        public OrdersWorkerBase(string userGuid)
        {
            Construct(userGuid);
        }

        void Construct(string userGuid)
        {
            UserGuid = userGuid;
        }
        #endregion

        #region Protected Properties
        protected string UserGuid { get; set; }
        #endregion

        #region Protected Methods
        public Order GetOrder(int id)
        {
            using (MyDbContext context = new MyDbContext())
            {
                return context.Orders.FirstOrDefault(o => o.Id == id && o.UserGuid == UserGuid);
            }
        }



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
