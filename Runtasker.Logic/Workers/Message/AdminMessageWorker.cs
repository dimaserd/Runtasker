using Runtasker.Logic.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System;
using Runtasker.Logic.Models.Messages;
using Runtasker.Logic.Models.Users;

namespace Runtasker.Logic.Workers.Message
{
    /// <summary>
    /// Класс отвечающий за методы чата для админа
    /// </summary>
    public class AdminMessageWorker : IDisposable
    {
        #region Конструкторы
        public AdminMessageWorker(MyDbContext context, string userId)
        {
            Context = context;
            UserId = userId;
        }
        #endregion

        #region Свойства
        string UserId { get; set; }

        MyDbContext Context { get; set; }
        #endregion

        #region Методы
        
        /// <summary>
        /// Получение сообщений по заказам
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<OrderChatInfo>> GetOrderChatInfosAsync()
        {
            List<Order> ordersWithMessages = await Context.Orders
                .Include(x => x.Messages)
                .Include(x => x.Customer)
                .ToListAsync();

            return ordersWithMessages.Select(x => x.ToOrderChatInfo(UserId))
                .OrderByDescending(x => x.UnreadCount);
        }

        /// <summary>
        /// Получение сообщений с заказчиком по поводу заказа
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="MyName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<OrderChatMessage>> GetChatAboutOrderAsync(int orderId, string MyName)
        {
            ApplicationUserInfo customerInfo = await GetCustomerInfoByOrderAsync(orderId);

            //получил информацию о текущем чате теперь надо засунуть его вперед списка
            OrderChatMessage info = GetChatInfo(customerInfo, orderId);

            List<Entities.Message> messages = await Context.Messages.Where(m => m.OrderId == orderId).ToListAsync();

            //создаю список
            List<OrderChatMessage> result = new List<OrderChatMessage>();

            result.Add(info);
            //добавляю  ссообщения
            result.AddRange(messages.Select(x => x.ToOrderChatMessage(MyName, customerInfo.Name, UserId)));

            return result;
        }


        #endregion

        #region Вспомогательные методы
        async Task<ApplicationUserInfo> GetCustomerInfoByOrderAsync(int orderId)
        {
            return (await Context.Orders
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == orderId)).Customer.ToApplicationUserInfo();
        }

        /// <summary>
        /// Довольно костыльный метод который возвращает общую информацию о текущем диалоге
        /// в виде сообщения (Скорее всего придется переделать)
        /// </summary>
        /// <returns></returns>
        OrderChatMessage GetChatInfo(ApplicationUserInfo chatterInfo, int orderId)
        {
            return new OrderChatMessage
            {
                OrderId = orderId,
                SenderId = UserId,
                ReceiverId = chatterInfo.UserId,
                ReceiverName = chatterInfo.Name,
                Text = "Сообщение специально для построения чата!"
            };
        }
        #endregion
        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }


        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
