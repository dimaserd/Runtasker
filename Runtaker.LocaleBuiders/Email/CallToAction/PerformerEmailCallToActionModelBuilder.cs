using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Email.OrderPerformer;
using System;

namespace Runtasker.LocaleBuilders.Email.CallToAction
{
    public class PerformerEmailCallToActionModelBuilder : UICultureSwitcher
    {
        public ForEmailCallToAction EstimatedOnlineHelp(string customerName, DateTime helpDate, string subjectName, string costString, string roubleSign)
        {
            return new ForEmailCallToAction
            {
                Header = string.Format(PerformerOrderEmRes.EstimatedOnlineHelpSubjectFormat, subjectName),
                BigText = string.Format(PerformerOrderEmRes.EstimatedOnlineHelpTextFormat, customerName, helpDate.ToLongDateString(), subjectName, costString, roubleSign)
                + $" {PerformerOrderEmRes.RuntaskerWish}",
                ActionBtnText = PerformerOrderEmRes.EstimatedOnlineHelpBtnText
            };
        }

        public ForEmailCallToAction EstimatedOrder(string customerName, int orderId, decimal orderSum, string roubleSign)
        {
            return new ForEmailCallToAction
            {
                Header = string.Format(PerformerOrderEmRes.EstimatedSubjectFormat, customerName),
                BigText = $"{string.Format(PerformerOrderEmRes.EstimatedTextFormat, customerName, orderId, orderSum, roubleSign)} {PerformerOrderEmRes.RuntaskerWish}",
                ActionBtnText = PerformerOrderEmRes.EstimatedBtnText
            };
        }

        public ForEmailCallToAction FoundError(string customerName, int orderId)
        {
            return new ForEmailCallToAction
            {
                Header = string.Format(PerformerOrderEmRes.GreetingFormat, customerName),
                BigText = $"{string.Format(PerformerOrderEmRes.ErrorFoundTextFormat, customerName, orderId)} {PerformerOrderEmRes.RuntaskerWish}",
                ActionBtnText = PerformerOrderEmRes.ErrorFoundButtonText
            };
        }

        public ForEmailCallToAction ExecutedOrder(string customerName, int orderId, decimal leftSum, string roubleSign)
        {
            return new ForEmailCallToAction
            {
                Header = string.Format(PerformerOrderEmRes.GreetingFormat, customerName),
                BigText = $"{string.Format(PerformerOrderEmRes.ExecutedTextFormat, customerName, orderId, leftSum, roubleSign)} {PerformerOrderEmRes.RuntaskerWish}",
                ActionBtnText = PerformerOrderEmRes.ExecutedButtonText
            };
        }
    }
}
