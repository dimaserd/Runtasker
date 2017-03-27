using Runtasker.LocaleBuilders.Enumerations;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Email.OrderCustomer;
using System;

namespace Runtasker.LocaleBuilders.Email.CallToAction
{
    public class CustomerEmailCallToActionModelBuilder : UICultureSwitcher
    {
        #region Public Methods

        public ForEmailCallToAction AddedOrder(string userName, int orderId)
        {
            return new ForEmailCallToAction
            {
                Header = string.Format(CustEmailNotRes.HelloFormat, userName),
                LittleHeader = null,
                BigText = $"{string.Format(CustEmailNotRes.AddedTextFormat, userName, orderId)} {CustEmailNotRes.RuntaskerWish}"
            };
        }

        public ForEmailCallToAction OnlineHelpApplied(string userName, string subjectName)
        {
            return new ForEmailCallToAction
            {
                Header = string.Format(CustEmailNotRes.HelloFormat, userName),
                LittleHeader = null,
                BigText = $"{string.Format(CustEmailNotRes.OnlineHelpAppliedTextFormat, userName, subjectName)} {CustEmailNotRes.RuntaskerWish}"
            };
        }

        public ForEmailCallToAction PaidFirstHalf(string userName, int orderId, DateTime orderFinishDate)
        {
            return new ForEmailCallToAction
            {
                Header = string.Format(CustEmailNotRes.HelloFormat, userName),
                BigText = string.Format(CustEmailNotRes.FirstPaidTextFormat, userName, orderId, orderFinishDate.ToString("d MMM yyyy"))
                         + $" {CustEmailNotRes.RuntaskerWish}",
            };
        }

        public ForEmailCallToAction DownloadedSolution(string userName, int orderId)
        {
            return new ForEmailCallToAction
            {
                Header = string.Format(CustEmailNotRes.HelloFormat, userName),
                BigText = string.Format(CustEmailNotRes.DownloadedTextFormat, userName, orderId)
                       + $" {CustEmailNotRes.RuntaskerWish}",
                ActionBtnText = CustEmailNotRes.DownloadedBtnText
            };
        }

        #endregion
    }
}
