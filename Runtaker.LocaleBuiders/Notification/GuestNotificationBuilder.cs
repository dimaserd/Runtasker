using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Notifications.GuestNotifications;

namespace Runtaker.LocaleBuiders.Notification
{
    /// <summary>
    /// Пока именнуй их через статики чтобы отличать от предыдущих моделей
    /// в дальнейшем они все классы выдающие ресурсы должны стать статическими
    /// </summary>
    public static class GuestNotificationBuilderStatic
    {
        public static ForNotification UserTriedToOrderOnlineHelp()
        {
            return new ForNotification
            {
                Title = GuestNotRes.UserTriedToOrderOnlineHelpTitle,
                Text = GuestNotRes.UserTriedToOrderOnlineHelpText,
                ActionBtnText = GuestNotRes.UserTriedToOrderOnlineHelpBtnText
            };
        }
    }
}
