using HtmlExtensions.HtmlEntities;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Logic.Entities;

namespace Runtasker.Models.Notifications
{
    public class UINotificationModel
    {

        #region Конструкторы

        public UINotificationModel()
        {
            
        }

        public UINotificationModel(ForNotification notificationModel, HtmlLink actionLink)
        {
            Title = notificationModel.Title;
            Text = notificationModel.Text;
            ActionLink = actionLink;
        }

        #endregion

        #region Свойства
        public string Title { get; set; }

        public string Text { get; set; }

        public NotificationType Type { get; set; }

        public HtmlLink ActionLink { get; set; }
        #endregion
    }
}