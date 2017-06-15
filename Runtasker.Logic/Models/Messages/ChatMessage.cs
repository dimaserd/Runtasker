using System;

namespace Runtasker.Logic.Models.Messages
{

    /// <summary>
    /// Стандартный класс описывающий отправляемое сообщение
    /// </summary>
    public class ChatMessage
    {

        /// <summary>
        /// Идентификатор отправителя
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// Идентификатор полуяателя
        /// </summary>
        public string ReceiverId { get; set; }

        /// <summary>
        /// Имя отправителя
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// Имя получателя
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// Текст отправленного сообщения
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Ссылка для скачивания прикрепленных файлов к сообщению
        /// </summary>
        public string AttachmentId { get; set; }

        /// <summary>
        /// Форматированная дата
        /// </summary>
        public string FormattedDate { get; set; }

        /// <summary>
        /// Метка о прочитанности сообщения
        /// </summary>
        public bool IsRead { get; set; }

    }

    public static class ChatMessageExtensions
    {
        public static Entities.Message ToMessage(ChatMessage chatMessage)
        {
            return new Entities.Message
            {
                Id = 0,
                Date = DateTime.Now,
                //
                AttachmentId = chatMessage.AttachmentId,
                Mark = null,
                ReceiverGuid = chatMessage.ReceiverId,
                SenderGuid = chatMessage.SenderId,
                Status = Entities.MessageStatus.New,
                Text = chatMessage.Text,
                Type = Entities.MessageType.Ordinary,
            };
        }
    }
}
