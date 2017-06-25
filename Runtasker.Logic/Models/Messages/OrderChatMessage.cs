using System;
using System.Collections.Generic;
using System.Web;

namespace Runtasker.Logic.Models.Messages
{
    /// <summary>
    /// Класс описывающий сообщения о заказе между исполнителем и заказчиком
    /// </summary>
    public class OrderChatMessage : ChatMessage
    {
        public int OrderId { get; set; }

        public IEnumerable<HttpPostedFileBase> Attachments {get;set;}
    }

    public static class OrderChatMessageExtensions
    {
        public static Entities.Message ToMessage(this OrderChatMessage mes)
        {
            return new Entities.Message
            {
                Id = 0,
                Date = DateTime.Now,
                AttachmentId = mes.AttachmentId,
                Mark = null,
                OrderId = mes.OrderId,
                ReceiverGuid = mes.ReceiverId,
                SenderGuid = mes.SenderId,
                Status = Entities.MessageStatus.New,
                Text = mes.Text,
                Type = Entities.MessageType.AboutOrder,
            };

        }

        public static OrderChatMessage ToOrderChatMessage(this Entities.Message message, string chatterAName, string chatterBName, string chatterAId)
        {
            OrderChatMessage result = new OrderChatMessage
            {
                AttachmentId = message.AttachmentId,
                OrderId = message.OrderId.Value,
                ReceiverId = message.ReceiverGuid,
                SenderId = message.SenderGuid,
                Text = message.Text,
                FormattedDate = message.Date.ToShortDateString(),
                IsRead = (message.Status == Entities.MessageStatus.Read),
            };

            //если первый является отправителем
            if(chatterAId == message.SenderGuid)
            {
                result.SenderName = chatterAName;
                result.ReceiverName = chatterBName;
            }
            else
            {
                result.SenderName = chatterBName;
                result.ReceiverName = chatterAName;
            }

            return result;
        }
    }
}
