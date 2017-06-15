using Extensions.String;
using Runtasker.Logic.Entities;
using System;

namespace Runtasker.Logic.Models.Messages
{
    public class OrderHubMessage : HubMessage
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        public int OrderId { get; set; }

    }

    public static class OrderHubMessageExtensions
    {
        public static OrderHubMessage ToOrderHubMessage(this Entities.Message message, string ChatterAName, string ChatterAId, string ChatterBName)
        {
            OrderHubMessage result = new OrderHubMessage
            {
                OrderId = message.OrderId.Value,
                Attachments = message.AttachmentId,
                ReceiverId = message.ReceiverGuid,
                SenderId = message.SenderGuid,
                Text = message.Text,
                IsRead = message.Status == MessageStatus.Read,
                //дата приводится к данному формату
                Date = message.Date.ToString("dd'/'MM'/'yyyy HH:mm:ss"),
                Id = message.Id
            };

            //если он отправитель
            if(ChatterAId == message.SenderGuid)
            {
                result.SenderName = ChatterAName;
                result.ReceiverName = ChatterBName;
            }
            else
            {
                result.SenderName = ChatterBName;
                result.ReceiverName = ChatterAName;
            }

            return result;
        }



        public static Entities.Message ToMessage(this OrderHubMessage m, string attachmentId)
        {
            return new Entities.Message
            {
                Date = DateTime.Now,
                Type = MessageType.AboutOrder,
                Mark = m.OrderId.ToString(),
                Text = m.Text.StripHTML(),
                //Links in Text must be wrapped and checked for html tags
                AttachmentId = attachmentId,
                //Attachments get throw file
                ReceiverGuid = m.ReceiverId,
                SenderGuid = m.SenderId,
                Status = MessageStatus.New,
                OrderId = m.OrderId,
            };
        }
    }
}
