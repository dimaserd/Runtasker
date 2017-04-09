using Logic.Extensions.Models;
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Enumerations;
using Runtasker.Logic.Workers.Logging;
using Runtasker.Settings;
using System;
using System.Data.Entity;
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
            
            //invoiceId - идентификатор платежа в системе яндексКасса


            //action; суммазаказа; orderSumCurrencyPaycash; orderSumBankPaycash; shopId; invoiceId; customerNumber; shopPassword
            //формирую строку для расчета MD5
            string val = $"{action};{orderSumAmount};{orderSumCurrencyPaycash};{orderSumBankPaycash};{shopId};{invoiceId};{customerNumber};{YandexKassaSettings.ShopPassword}";

            //считаю хеш
            string md5 = CreateMD5(val);



            //сравниваю строки
            bool compareResult = string.Compare(md5, MD5, StringComparison.OrdinalIgnoreCase) == 0;

            
            //если хеши идентичны то 
            if (compareResult)
            {
                //получаю существующий платеж по идентификатору системы яндексКасса
                Payment existingPayment = await Context.Payments.Include(x => x.User).FirstOrDefaultAsync(x => x.PaymentServiceId == invoiceId);

                //если платежа не существует выходим из метода
                if(existingPayment == null)
                {
                    return new WorkerResult("0");
                }

                //если платеж уже подтвержден
                if(existingPayment.Confirmed && existingPayment.Hash == MD5.ToLower())
                {
                    //выходим из метода
                    return new WorkerResult
                    {
                        Succeeded = true
                    };
                }

                //получаем пользователя из платежа
                ApplicationUser user = existingPayment.User;

                //изменяем данные платежа
                existingPayment.Confirmed = true;
                existingPayment.Hash = MD5.ToLower();
                Context.Entry(existingPayment).State = EntityState.Modified;

                //сохраняем изменения
                await Context.SaveChangesAsync();

                //записываем платежное уведомление но не делаем сохранений
                PTLogger.OnPaymentReceivedFromService(existingPayment, SaveChangesType.Handled);

                //сохраняем добавленное
                await Context.SaveChangesAsync();

                //добавляем пользователю денег на счет
                AddMoneyToUser(existingPayment);

                //Передаем сущность пользователя в класс уведомлений
                //чтобы избежать запроса в базу данных
                Notificater.SetCustomer(user);

                //и создаем платежное уведомление
                Notificater.OnUserPaid(existingPayment);
                
                //возвращаем успешный результат операции
                return new WorkerResult
                {
                    Succeeded = true
                };
            }

            //возвращаю неуспешный результат операции
            return new WorkerResult("1");
        }


        public async Task<WorkerResult> YandexKassaPaymentStartedAsync(string action = null, string orderSumAmount = null,
            string orderSumCurrencyPaycash = null, string orderSumBankPaycash = null,
            string shopId = null, string invoiceId = null, string customerNumber = null,
            string MD5 = null)
        {
            //находим пользователя по email
            ApplicationUser user = await UserManager.FindByEmailAsync(customerNumber);
            
            //создаем платеж в системе
            Payment payment = new Payment
            {
                //MD5 может измениться
                Hash = MD5.ToLower(),
                UserGuid = user.Id,
                Date = DateTime.Now,
                //записываем идентификатор платежа в системе ЯндексКасса
                //по нему будет проходить подтверждение
                PaymentServiceId = invoiceId,
                //платеж не подтвержден
                Confirmed = false,
                ViaType = PaymentViaType.YandexKassa,
                Amount = decimal.Parse(orderSumAmount, CultureInfo.InvariantCulture)
            };

            //добавляю платеж в базу
            Context.Payments.Add(payment);

            await Context.SaveChangesAsync();

            return new WorkerResult
            {
                Succeeded = true
            };
        }
        #endregion
    }
}
