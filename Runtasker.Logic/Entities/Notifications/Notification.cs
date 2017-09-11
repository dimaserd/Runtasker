
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runtasker.Logic.Entities
{
    /// <summary>
    /// Тип того о чем сообщается в данном уведомлении
    /// </summary>
    public enum NotificationAboutType
    {
        /// <summary>
        /// Обычное уведомление (не требует перелогина пользователя)
        /// </summary>
        Ordinary,

        /// <summary>
        /// Уведомление о балансе (требует перелогина пользователя для изменения баланса в UI)
        /// </summary>
        Balance,

        /// <summary>
        /// Пустой для обновления куков для пользователя
        /// </summary>
        EmptyForRefresh,
    }

    public enum NotificationType
    {
        [Display(Name = "alert-success")]
        Success,
        [Display(Name = "alert-info")]
        Info,
        [Display(Name = "alert-warning")]
        Warning,
        [Display(Name = "alert-danger")]
        Danger
    }

    /// <summary>
    /// Статус уведомления
    /// </summary>
    public enum NotificationStatus
    {
        /// <summary>
        /// Не просмотрено
        /// </summary>
        Unseen,
        /// <summary>
        /// Просмотрено
        /// </summary>
        Seen
    }

    

    /// <summary>
    /// Сущность описывающая уведомление
    /// </summary>
    public class Notification
    {
        #region Конструктор

        public Notification()
        {
            DefaultSettings();
        }
        
        void DefaultSettings()
        {
            Id = Guid.NewGuid().ToString();
            Status = NotificationStatus.Unseen;
            CreationDate = DateTime.Now;
        }

        #endregion

        #region Свойста
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        //[ForeignKey("User")]
        public string UserGuid { get; set; }
        //public virtual ApplicationUser User { get; set; }

        public NotificationType Type { get; set; }

        /// <summary>
        /// Статус уведомления
        /// </summary>
        public NotificationStatus Status { get; set; }

        /// <summary>
        /// О чем сообщается в данном уведомлении
        /// </summary>
        public NotificationAboutType AboutType { get; set; }

        /// <summary>
        /// Заголовок уведомления
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текст уведомления
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата создания 
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Заготовленная Html ссылка в дальнейшем нужно будет избавляться от этого свойства
        /// </summary>
        public string Link { get; set; }
        #endregion
    }
}
