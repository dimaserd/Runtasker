using Logic.Extensions.Namers;
using Runtasker.Logic.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Runtasker.Logic.Workers.MessageWorker
{
    public class PerformerMessageOrderWorker
    {
        #region Конструкторы
        public PerformerMessageOrderWorker(MyDbContext context)
        {
            Context = context;
            Construct();
        }

        void Construct()
        {
            Namer = new MessageNamer();
        }
        #endregion

        #region Свойства

        MyDbContext Context { get; set; }
        Order _Order { get; set; } //Sets in function SetOrder, Maybe it should be done in constructor

        //We need it to get the mark of messages
        MessageNamer Namer { get; set; }
        #endregion

        #region Функции

        /// <summary>
        /// Метод,  который устанавливает заказ в поле подгружая все остальные связанные свойства. Чтобы сократить
        /// обращения к базе данных
        /// </summary>
        /// <param name="orderId"></param>
        public void SetOrder(int orderId)
        {
            if(_Order == null)
            {
                _Order = Context.Orders.Include(x => x.Customer).Include(x => x.Messages).FirstOrDefault(o => o.Id == orderId);

            }
        }

        public string GetChatterName(int? orderId = null)
        {
            
            Order order = _Order;
            if (order != null)
            {
                return Context.Users.FirstOrDefault(u => u.Id == order.UserGuid).Name;
            }
            else
            {
                return "";
            }
        }

        public string GetUserGuidToChat(int? orderId = null)
        {
            
            Order order = _Order;
            return (order != null) ? order.UserGuid : "";
        }

        public IEnumerable<Entities.Message> GetMessagesAboutOrder(int? orderId = null)
        {
            Order order = _Order;

            if (order == null)
            {
                return null;
            }

            
            return order.Messages;
        }

        #endregion
    }
}
