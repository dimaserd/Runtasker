using Runtasker.Logic.Entities;
using System.Linq;

namespace Runtasker.Logic.Models.Messages
{
    /// <summary>
    /// Класс описыввающий какое колличество сообщений не прочитано пользователем по конкретному заказу
    /// </summary>sss
    public class OrderChatInfo
    {
        public int OrderId { get; set; }

        public int UnreadCount { get; set; }

        public ApplicationUser Chatter { get; set; }
    }

    public static class OrderChatExtensions
    {
        /// <summary>
        /// По айди пользователя выдаем кол-во непрочитанных им сообщений в данном списке
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static OrderChatInfo ToOrderChatInfo(this Order order, string userId)
        {
            return new OrderChatInfo
            {
                OrderId = order.Id,
                UnreadCount = order.Messages.Count(x => x.ReceiverId == userId && x.Status == MessageStatus.New),
                Chatter = order.Customer,
            };
        }
    }
}
