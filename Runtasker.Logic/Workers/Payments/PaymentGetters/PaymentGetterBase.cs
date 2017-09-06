using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Notifications;
using Runtasker.Logic.Workers.PaymentTransactions;
using System.Text;

namespace Runtasker.Logic.Workers.Payments.PaymentGetters
{
    public class PaymentGetterBase
    {
        #region Constructors
        public PaymentGetterBase(MyDbContext context)
        {
            Context = context;
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));
            Notificater = new PaymentNotificationMethods(Context);
            PTLogger = new PaymentTransactionsLogger(Context);
        }
        #endregion

        #region Свойства
        protected MyDbContext Context { get; set; }

        protected UserManager<ApplicationUser> UserManager { get; set; }

        protected PaymentNotificationMethods Notificater { get; set; }

        protected PaymentTransactionsLogger PTLogger { get; set; }
        #endregion

        #region Методы
        /// <summary>
        /// Добавляет деньги на счет пользователя по платежу
        /// </summary>
        /// <param name="p"></param>
        protected void AddMoneyToUser(Payment p)
        {
            ApplicationUser user = UserManager.FindById(p.UserGuid);

            if (user != null)
            {
                //прибавляем к балансу пользователя сумму платежа 
                user.Balance += p.Amount;
            }
            //сохраняем изменения
            UserManager.Update(user);
        }

        #region Методы шифрования
        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        #endregion

        #endregion
    }
}
