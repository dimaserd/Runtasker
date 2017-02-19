using Logic.Extensions.Namers;
using Runtasker.Logic.Entities;
using System.IO;

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
        public void OnPaymentReceivedFromService(Payment payment)
        {
            #region Temporary Log Methods

            //File logging
            string RootDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
            string filePath = $"{RootDirectory}/payment_transactions.txt";
            string fileContents = $"came to OnPaymentReceivedFromService \n"
            + $"Context is not null : {(Context != null)} \n"
            + $"Payment : {Newtonsoft.Json.JsonConvert.SerializeObject(payment)} \n"
            + $"________________________________________ \n";

            File.AppendAllText(filePath, fileContents);

            #endregion
            PaymentTransaction pt = new PaymentTransaction
            {
                Description = (payment.ViaType == PaymentViaType.Robokassa) ? 
                    DescNamer.PaymentWithRobokassaService() : DescNamer.PaymentWithYandexMoneyFirstService(),

                Sum = payment.Amount,
                Type = TransactionType.Recharging,
                UserGuid = payment.UserGuid
            };

            Context.PaymentTransactions.Add(pt);
            Context.SaveChanges();
        }
        #endregion
    }
}
