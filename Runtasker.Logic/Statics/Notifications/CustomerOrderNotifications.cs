using Runtasker.LocaleBuilders.Models;
using Runtasker.Logic.Enumerations.Notifications.Customer.Order;

namespace Runtasker.Logic.Statics.Notifications
{
    /// <summary>
    /// Придумай как это нормально сделать
    /// Вид уведомления ничего не должен знать о тексте 
    /// </summary>
    public static class CustomerOrderNotifications
    {
        public static ForNotification GetLocalNotification(CustomerOrderNotificationType notificationType)
        {
            switch(notificationType)
            {
                case (CustomerOrderNotificationType.OrderCreated):
                    return new ForNotification
                    {

                    };
                default:
                    return new ForNotification
                    {

                    };
            }
        }
    }
}
