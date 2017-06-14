namespace Runtasker.Logic.Models.Messages
{
    public class HubMessage
    {
        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Идентификатор отправителя
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// Идентификатор получателя
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
        /// Для прикрепления ссылки на архив
        /// </summary>
        public string Attachments { get; set; }

    }
}
