using Logic.Extensions.Models;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Enumerations;
using Runtasker.Logic.Workers.Logging;
using Runtasker.Settings;
using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers.Payments.PaymentGetters
{
    public class YandexKassaPaymentGetter : PaymentGetterBase
    {
        #region Constructors
        public YandexKassaPaymentGetter(MyDbContext context) : base (context)
        {

        }


        #endregion

        #region Public methods

        /// <summary>
        /// Метод оплаты логгируется
        /// </summary>
        /// <param name="action"> описывает действие</param>
        /// <param name="orderSumAmount"></param>
        /// <param name="orderSumCurrencyPaycash"></param>
        /// <param name="orderSumBankPaycash"></param>
        /// <param name="shopId"></param>
        /// <param name="invoiceId"></param>
        /// <param name="customerNumber"></param>
        /// <param name="MD5"></param>
        /// <returns></returns>
        public async Task<WorkerResult> YandexKassaNotificatedAsync(string action = null, string orderSumAmount = null,
            string orderSumCurrencyPaycash = null, string orderSumBankPaycash = null,
            string shopId = null, string invoiceId = null, string customerNumber = null,
            string MD5 = null)
        {

            //action; суммазаказа; orderSumCurrencyPaycash; orderSumBankPaycash; shopId; invoiceId; customerNumber; shopPassword
            //формирую строку для расчета MD5
            string val = $"{action};{orderSumAmount};{orderSumCurrencyPaycash};{orderSumBankPaycash};{shopId};{invoiceId};{customerNumber};{YandexKassaSettings.ShopPassword}";

            //считаю хеш
            string md5 = CreateMD5(val);



            //сравниваю строки
            bool compareResult = string.Compare(md5, MD5, StringComparison.OrdinalIgnoreCase) == 0;

            #region Логгирование в файл
            using (LoggingWorker logger = new LoggingWorker())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"action={action}\n")
                .Append($"orderSumAmount={orderSumAmount}\n")
                .Append($"orderSumCurrencyPaycash={orderSumCurrencyPaycash}\n")
                .Append($"orderSumBankPaycash={orderSumBankPaycash}\n")
                .Append($"shopId={shopId}\n")
                .Append($"invoiceId={invoiceId}\n")
                .Append($"customerNumber={customerNumber}\n")
                .Append($"MD5={MD5}\n")
                .Append($"РассчитанныйMD5={md5}\n")
                .Append($"Хеши совпали={compareResult}\n");

                string fileName = "yandexKassaTest.txt";

                logger.LogTextToFile(fileName, sb.ToString());
            }
            #endregion

            //если хеши идентичны то 
            if (compareResult)
            {
                //получаем пользователя по электронному адресу
                ApplicationUser user = await UserManager.FindByEmailAsync(customerNumber);

                //создаем платеж
                Payment payment = new Payment
                {
                    ViaType = PaymentViaType.YandexKassa,
                    Amount =  decimal.Parse(orderSumAmount, CultureInfo.InvariantCulture),
                    UserGuid = user.Id,
                };
                //добавляем платеж в базу
                Context.Payments.Add(payment);

                //записываем платежное уведомление но не делаем сохранений
                PTLogger.OnPaymentReceivedFromService(payment, SaveChangesType.Handled);

                //сохраняем добавленное
                await Context.SaveChangesAsync();

                //добавляем пользователю денег на счет
                AddMoneyToUser(payment);

                //Передаем сущность пользователя в класс уведомлений
                //чтобы избежать запроса в базу данных
                Notificater.SetCustomer(user);

                //и создаем платежное уведомление
                Notificater.OnUserPaid(payment);
                
                //возвращаем успешный результат операции
                return new WorkerResult
                {
                    Succeeded = true
                };
            }

            //возвращаю неуспешный результат операции
            return new WorkerResult("1");
        }

        #endregion
    }
}
