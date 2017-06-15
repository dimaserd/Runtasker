using Runtasker.Logic.Workers.MessageWorker;
using System;

namespace Runtasker.Logic.Models.Messages
{
    public class ChatUIHubMessage
    {
        /// <summary>
        /// Id сообщения чтобы случайно не вывести то которое уже есть в представлении
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Текст сообщения 
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Идентификатор отправителя
        /// </summary>
        public string SenderId { get; set; }

        public string Attachments { get; set; }

        /// <summary>
        /// Имя отправителя
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// Дата отправки сообщения
        /// </summary>
        public DateTime Date { get; set; }

    }

    public static class ChatUIHubMessageExtensions
    {
        public static ChatUIHubMessage ToChatUIHubMessage(UIHubMessage message)
        {
            return new ChatUIHubMessage
            {
                Id = message.Id,
                Date = message.Date,
                Attachments = message.Attachments,
                SenderId = message.SenderGuid,
                NickName = message.NickName,
                Text = message.Text,
            };
        }
    }
}
