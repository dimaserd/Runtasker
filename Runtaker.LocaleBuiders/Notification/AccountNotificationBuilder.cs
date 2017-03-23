using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Notifications.AccountMethods;

namespace Runtasker.LocaleBuilders.Notification
{
    public class AccountNotificationBuilder
    {
        public ForNotification UserConfirmedEmail(int bonus, string roubleSign, string AddOrderSign)
        {
            return new ForNotification
            {
                Title = AccountRes.RegisteredTitle,
                Text = string.Format(AccountRes.RegisteredTextFormat, bonus, roubleSign)
                        + $" {AccountRes.RuntaskerWish}",
                ActionBtnText = $"{AddOrderSign} {AccountRes.RegisteredButtonText}"
            };
        }
    }
}
