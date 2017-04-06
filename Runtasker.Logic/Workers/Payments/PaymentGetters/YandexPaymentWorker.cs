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
        public YandexPaymentWorker(MyDbContext context) : base (context)
        {
            Construct();
        }

        void Construct()
        {
        }
        #endregion

        #region Constants
        private const string YandexMoneyKey = "dujsxS25X9YAGEvpurtazW+O";
        #endregion

        

        #region Public Methods

        public void GetPayment(string notification_type = null, string operation_id = null,
            string label = null, string datetime = null, string amount = null,
            string withdraw_amount = null, string sender = null, string sha1_hash = null,
            string currency = null, bool? codepro = null)
        {
            string userGuid = label;
            
            // проверяем хэш
            string paramString = string.Format("{0}&{1}&{2}&{3}&{4}&{5}&{6}&{7}&{8}",
                notification_type, operation_id, amount, currency, datetime, sender,
                codepro.ToString().ToLower(), YandexMoneyKey, label);


            string paramStringHash1 = CreateMD5(paramString);
            
            // создаем класс для сравнения строк
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            
            // если хэши идентичны, добавляем данные о заказе в бд
            if (0 == comparer.Compare(paramStringHash1, sha1_hash))
            {
                Payment payment = new Payment
                {
                    ViaType = PaymentViaType.YandexMoney,
                    Amount = decimal.Parse(withdraw_amount),
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

        
    }
}
