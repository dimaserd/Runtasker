using Logic.Extensions.Models;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.BaseWorkers;
using VkParser.Models.MessageSenderModels;

namespace Runtasker.Logic.Workers.Notifications.Orders.VkNotifications
{
    /// <summary>
    /// Класс который создает текстовые уведомления для иссполнителей
    /// </summary>
    public class VkPerformerNotificater : BaseContextWorker
    {
        #region Constructor
        public VkPerformerNotificater(MyDbContext db) : base(context: db)
        {

        }
        #endregion

        #region Public methods
        public string OnCustomerAddedOrder(Order order)
        {
            return $"Пользователь добавил заказ по предмету {order.Subject.ToDescriptionString()}\n"
                + $"Проверьте его на наличие ошибок и оцените его стоимость, помните что"
                + $"заказ нужно завершить к {order.FinishDate.ToShortDateString()}";

        }
        #endregion
    }
}
