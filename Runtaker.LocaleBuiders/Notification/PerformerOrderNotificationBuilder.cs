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
            switch(UICultureName)
            {
                case "ru-RU":
                    return new ForNotification
                    {
                        Title = $"{PerformerOrderNotRes.AddedErrorTitle}{orderId} {errorDesc}.",
                        Text = $"{PerformerOrderNotRes.AddedErrorText}",
                    };

                default:
                    return new ForNotification
                    {
                        Title = $"{PerformerOrderNotRes.AddedErrorTitle}{orderId} {errorDesc}.",
                        Text = $"{PerformerOrderNotRes.AddedErrorText}",
                    };
            }
        }

        public ForNotification Estimated(int orderId, decimal orderSum, string roubleSign)
        {
            switch(UICultureName)
            {
                case "ru-RU":
                    return new ForNotification
                    {
                        Text = $"{PerformerOrderNotRes.EstimatedText1}{orderId} {PerformerOrderNotRes.EstimatedText2} "
                            + $"{orderSum.ToMoney()}{roubleSign}. {PerformerOrderNotRes.EstimatedText3}. {PerformerOrderNotRes.RuntaskerWishes}",
                        Title = PerformerOrderNotRes.EstimatedTitle1,
                        ActionBtnText = $"{PerformerOrderNotRes.EstimatedBtnText} {(orderSum / 2).ToMoney()}{roubleSign}"
                    };

                default:
                    return new ForNotification
                    {
                        Text = $"{PerformerOrderNotRes.EstimatedText1}{orderId} {PerformerOrderNotRes.EstimatedText2} "
                        + $"{orderSum.ToMoney()}{roubleSign}. {PerformerOrderNotRes.EstimatedText3}. {PerformerOrderNotRes.RuntaskerWishes}",
                        Title = PerformerOrderNotRes.EstimatedTitle1,
                        ActionBtnText = $"{PerformerOrderNotRes.EstimatedBtnText} {(orderSum / 2).ToMoney()}{roubleSign}"
                    };
            }    
        }

        public ForNotification StartedExecuting(int orderId, DateTime orderFinishDate)
        {
            switch(UICultureName)
            {
                case "ru-RU":
                    return new ForNotification
                    {
                        Title = $"{PerformerOrderNotRes.StartedExecutingTitle1}{orderId} {PerformerOrderNotRes.StartedExecutingTitle2}",
                        Text = $"{PerformerOrderNotRes.StartedExecutingText1} {orderFinishDate.ToString("d MMM yyyy")} {PerformerOrderNotRes.StartedExecutingText2} "
                        + PerformerOrderNotRes.RuntaskerWishes,
                    };

                default:
                    return new ForNotification
                    {
                        Title = $"{PerformerOrderNotRes.StartedExecutingTitle1}{orderId} {PerformerOrderNotRes.StartedExecutingTitle2}",
                        Text = $"{PerformerOrderNotRes.StartedExecutingText1} {orderFinishDate.ToString("d MMM yyyy")} {PerformerOrderNotRes.StartedExecutingText2} "
                        + PerformerOrderNotRes.RuntaskerWishes,
                    };
            }
            
        }

        public ForNotification Executed(int orderId, decimal leftSum, string roubleSign)
        {
            switch(UICultureName)
            {
                case "ru-RU":
                    return new ForNotification
                    {
                        Title = $"{PerformerOrderNotRes.ExecutedTitle1}{orderId}.",
                        Text = $"{PerformerOrderNotRes.ExecutedText1} {leftSum.ToMoney()}{roubleSign} {PerformerOrderNotRes.ExecutedText2}",
                        ActionBtnText = $"{PerformerOrderNotRes.ExecutedBtnText} {leftSum.ToMoney()}{roubleSign}"
                    };

                default:
                    return new ForNotification
                    {
                        Title = $"{PerformerOrderNotRes.ExecutedTitle1}{orderId}.",
                        Text = $"{PerformerOrderNotRes.ExecutedText1}",
                        ActionBtnText = $"{PerformerOrderNotRes.ExecutedBtnText} {leftSum.ToMoney()}{roubleSign}"
                    };
            }
        }
    }
}
