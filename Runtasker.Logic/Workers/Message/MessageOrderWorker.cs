using Logic.Extensions.Namers;
using Runtasker.Logic.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Runtasker.Logic.Workers.MessageWorker
{

    public class MessageOrderWorker : MessageWorkerBase
    {
        #region Contructors
        public MessageOrderWorker(string userGuid, MyDbContext context) : base(context)
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

        #region Private Properties
        //We get Guid and OrderId than we send messages to ChatBuilder
        private string UserGuid { get; set; }

        public int OrderId { get; set; }

        MessageNamer Namer { get; set; }

        #endregion

        #region Public Methods

        //for now its just an administrator
        public string GetChattterGuid()
        {
            return Context.Users.FirstOrDefault(u => u.Email == "dimaserd84@gmail.com").Id;
        }

        public string GetChatterName()
        {
            return "Jenny";
        }

        //Returns Messages about Order
        public IEnumerable<Message> GetChatAboutOrder(int orderId)
        {
            Order order = Context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null || order.UserGuid != UserGuid)
            {
                return null;
            }

            return (from m in Context.Messages
                    where
                    m.Type == MessageType.AboutOrder
                    && m.Mark == orderId.ToString()
                    select m).ToList();
        }

        public Message WriteMessageAboutOrder(Message message)
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
        public override IEnumerable<Message> GetChat()
        {
            return GetChatAboutOrder(OrderId);
        }

        public override Message SendMessage(Message message)
        {
            return WriteMessageAboutOrder(message);
        }
        #endregion
    }
}
