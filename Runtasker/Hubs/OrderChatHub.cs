using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.MessageWorker;
using System.Linq;
using System;
using Microsoft.AspNet.SignalR.Hubs;
using Runtasker.Logic.Workers.Files;
using Extensions.String;

namespace Runtasker.Hubs
{
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
            Message mes = new Message
            {
                Date = DateTime.Now,
                Type = MessageType.AboutOrder,
                Mark = m.OrderId.ToString(),
                Text = m.Text.StripHTML(),
                //Links in Text must be wrapped and checked for html tags
                AttachmentId = Filer.AttachmentWorker.GetToMessage(m.Attachments),
                //Attachments get throw file
                ReceiverGuid = m.ToGuid,
                SenderGuid = m.UserGuid,
                Status = MessageStatus.New,
                OrderId = m.OrderId,
            };

            //добавление сообщения в базу
            context.Messages.Add(mes);
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
            //Calling a function on client
            OnMessageSend(data);
        }


        public void MessageIsRead(int Id)
        {
            
        }



        #endregion

        #region Методы вызываемые на клиенте

        public void OnMessageSend(UIHubMessage message)
        {
            //get here the group abd send message
            string groupName = GetGroup(message.ReceiverGuid, message.SenderGuid);
            //TODO make constructor to ChatUIHubMessage from type UIHubMessage
            ChatUIHubMessage m = new ChatUIHubMessage
            {
                Id = message.Id,
                Date = message.Date,
                Attachments = message.Attachments,
                SenderGuid = message.SenderGuid,
                NickName = message.NickName,
                Text = message.Text,
            };
            //Methods don't exist in javascript
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