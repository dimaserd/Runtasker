using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Notifications.AccountMethods;

namespace Runtasker.LocaleBuilders.Notification
{
    public class AccountNotificationBuilder
    {
        public ForNotification UserConfirmedEmail(int Bonus, string roubleSign, string AddOrderSign)
        {
            return new ForNotification
            {
                Title = AccountRes.RegisteredTitle,
                Text = $"{AccountRes.RegisteredText1}! {AccountRes.RegisteredText2} "
                        + AccountRes.RuntaskerWish,
                ActionBtnText = $"{AddOrderSign} {AccountRes.RegisteredButtonText}"
            };
        }
    }
}
