using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Notifications.CustomerOrderMethods;
using System;

namespace Runtasker.LocaleBuilders.Notification
{
    /// <summary>
    /// Содержит методы которые передают объекты для построения уведомлений
    /// в объекто содержиться локализованный заголовок, текст уведомления и текст кнопки
    /// </summary>
    public class CustomerOrderNotificationBuilder : UICultureSwitcher
    {
        public ForNotification AddedOrder(int orderId)
        {
            return new ForNotification
            {
                Title = string.Format(CustomerOrder.AddOrderTitleFormat, orderId),
                Text = CustomerOrder.AddOrderText,
                ActionBtnText = null
            };
        }
        
        public ForNotification AddedOnlineHelpOrder(string subjectName)
        {
            return new ForNotification
            {
                Title = string.Format(CustomerOrder.OnlineOrderCreatedTitleFormat, subjectName),
                Text = CustomerOrder.OnlineOrderCreatedText,
                ActionBtnText = null
            };
        }

        public ForNotification AddedDescription(int orderId)
        {
            return new ForNotification
            {
                Title = string.Format(CustomerOrder.AddDescriptionTitleFormat, orderId),
                Text = CustomerOrder.AddDescriptionText,
                ActionBtnText = null
            };
        }

        public ForNotification AddedNewFiles(int orderId)
        {
            return new ForNotification
            {
                Title = string.Format(CustomerOrder.AddFilesTitleFormat, orderId),
                Text = CustomerOrder.AddFilesText,
                ActionBtnText = null
            };
        }

        public ForNotification PaidFirstHalf(int orderId, DateTime orderFinishDate)
        {
            return new ForNotification
            {
                Title = string.Format(CustomerOrder.PaidFirstHalfTitleFormat, orderId),
                Text = $"{string.Format(CustomerOrder.PaidFirstHalfTextFormat, orderFinishDate.ToString("d MMM yyyy"))} " +
                        CustomerOrder.RuntaskerWish,
            };
        }

        public ForNotification PaidSecondHalf(int orderId, string saveSign)
        {
            return new ForNotification
            {
                Title = string.Format(CustomerOrder.PaidAnotherHalfTitleFormat, orderId),
                Text = $"{CustomerOrder.PaidAnotherHalfText} {CustomerOrder.RuntaskerWish}",
                ActionBtnText = $"{saveSign} {CustomerOrder.PaidAnotherHalfButtonText}"
            };
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

        public ForNotification OnlineHelpPaid(int orderId)
        {
            return new ForNotification
            {
                Title = string.Format(CustomerOrder.OnlineHelpPaidTitleFormat, orderId),
                Text = CustomerOrder.OnlineHelpPaidText,
                ActionBtnText = null
            };
        }
    }
}
