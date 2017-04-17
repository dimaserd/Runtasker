using HtmlExtensions.HtmlEntities;
using Runtaker.LocaleBuiders.Notification;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Logic.Enumerations.Notifications.Anonymous;
using Runtasker.Models.Notifications;

namespace Runtasker.Statics.Notifications
{
    public static class UIGuestNotifications
    {
        public static UINotificationModel GetUINotification(AnonymousNotificationType? notifictionType)
        {
            switch(notifictionType)
            {
                case AnonymousNotificationType.TriedToOrderOnlineHelp:
                    //получить откуда-то нужно
                    ForNotification forNotification = GuestNotificationBuilderStatic.UserTriedToOrderOnlineHelp();

                    return new UINotificationModel()
                    {
                        Title = forNotification.Title,
                        Text = forNotification.Text,
                        ActionLink = new HtmlLink
                        (
                            hrefParam: "Account/Register",
                            textParam: forNotification.ActionBtnText,
                            buttonSizeParam: HtmlButtonSize.Large,
                            buttonTypeParam: HtmlButtonType.Success
                        )
                    };

                default:
                    return null;
            }
        }
    }
}