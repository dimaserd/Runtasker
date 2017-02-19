using Logic.Extensions.Namers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Entities;
using System;

namespace Runtasker.Logic.Workers.Payments
{
    public class CustomerOrderPaymentMethods
    {
        #region Constructors
        public CustomerOrderPaymentMethods(string userGuid)
        {
            Construct(userGuid);
        }

        void Construct(string userGuid)
        {
            UserGuid = userGuid;

            DescNamer = new PaymentTransactionDescriptionNamer();
        }
        #endregion

        #region Properties
        string UserGuid { get; set; }

        PaymentTransactionDescriptionNamer DescNamer { get; set; }
        #endregion

        #region Methods like events
        public void OnCustomerPaidFirstHalfOfAnOrder(Order order)
        {
            using (MyDbContext context = new MyDbContext())
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
                context.PaymentTransactions.Add(PT);
                context.SaveChanges();
            }
        }

        public void OnCustomerPaidSecondHalfOfAnOrder(Order order)
        {
            using (MyDbContext context = new MyDbContext())
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
                context.PaymentTransactions.Add(PT);
                context.SaveChanges();
            }
        }

        public void OnInvitedCustomerFinishedOrder(Invitation I)
        {
            using (MyDbContext context = new MyDbContext())
            {
                PaymentTransaction PT = new PaymentTransaction
                {
                    Sum = 300,
                    Description = DescNamer.GetForInvitedUser(I.ReceiverEmail),
                    UserGuid = I.SenderGuid,
                    Type = TransactionType.Recharging
                };
                context.PaymentTransactions.Add(PT);
                context.SaveChanges();
            }
        }
        #endregion

        #region Help Methods
        void MinusUserBalance(decimal sum)
        {
            using (MyDbContext context = new MyDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var user = userManager.FindById(UserGuid);
                if (user != null)
                {
                    user.Balance -= sum;
                }
                userManager.Update(user);
            }
        }

        void PlusUserBalance(decimal sum, string userGuid)
        {
            using (MyDbContext context = new MyDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var user = userManager.FindById(userGuid);
                if (user != null)
                {
                    user.Balance += sum;
                }
                userManager.Update(user);
            }
        }
        #endregion
    }
}
