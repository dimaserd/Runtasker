using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runtasker.Logic.Entities.Base;

namespace Runtasker.Logic.Entities.Notifications
{
    public enum CustomerSystemNotificationType
    {
        CustomerRegistered,
    }

    public class CustomerSystemNotification
    {
        public CustomerSystemNotificationType Type { get; set; }

        /// <summary>
        /// Использовать в пользовательском интерфейсе
        /// </summary>
        public bool UseInUI { get; set; }

    }

    public class CustomerSystemNotificationLangClarification : LanguageClarificationBase
    {
        /// <summary>
        /// Заголовок уведомления
        /// </summary>
        public string TitleFormat { get; set; }

        /// <summary>
        /// Текст уведомления
        /// </summary>
        public string TextFormat { get; set; }
    }

}
