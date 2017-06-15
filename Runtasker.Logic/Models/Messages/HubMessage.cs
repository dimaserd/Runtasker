namespace Runtasker.Logic.Models.Messages
{
    public class HubMessage
    {
        /// <summary>
        /// ИДентификатор сообщения должен быть уникальным в чате чтобы избежать 
        /// двойных построений
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Отформатированная дата
        /// </summary>
        public string FormattedDate { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Идентификатор отправителя
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// Флаг указывающий на то, является ли сообщение прочитанным
        /// </summary>
        public bool IsRead { get; set; }

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

        /// <summary>
        /// Отформатированная Дата отправки сообщения
        /// </summary>
        public string Date { get; set; }

    }
}
