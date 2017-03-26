using Logic.Extensions.Namers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using System;

namespace Runtasker.Logic.Workers.Payments
{
    public class CustomerOrderPaymentMethods
    {
        #region Constructors
        public CustomerOrderPaymentMethods(string userGuid, MyDbContext context)
        {
            Construct(userGuid);
            Context = context;
        }

        void Construct(string userGuid)
        {
            UserGuid = userGuid;

            DescNamer = new PaymentTransactionDescriptionNamer();
        }
        #endregion

        #region Properties
        MyDbContext Context { get; set; }

        string UserGuid { get; set; }

        PaymentTransactionDescriptionNamer DescNamer { get; set; }
        #endregion

        #region Methods like events
        public void OnCustomerPaidFirstHalfOfAnOrder(Order order)
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
            Context.SaveChanges();
        }

        public void OnCustomerPaidSecondHalfOfAnOrder(Order order)
        {
            decimal sum = order.Sum / 2;

            MinusUserBalance(sum);
            PaymentTransaction PT = new PaymentTransaction
            {
                Sum = -sum,
                Description = DescNamer.GetForPayAnotherHalfOfAnOrder(order.Id),
                UserGuid = order.UserGuid,
                Type = TransactionType.Spending
            };
            Context.PaymentTransactions.Add(PT);
            Context.SaveChanges();
        }

        public void OnCutomerPaidOnlineHelp(Order order)
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
            Context.PaymentTransactions.Add(PT);
            Context.SaveChanges();
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

        void ActionWithBalanceSubMethod(decimal sum, string userGuid = null)
        {
            if(userGuid == null)
            {
                userGuid = UserGuid;
            }
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));
            var user = userManager.FindById(userGuid);
            if (user != null)
            {
                user.Balance += sum;
            }
            userManager.Update(user);
        }
        #endregion
    }
}
