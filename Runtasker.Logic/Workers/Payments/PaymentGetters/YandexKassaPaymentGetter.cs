using Logic.Extensions.Models;
using Runtasker.Logic.Workers.Logging;
using Runtasker.Settings;
using System;
using System.Text;

namespace Runtasker.Logic.Workers.Payments.PaymentGetters
{
    public class YandexKassaPaymentGetter : PaymentGetterBase
    {
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
        public WorkerResult YandexKassaNotificated(string action = null, string orderSumAmount = null,
            string orderSumCurrencyPaycash = null, string orderSumBankPaycash = null,
            string shopId = null, string invoiceId = null, string customerNumber = null,
            string MD5 = null)
        {



            //формирую строку для расчета MD5
            string val = $"{action}{orderSumAmount}{orderSumCurrencyPaycash}{orderSumBankPaycash}{shopId}{invoiceId}{customerNumber}{YandexKassaSettings.ShopPassword}";

            //считаю хеш
            string md5 = GetHash(val);



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

            if (compareResult)
            {
                return new WorkerResult
                {
                    Succeeded = true
                };
            }

            //возвращаю 
            return new WorkerResult("1");
        }
    }
}
