using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Notifications;
using System;

namespace Runtasker.Logic.Workers.Orders
{
    public class CustomerOrderErrorEvents
    {
        #region Конструктор
        public CustomerOrderErrorEvents(string userGuid, MyDbContext context)
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

        MyDbContext Context { get; set; }

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
