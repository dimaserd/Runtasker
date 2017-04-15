using Runtasker.LocaleBuilders.Models;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models.ManageModels;
using System.Collections.Generic;
using VkParser.Models.MessageSenderModels;

namespace Runtasker.Logic.Workers.Notifications.Orders.VkNotifications
{
    /// <summary>
    /// Класс работающий по событиям связанным с заказом
    /// со стороны заказчика
    /// </summary>
    public class VkCustomerNotificationMethods
    {
        #region Конструктор
        public VkCustomerNotificationMethods()
        {

        }
        #endregion

        #region Методы по событиям

       /// <summary>
       /// Не сделано
       /// </summary>
       /// <param name="order"></param>
       /// <param name="notificationModel"></param>
        public void OnCustomerAddedOrder(Order order, ForNotification notificationModel)
        {
            //получение необходимой информации о заказчике 
            //для отправления уведомления через вконтакте
            OtherUserInfo customerInfo = order.Customer.GetOtherInfo();

            //создаю список сообщений которые будут отправлены
            List<VkMessage> messages = new List<VkMessage>();

            //создаю сообщение для пользователя
            VkMessage customerMessage = new VkMessage
            {
                Text = notificationModel.ToString(),
                UserDomain = customerInfo.VkDomain,
                UserId = customerInfo.VkId
            };

            //и добавляю его в список отправляемых сообщений
            messages.Add(customerMessage);

            VkMessage adminMessage = new VkMessage
            {
                Text = $"Пользователь добавил заказ по предмету {order.Subject.ToDescriptionString()}",
                UserDomain = null,
                UserId = null,
            };
        }
        #endregion
    }
}
