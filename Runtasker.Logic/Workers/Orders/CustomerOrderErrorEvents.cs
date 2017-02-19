using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Notifications;

namespace Runtasker.Logic.Workers.Orders
{
    public class CustomerOrderErrorEvents
    {
        #region Constructors
        public CustomerOrderErrorEvents(string userGuid)
        {
            Construct(userGuid);
        }

        void Construct(string userGuid)
        {
            
            UserGuid = userGuid;
            Notificater = new CustomerOrderNotificationErrorMethods(UserGuid);
        }
        #endregion

        #region Properties
        string UserGuid { get; set; }

        CustomerOrderNotificationErrorMethods Notificater { get; set; }
        #endregion

        #region Public Methods like events
        public void OnCustomerTriedToAddAnOrderWithUnconfirmedAccount()
        {
            Notificater.OnCustomerTriedToAddAnOrderWithUnconfirmedAccount();
        }

        public void OnCustomerTriedToPayWithoutMoney(ApplicationUser customer, Order order)
        {
            Notificater.OnCustomerTriedToPayWithoutMoney(customer.Balance, order);
        }
        #endregion
    }
}
