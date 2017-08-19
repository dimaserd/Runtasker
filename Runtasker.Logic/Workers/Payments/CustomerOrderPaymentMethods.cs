using Logic.Extensions.Namers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Enumerations;
using System;

namespace Runtasker.Logic.Workers.Payments
{
    public class CustomerOrderPaymentMethods
    {
        #region Constructors
        public CustomerOrderPaymentMethods(string userGuid, UserManager<ApplicationUser> userManager, IMyDbContext context)
        {
            Construct(userGuid);
            UserManager = userManager;
            Context = context;
        }

        void Construct(string userGuid)
        {
            UserGuid = userGuid;

            DescNamer = new PaymentTransactionDescriptionNamer();
        }
        #endregion

        #region Свойства
        IMyDbContext Context { get; set; }

        string UserGuid { get; set; }

        UserManager<ApplicationUser> UserManager { get; set; }

        PaymentTransactionDescriptionNamer DescNamer { get; set; }
        #endregion

        #region Методы по событиям
        public void OnCustomerPaidFirstHalfOfAnOrder(Order order, SaveChangesType saveType = SaveChangesType.Now)
        {
            decimal sum = order.PaidSum;

            MinusUserBalance(sum);
            PaymentTransaction PT = new PaymentTransaction
            {
                Sum = -sum,
                Description = DescNamer.GetForPayFirstHalfOfAnOrder(order.Id),
                UserGuid = UserGuid,
                Type = TransactionType.Spending
            };

            Context.PaymentTransactions.Add(PT);

            if(saveType == SaveChangesType.Now)
            {
                Context.SaveChanges();
            }
            
        }

        public void OnCustomerPaidSecondHalfOfAnOrder(Order order, SaveChangesType saveType = SaveChangesType.Now)
        {
            decimal sum = order.Sum / 2;

            MinusUserBalance(sum);

            //создаем платежное уведомление
            PaymentTransaction PT = new PaymentTransaction
            {
                Sum = -sum,
                Description = DescNamer.GetForPayAnotherHalfOfAnOrder(order.Id),
                UserGuid = order.UserGuid,
                Type = TransactionType.Spending
            };

            //добавление его в базу
            Context.PaymentTransactions.Add(PT);

            if(saveType == SaveChangesType.Now)
            {
                Context.SaveChanges();
            }
        }

        public void OnCutomerPaidOnlineHelp(Order order, SaveChangesType saveType = SaveChangesType.Now)
        {
            if(order.WorkType != OrderWorkType.OnlineHelp)
            {
                throw new Exception("Данный заказ не является онлайн-помощью!");
            }

            decimal sum = order.Sum;

            //вычитыем деньги из баланса пользователя
            MinusUserBalance(sum);

            //создаем платежное уведомление
            PaymentTransaction PT = new PaymentTransaction
            {
                Sum = -sum,
                Description = DescNamer.GetForPayOnlineHelp(order.Id),
                UserGuid = order.UserGuid,
                Type = TransactionType.Spending
            };
            //добавляем в базу
            Context.PaymentTransactions.Add(PT);

            if(saveType == SaveChangesType.Now)
            {
                //сохраняем изменения
                Context.SaveChanges();
            }
            
        }

        public void OnInvitedCustomerFinishedOrder(Invitation I)
        {
            PaymentTransaction PT = new PaymentTransaction
            {
                Sum = 300,
                Description = DescNamer.GetForInvitedUser(I.ReceiverEmail),
                UserGuid = I.SenderGuid,
                Type = TransactionType.Recharging
            };
            Context.PaymentTransactions.Add(PT);
            Context.SaveChanges();
        }
        #endregion

        #region Help Methods
        void MinusUserBalance(decimal sum)
        {
            ActionWithBalanceSubMethod(-sum);
        }

        void PlusUserBalance(decimal sum, string userGuid)
        {
            ActionWithBalanceSubMethod(sum, userGuid);
        }

        /// <summary>
        /// Добавляет денег пользователю
        /// </summary>
        /// <param name="sum"></param>
        /// <param name="userGuid"></param>
        void ActionWithBalanceSubMethod(decimal sum, string userGuid = null)
        {
            if(userGuid == null)
            {
                userGuid = UserGuid;
            }
            var user = UserManager.FindById(userGuid);
            if (user != null)
            {
                user.Balance += sum;
            }
            UserManager.Update(user);
            
        }
        #endregion
    }
}
