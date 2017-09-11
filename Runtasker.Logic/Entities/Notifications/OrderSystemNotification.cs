using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtasker.Logic.Entities.Notifications
{
    public enum OrderSystemNotificationType
    {


    }

    public class OrderSystemNotification
    {
        public string Id { get; set; }

        /// <summary>
        /// Использовать в пользовательском интерфейсе
        /// </summary>
        public bool UseInUI { get; set; }
    }
}
