using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Notifications;
using Runtasker.Logic.Workers.PaymentTransactions;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Runtasker.Logic.Workers.Payments
{
    public class RobokassaPaymentMethods
    {
        #region Constants
        string RoboKassaPass2
        {
            get { return "D9hR9RUMs01jh8mKgMWB"; }
        }
        
        string RoboKassaLogin
        {
            get { return "runtasker"; }
        }
        #endregion

        #region Constructors
        public RobokassaPaymentMethods(MyDbContext context, string userGuid)
        {
            Construct(context, userGuid);
        }

        void Construct(MyDbContext context, string userGuid = null)
        {
            Context = context;
            UserGuid = userGuid;
            Notificater = new PaymentNotificationMethods(Context);
            PTLogger = new PaymentTransactionsLogger(Context);
        }
        #endregion

        #region Properties
        MyDbContext Context { get; set; }

        string UserGuid { get; set; }

        PaymentNotificationMethods Notificater { get; set; }

        PaymentTransactionsLogger PTLogger { get; set; }
        #endregion

        #region Public Methods
        //7 percent withdraw
        public RoboKassaPaymentModel GetPaymentModel(decimal sum)
        {
            Payment payment = new Payment
            {
                UserGuid = UserGuid,
                ViaType = PaymentViaType.Robokassa,
                Description = null,
                Amount = sum
            };
            Context.Payments.Add(payment);
            Context.SaveChanges();

            return new RoboKassaPaymentModel
            {
                Amount = sum,
                //7 percent withdraw
                WithdrawAmount = sum * 1.07m,
                PaymentId = payment.Id
            };
        }

        public void OnPaymentReceived(string OutSum, string InvId, string SignatureValue)
        {


            int id;
            decimal outSum;
            if(!int.TryParse(InvId, out id) || !decimal.TryParse(OutSum, out outSum))
            {
                return;
            }

            Payment payment = Context.Payments.FirstOrDefault(p => p.Id == id);
            if(payment == null || payment.Amount != outSum)
            {
                return;
            }
            //OutSum:InvId:Пароль#1
            string hashBase = string.Format($"{OutSum}:{InvId}:{RoboKassaPass2}");

            //comparing hashes
            if(string.Compare(SignatureValue, GetHash(hashBase), StringComparison.OrdinalIgnoreCase) != 0)
            {               
                return;
            }

            ApplicationUser user = Context.Users.FirstOrDefault(u => u.Id == payment.UserGuid);
            if(user == null)
            {
                return;
            }

            PlusUserBalance(outSum, user);           
            Notificater.OnUserPaid(payment);
            PTLogger.OnPaymentReceivedFromService(payment);

        }
        #endregion

        #region Help Methods
        public string GetHash(string hashBase)
        {
            

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bSignature = md5.ComputeHash(Encoding.UTF8.GetBytes(hashBase));

            StringBuilder sbSignature = new StringBuilder();
            foreach (byte b in bSignature)
            {
                sbSignature.AppendFormat("{0:x2}", b);
            }


            string hash = sbSignature.ToString();

            return hash;
        }

        public void PlusUserBalance(decimal sum, ApplicationUser user)
        {
            UserManager<ApplicationUser> usermanager =
            new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));

            user.Balance += sum;
            usermanager.Update(user);

        }
        #endregion
    }
}
