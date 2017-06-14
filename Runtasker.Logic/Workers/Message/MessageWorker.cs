using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Files;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers.MessageWorker
{

    public class MessageWorker : MessageWorkerBase
    {
        #region Constructors
        public MessageWorker(string userGuid) : base()
        {
            Construct(userGuid);
        }

        public MessageWorker(MyDbContext context, string guid) : base(context)
        {
            Construct(guid);
        }

        private void Construct(string guid = null)
        {
            FileWorker = new SuperFileWorker(Context);
            ChatMethods = new ChatHubMethods(Context, FileWorker);
            UserGuid = guid;
        }

        #endregion

        #region Private Fields

        private SuperFileWorker FileWorker { get; set; }

        private string UserGuid { get; set; }

        #endregion

        #region Public Properties
        //Maybe we don't need this in Message Worker
        public ChatHubMethods ChatMethods { get; private set; }
        #endregion

        #region Public Methods

        public MessageIndexViewModel GetMessageIndexModel()
        {
            return new MessageIndexViewModel
            {
                AboutOrderChatIds = GetAboutOrderChatIds(),
                ChatGuids = null
            };

        }

        public IEnumerable<Runtasker.Logic.Entities.Message> GetChat(string toGuid, string userGuid = null)
        {
            string userGuidParam = userGuid ?? UserGuid;
            return from m in Context.Messages
                   where (m.SenderGuid == userGuidParam && m.ReceiverGuid == toGuid)
                      || (m.SenderGuid == toGuid && m.ReceiverGuid == userGuidParam)
                   orderby m.Date
                   select m;
        }

        public PanelViewModel GetPanelModel()
        {
            return new PanelViewModel
            {
                InboxCount = Context.Messages.Count(m => m.ReceiverGuid == UserGuid 
                    && m.Status == MessageStatus.New),
                OrderChatLinks = GetOrderChatLinks()
            };

        }

        public async Task<PanelViewModel> GetPanelModelAsync()
        {
            return new PanelViewModel
            {
                InboxCount = Context.Messages.Count(m => m.ReceiverGuid == UserGuid
                    && m.Status == MessageStatus.New),
                OrderChatLinks = await GetOrderChatLinksAsync()
            };

        }
        #endregion

        #region Private Methods

        private IEnumerable<int> GetAboutOrderChatIds()
        {
            List<int> result = new List<int>();
            foreach(Order order in GetActiveOrders())
            {
                result.Add(order.Id);
            }
            return result;
        }

        private IEnumerable<Order> GetActiveOrders()
        {
            return Context.Orders.Where(o => o.UserGuid == UserGuid && o.Status != OrderStatus.Downloaded).ToList();
        }

        private int GetNumberOfUnreadMessagesAboutOrder(int orderId)
        {
            return Context.Messages.Where(o => o.Type == MessageType.AboutOrder
            && o.Mark == orderId.ToString() && o.Status == MessageStatus.New
            && o.ReceiverGuid == UserGuid).ToList().Count;
        }
        //testing
        private IEnumerable<OrderChatLink> GetOrderChatLinks2()
        {
            var result = new List<OrderChatLink>();
            foreach(Order order in GetActiveOrders())
            {
                //Undone
                result.Add(
                new OrderChatLink
                {
                    OrderId = order.Id,
                    UnreadMessages = GetNumberOfUnreadMessagesAboutOrder(order.Id)
                });
            }
            return result.AsEnumerable();
        }

        IEnumerable<OrderChatLink> GetOrderChatLinks()
        {
            IEnumerable<OrderChatLink> result =
                (from order in Context.Orders
                 where order.UserGuid == UserGuid
                 && order.Status != OrderStatus.Appreciated

                 select new OrderChatLink
                 {
                     OrderId = order.Id,
                     UnreadMessages = Context.Messages.Count(m => m.OrderId == order.Id && 
                                                             m.Status == MessageStatus.New &&
                                                             m.ReceiverGuid == UserGuid) 
                }).ToList();
            return result;
        }

        async Task<IEnumerable<OrderChatLink>> GetOrderChatLinksAsync()
        {
            IEnumerable<OrderChatLink> result = await
                (from order in Context.Orders
                 where order.UserGuid == UserGuid
                 && order.Status != OrderStatus.Appreciated

                 select new OrderChatLink
                 {
                     OrderId = order.Id,
                     UnreadMessages = Context.Messages.Count(m => m.OrderId == order.Id &&
                                                             m.Status == MessageStatus.New &&
                                                             m.ReceiverGuid == UserGuid)
                 }).ToListAsync();
            return result;
        }

        #endregion
    }

    public class ChatHubMethods
    {
        #region Constructors
        public ChatHubMethods(IMyDbContext context, SuperFileWorker fileworker)
        {
            Context = context;
            FileWorker = fileworker;
        }
        #endregion

        #region Private Fields
        IMyDbContext Context { get; set; }

        SuperFileWorker FileWorker { get; set; }
        #endregion

        #region Public Methods
        public Entities.Message SendMessage(Entities.Message message)
        {
            message.AttachmentId = FileWorker.AttachmentWorker.GetToMessage(message.AttachmentId);
            message.Date = DateTime.Now;
            message.Status = MessageStatus.New;
            message.Mark = "";
            message.Type = MessageType.Ordinary;

            Context.Messages.Add(message);
            Context.SaveChanges();
            return message;
        }

        public void MarkMessagesAsRead(int id)
        {
            Entities.Message message = Context.Messages.FirstOrDefault(m => m.Id == id);
            if(message == null)
            {
                return;
            }
            message.Status = MessageStatus.Read;
            Context.SaveChanges();
        }
        #endregion

        #region Private Methods
        private IEnumerable<Entities.Message> GetOnlyNewMessagesFromUser(int id, string senderGuid, string receiverGuid)
        {
            return from m in Context.Messages
                   where m.SenderGuid == receiverGuid
                   && m.ReceiverGuid == senderGuid
                   && m.Id <= id
                   && m.Status == MessageStatus.New
                   select m;
        }
        #endregion

    }
}
