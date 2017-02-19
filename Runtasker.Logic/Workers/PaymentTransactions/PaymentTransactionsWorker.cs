using Runtasker.Logic.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Runtasker.Logic.Workers.PaymentTransactions
{
    public class CustomerPaymentTransactionsWorker
    {
        #region Constructors
        public CustomerPaymentTransactionsWorker(MyDbContext context, string userGuid)
        {
            Construct(context, userGuid);
        }

        void Construct(MyDbContext context, string userGuid)
        {
            Context = context;
            UserGuid = userGuid;
        }
        #endregion

        #region Properties
        MyDbContext Context { get; set; }
        string UserGuid { get; set; }
        #endregion

        #region Public Methods
        public IEnumerable<PaymentTransaction> GetTransactions()
        {
            return Context.PaymentTransactions.Where(pt => pt.UserGuid == UserGuid).ToList();
        }
        #endregion

    }
}
