using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
