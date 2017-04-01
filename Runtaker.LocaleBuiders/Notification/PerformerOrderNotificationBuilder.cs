using Extensions.Decimal;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Notifications.PerformerOrderMethods;
using System;

namespace Runtasker.LocaleBuilders.Notification
{
    public class PerformerOrderNotificationBuilder : UICultureSwitcher
    {
        public ForNotification AddedError(int orderId, string errorDesc)
        {
            return new ForNotification
            {
                Title = string.Format(PerformerOrderNotRes.AddedErrorTitleFormat, orderId, errorDesc),
                Text = PerformerOrderNotRes.AddedErrorText,
            };
        }

        public ForNotification Estimated(int orderId, decimal orderSum, string roubleSign)
        {
            return new ForNotification
            {
                Text = $"{string.Format(PerformerOrderNotRes.EstimatedTextFormat, orderId, orderSum.ToMoney(), roubleSign)} {PerformerOrderNotRes.RuntaskerWishes}",
                Title = string.Format(PerformerOrderNotRes.EstimatedTitleFormat, orderId),
                ActionBtnText = string.Format(PerformerOrderNotRes.EstimatedBtnTextFormat, (orderSum / 2).ToMoney(), roubleSign)
            };
        }

        public ForNotification StartedExecuting(int orderId, DateTime orderFinishDate)
        {
            return new ForNotification
            {
                Title = string.Format(PerformerOrderNotRes.StartedExecutingTitleFormat, orderId),
                Text = $"{string.Format(PerformerOrderNotRes.StartedExecutingTextFormat, orderFinishDate.ToString("d MMM yyyy"))} {PerformerOrderNotRes.RuntaskerWishes}",
            };
        }

        public ForNotification Executed(int orderId, decimal leftSum, string roubleSign)
        {
            return new ForNotification
            {
                Title = string.Format(PerformerOrderNotRes.ExecutedTitleFormat, orderId),
                Text = $"{string.Format(PerformerOrderNotRes.ExecutedTextFormat, leftSum.ToMoney(), roubleSign)} {PerformerOrderNotRes.RuntaskerWishes}",
                ActionBtnText = string.Format(PerformerOrderNotRes.ExecutedBtnTextFormat, leftSum.ToMoney(), roubleSign)
            };
        }
    }
}
