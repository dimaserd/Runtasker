using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Notifications.CustomerOrderMethods;
using System;

namespace Runtasker.LocaleBuilders.Notification
{
    public class CustomerOrderNotificationBuilder : UICultureSwitcher
    {
        public ForNotification AddedOrder(int orderId)
        {
            return new ForNotification
            {
                Title = $"{CustomerOrder.AddOrderTitle1}{orderId} "
                + CustomerOrder.AddOrderTitle2,
                Text = CustomerOrder.AddOrderText,
                ActionBtnText = null
            };
        }

        public ForNotification AddedDescription(int orderId)
        {
            return new ForNotification
            {
                Title = $"{CustomerOrder.AddDescriptionTitle}{orderId}!",
                Text = CustomerOrder.AddDescriptionText,
                ActionBtnText = null
            };
        }

        public ForNotification AddedNewFiles(int orderId)
        {
            return new ForNotification
            {
                Title = $"{CustomerOrder.AddFilesTitle}{orderId}!",
                Text = CustomerOrder.AddFilesText,
                ActionBtnText = null
            };
        }

        public ForNotification PaidFirstHalf(int orderId, DateTime orderFinishDate)
        {
            switch (UICultureName)
            {
                case "ru-RU":
                    return new ForNotification
                    {
                        Title = $"{CustomerOrder.PaidFirstHalfTitle1}{orderId}.",
                        Text = $"{CustomerOrder.PaidFirstHalfText1} {orderFinishDate.ToString("d MMM yyyy")} " +
                        $"{CustomerOrder.PaidFirstHalfText2} {CustomerOrder.RuntaskerWish}",
                    };

                default:
                    return new ForNotification
                    {
                        Title = $"{CustomerOrder.PaidFirstHalfTitle1}{orderId}.",
                        Text = $"{CustomerOrder.PaidFirstHalfText1} {orderFinishDate.ToString("d MMM yyyy")} " +
                        $"{CustomerOrder.PaidFirstHalfText2} {CustomerOrder.RuntaskerWish}",
                    };
            }
        }

        public ForNotification PaidSecondHalf(int orderId, string saveSign)
        {
            switch (UICultureName)
            {
                case "ru-RU":
                    return new ForNotification
                    {
                        Title = $"{CustomerOrder.PaidAnotherHalfTitle1}{orderId}!",
                        Text = CustomerOrder.PaidAnotherHalfText,
                        ActionBtnText = $"{saveSign} {CustomerOrder.PaidAnotherHalfButtonText}"
                    };

                default:
                    return new ForNotification
                    {
                        Title = $"{CustomerOrder.PaidAnotherHalfTitle1}{orderId} "
                        + $"{CustomerOrder.PaidAnotherHalfTitle2}!",
                        Text = CustomerOrder.PaidAnotherHalfText,
                        ActionBtnText = $"{saveSign} {CustomerOrder.PaidAnotherHalfButtonText}"
                    };
            }

            
        }

        public ForNotification RatedSolution(int orderRating)
        {
            return new ForNotification
            {
                Title = CustomerOrder.RatingTitle,
                Text = (orderRating <= 3) ?
                $"{CustomerOrder.BadRatingText1} " :
                $"{CustomerOrder.GoodRatingText1} "
                + CustomerOrder.RatingText2
            };
        }


    }
}
