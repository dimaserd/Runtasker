using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Notifications;
using Runtasker.Logic.Workers.Payments.PaymentGetters;
using Runtasker.Logic.Workers.PaymentTransactions;
using System;
using System.IO;

namespace Runtasker.Logic.Workers
{
    public class YandexPaymentWorker : PaymentGetterBase
    {
        #region Constructors
        public YandexPaymentWorker(MyDbContext context)
        {
            Construct(context);
        }

        void Construct(MyDbContext context)
        {
            Context = context ?? new MyDbContext();

            
            
            Notificater = new PaymentNotificationMethods(Context);

            PTLogger = new PaymentTransactionsLogger(Context);

            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));
        }
        #endregion

        #region Constants
        private const string YandexMoneyKey = "dujsxS25X9YAGEvpurtazW+O";
        #endregion

        #region Private Properties
        MyDbContext Context { get; set; }

        PaymentNotificationMethods Notificater { get; set; }

        PaymentTransactionsLogger PTLogger { get; set; }

        UserManager<ApplicationUser> userManager { get; set; }

        #endregion

        #region Public Methods

        public void GetPayment(string notification_type = null, string operation_id = null,
            string label = null, string datetime = null, string amount = null,
            string withdraw_amount = null, string sender = null, string sha1_hash = null,
            string currency = null, bool? codepro = null)
        {
            #region Temporary Log Methods
            string RootDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");

            string filePath = $"{RootDirectory}/yandex.txt";

            string fileContents = $"came to Yandex GetPayment {DateTime.Now} \n";
            File.AppendAllText(filePath, fileContents);
            #endregion






            string userGuid = label;

            
            

            // проверяем хэш
            string paramString = String.Format("{0}&{1}&{2}&{3}&{4}&{5}&{6}&{7}&{8}",
                notification_type, operation_id, amount, currency, datetime, sender,
                codepro.ToString().ToLower(), YandexMoneyKey, label);


            string paramStringHash1 = GetHash(paramString);
            
            // создаем класс для сравнения строк
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            
            // если хэши идентичны, добавляем данные о заказе в бд
            if (0 == comparer.Compare(paramStringHash1, sha1_hash))
            {
                File.AppendAllText(filePath, "Hashes identical! \n");

                Payment payment = new Payment
                {
                    ViaType = PaymentViaType.YandexMoney,
                    Amount = Decimal.Parse(withdraw_amount),
                    UserGuid = userGuid,
                };
                
                Context.Payments.Add(payment);
                Context.SaveChanges();

                //Writing transaction to database
                PTLogger.OnPaymentReceivedFromService(payment);

                AddMoneyToUser(payment);
                Notificater.OnUserPaid(payment);
            }
        }

        #endregion

        #region Private Methods
        private void AddMoneyToUser(Payment p)
        {
            var user =  userManager.FindById(p.UserGuid);

            if (user != null)
            {
                user.Balance += p.Amount;
            }
            else
            {
                string RootDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
                string filePath = $"{RootDirectory}/errors.txt";
                string fileContents = $"{DateTime.Now} User not found bu guid \n";
                File.AppendAllText(filePath, fileContents);
            }
            userManager.Update(user);
            
        }


        
        #endregion
    }
}
