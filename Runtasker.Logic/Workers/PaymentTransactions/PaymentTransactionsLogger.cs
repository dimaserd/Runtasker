using Logic.Extensions.Namers;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Enumerations;

namespace Runtasker.Logic.Workers.PaymentTransactions
{
    public class PaymentTransactionsLogger
    {
        #region Constructors
        public PaymentTransactionsLogger(MyDbContext context)
        {
            Construct(context);
        }

        void Construct(MyDbContext context)
        {
            Context = context;
            //For naming descriptions in transactions
            DescNamer = new PaymentTransactionDescriptionNamer();
        }
        #endregion

        #region Properties
        MyDbContext Context { get; set; }
        PaymentTransactionDescriptionNamer DescNamer { get; set; }
        #endregion

        #region Public methods
        public void OnPaymentReceivedFromService(Payment payment, SaveChangesType saveType = SaveChangesType.Now)
        {

            PaymentTransaction pt = new PaymentTransaction
            {
                Description = (payment.ViaType == PaymentViaType.Robokassa) ? 
                    DescNamer.PaymentWithRobokassaService() : DescNamer.PaymentWithYandexMoneyFirstService(),

                Sum = payment.Amount,
                Type = TransactionType.Recharging,
                UserGuid = payment.UserGuid
            };

            Context.PaymentTransactions.Add(pt);

            if(saveType == SaveChangesType.Now)
            {
                Context.SaveChanges();
            }
        }
        #endregion
    }
}
