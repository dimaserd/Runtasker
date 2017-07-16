using Logic.Extensions.Namers;
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using Runtasker.Settings;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers.MessageWorker
{

    public class MessageOrderWorker : MessageWorkerBase
    {
        #region Contructors
        public MessageOrderWorker(string userGuid, IMyDbContext context) : base(context)
        {
            Construct(0, userGuid);
        }

        //Other constructors with OrderId property
        public MessageOrderWorker(int orderId, string userGuid) : base()
        {
            Construct(orderId, userGuid);
        }

        private void Construct(int? orderId = null, string userGuid = null)
        {
            UserGuid = userGuid ?? "";
            OrderId = (orderId.HasValue) ? orderId.Value : 0;

            Namer = new MessageNamer();
        }
        #endregion

        #region Свойства

        
        //We get Guid and OrderId than we send messages to ChatBuilder
        private string UserGuid { get; set; }

        public int OrderId { get; set; }

        MessageNamer Namer { get; set; }

        #endregion

        #region Методы

        
        
        /// <summary>
        /// Возвращает идентификатор администратора
        /// </summary>
        /// <returns></returns>
        public string GetAdminId()
        {
            return Context.Users.FirstOrDefault(u => u.Email == AdminSettings.AdminEmail).Id;
        }

        /// <summary>
        /// Возвращает константное имя администратора (вне зависимости от указанных настроек)
        /// </summary>
        /// <returns></returns>
        public string GetAdminName()
        {
            return AdminSettings.ChatterName;
        }

        /// <summary>
        /// Возвращает заказчика по номеру заказа
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetCustomerAsync(int orderId)
        {
            return (await (Context.Orders
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == orderId)))
                .Customer;
        }
        #region Получение сообщений

        /// <summary>
        /// Возвращает сообщения о заказе для заказчика
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Entities.Message>> GetChatAboutOrderAsCustomerAsync(int orderId)
        {
            Order order = Context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null || order.UserGuid != UserGuid)
            {
                return null;
            }

            return await (from m in Context.Messages
                    where
                    m.Type == MessageType.AboutOrder
                    && m.OrderId == orderId
                    select m).ToListAsync();
        }


        /// <summary>
        /// Возвращает сообщения о заказе для исполнителя или администратора
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Entities.Message>> GetChatAboutOrderAsPerformerAsync(int orderId)
        {
            Order order = Context.Orders.FirstOrDefault(o => o.Id == orderId);
            
            return await (from m in Context.Messages
                          where
                          m.Type == MessageType.AboutOrder
                          && m.OrderId == orderId
                          select m).ToListAsync();
        }
        #endregion

        public Entities.Message WriteMessageAboutOrder(Entities.Message message)
        {
            string mark = Namer.GetForMessageAboutOrder(OrderId);

            message.Type = MessageType.AboutOrder;
            message.Mark = mark; //mark message
            Context.Messages.Add(message);
            Context.SaveChanges();
            return message;
        }
        #endregion

        #region Overriden Methods
        public override IEnumerable<Entities.Message> GetChat()
        {
            return null;
        }

        public override Entities.Message SendMessage(Entities.Message message)
        {
            return WriteMessageAboutOrder(message);
        }
        #endregion
    }
}
