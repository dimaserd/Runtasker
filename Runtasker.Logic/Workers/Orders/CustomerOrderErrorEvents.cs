using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Notifications;
using System;

namespace Runtasker.Logic.Workers.Orders
{
    public class CustomerOrderErrorEvents
    {
        #region Конструктор
        public CustomerOrderErrorEvents(string userGuid, IMyDbContext context)
        {
            UserGuid = userGuid;
            Context = context;
            Construct();
        }

        void Construct()
        {
            Notificater = new CustomerOrderNotificationErrorMethods(UserGuid, Context);
        }
        #endregion

        #region Свойства
        string UserGuid { get; set; }

        IMyDbContext Context { get; set; }

        CustomerOrderNotificationErrorMethods Notificater { get; set; }
        #endregion

        #region Методы по событиям
        public void OnCustomerTriedToAddAnOrderWithUnconfirmedAccount()
        {
            Notificater.OnCustomerTriedToAddAnOrderWithUnconfirmedAccount();
        }

        public void OnCustomerTriedToPayWithoutMoney(ApplicationUser customer, Order order)
        {
            decimal sumThatUserNeedToPay = (order.WorkType == OrderWorkType.OnlineHelp)? order.Sum : order.Sum / 2;
            
            Notificater.OnCustomerTriedToPayWithoutMoney(customer.Balance, sumThatUserNeedToPay, order.Id);
        }
        #endregion
    }
}
