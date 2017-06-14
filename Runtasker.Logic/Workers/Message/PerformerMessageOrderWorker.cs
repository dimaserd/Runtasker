using Logic.Extensions.Namers;
using Runtasker.Logic.Entities;
using System.Collections.Generic;
using System.Linq;

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

        //To reduce calls to database
        public void SetOrder(int orderId)
        {
            _Order = Context.Orders.FirstOrDefault(o => o.Id == orderId);
        }

        public string GetChatterName(int? orderId = null)
        {
            Order order = _Order ?? Context.Orders.FirstOrDefault(o => o.Id == orderId.Value);
            if(order != null)
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
            Order order = _Order ?? Context.Orders.FirstOrDefault(o => o.Id == orderId.Value);
            return (order != null) ? order.UserGuid : "";
        }

        public IEnumerable<Entities.Message> GetMessagesAboutOrder(int? orderId = null)
        {
            Order order = _Order ?? Context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return null;
            }

            //Getting mark for messages about order
            string mark = Namer.GetForMessageAboutOrder(order.Id);

            return (from m in Context.Messages
                    where
                    m.Type == MessageType.AboutOrder
                    && m.Mark == mark
                    select m).ToList();
        }

        #endregion
    }
}
