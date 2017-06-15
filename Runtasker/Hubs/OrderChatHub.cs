using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.MessageWorker;
using System.Linq;
using Microsoft.AspNet.SignalR.Hubs;
using Runtasker.Logic.Workers.Files;
using Runtasker.Logic.Models.Messages;

namespace Runtasker.Hubs
{
    /// <summary>
    /// Хаб, отвечающий за чат между пользователями
    /// </summary>
    [HubName("aboutOrderChatHub")]
    public class AboutOrderChatHub : ChatHubBase
    {
        #region Конструкторы
        public AboutOrderChatHub()
        {
            Filer = new SuperFileWorker(context);
        }
        #endregion

        #region Свойства
        SuperFileWorker Filer { get; set; }
        #endregion

        #region Реализованные методы

        UIHubMessage SendMessage(object message)
        {
            OrderHubMessage m = message as OrderHubMessage;

            string attachmentId = Filer.AttachmentWorker.GetToMessage(m.Attachments);

            Message mes = m.ToMessage(attachmentId);
            
            //добавление сообщения в базу
            context.Messages.Add(mes);
            
            //сохранение изменений
            context.SaveChanges();

            string senderName = GetSenderNickName(mes.SenderGuid);

            return mes.ToUIHubMessage(senderName);
        }

        UIHubMessage MakeMessageRead(int messageId)
        {
            Message message = context.Messages.FirstOrDefault(m => m.Id == messageId);
            if(message != null)
            {
                message.Status = MessageStatus.Read;
                context.SaveChanges();
                return new UIHubMessage
                {
                    Id = message.Id,
                    ReceiverGuid = message.ReceiverGuid,
                    SenderGuid = message.SenderGuid
                };
            }
            return null;
        }
        #endregion

        #region Public Methods FromClient
        public void SendMessageAboutOrder(OrderHubMessage Data)
        {
            UIHubMessage data = SendMessage(Data);
            
            OnMessageSend(data);
        }

        #endregion

        #region Методы вызываемые на клиенте

        public void OnMessageSend(UIHubMessage message)
        {
            //получаем группу пользователя
            string groupName = GetGroup(message.ReceiverGuid, message.SenderGuid);

            //че то херь какия то вроде есть метод расширения но ничего не работает
            ChatUIHubMessage m = new ChatUIHubMessage
            {
                Id = message.Id,
                Date = message.Date,
                Attachments = message.Attachments,
                SenderId = message.SenderGuid,
                NickName = message.NickName,
                Text = message.Text,
            };

            //Вызов на клиентах данный группы этого методы
            //!!!!Важно на клиенте нельзя принимать объект
            Clients.Group(groupName).onNewMessage(m);
        }

        public void OnMessageRead(int messageId)
        {
            UIHubMessage HubMes = MakeMessageRead(messageId);
            if (HubMes != null)
            {
                string groupName = GetGroup(HubMes.ReceiverGuid, HubMes.SenderGuid);
                Clients.Group(groupName).onMessageMarked(HubMes.Id);
            }
        }

        #endregion
    }
}