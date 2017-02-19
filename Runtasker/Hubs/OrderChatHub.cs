using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.MessageWorker;
using System.Linq;
using System;
using Microsoft.AspNet.SignalR.Hubs;
using Runtasker.Logic.Workers.Files;

namespace Runtasker.Hubs
{
    [HubName("aboutOrderChatHub")]
    public class AboutOrderChatHub : ChatHubBase
    {
        #region Constructors
        public AboutOrderChatHub()
        {
            Filer = new SuperFileWorker(context);
        }
        #endregion

        #region Properties
        SuperFileWorker Filer { get; set; }
        #endregion

        #region Implemented Methods

        UIHubMessage SendMessage(object message)
        {
            OrderHubMessage m = message as OrderHubMessage;
            Message mes = new Message
            {
                Date = DateTime.Now,
                Type = MessageType.AboutOrder,
                Mark = m.OrderId.ToString(),
                Text = m.Text,
                //Links in Text must be wrapped and checked for html tags
                AttachmentId = Filer.AttachmentWorker.GetToMessage(m.Attachments),
                //Attachments get throw file
                ReceiverGuid = m.ToGuid,
                SenderGuid = m.UserGuid,
                Status = MessageStatus.New,
                OrderId = m.OrderId,
            };
            context.Messages.Add(mes);
            context.SaveChanges();
            return new UIHubMessage
            {
                /*For group building*/
                SenderGuid = mes.SenderGuid,
                ReceiverGuid = mes.ReceiverGuid,
                /*/For group building*/

                /*For building message in Chat via JavaScript*/
                Id = mes.Id,
                Attachments = mes.AttachmentId,
                Date = mes.Date,
                NickName = GetSenderNickName(mes.SenderGuid),
                Text = mes.Text
                /*/For building message in Chat via JavaScript*/
            };
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

        #region Public Methods OnClient

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