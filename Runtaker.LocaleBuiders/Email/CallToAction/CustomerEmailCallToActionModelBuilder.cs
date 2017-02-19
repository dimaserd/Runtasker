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
            switch(UICultureName)
            {
                case "ru-RU":
                    return new ForEmailCallToAction
                    {
                        Header = $"{CustEmailNotRes.Hello} {userName}!",
                        LittleHeader = null,
                        BigText = $"{CustEmailNotRes.Dear} {userName}, {CustEmailNotRes.AddedText1} №{orderId}! " +
                        $"{CustEmailNotRes.AddedText2} {CustEmailNotRes.RuntaskerWish}"
                    };
                //english checked
                default:
                    return new ForEmailCallToAction
                    {
                        Header = $"{CustEmailNotRes.Hello} {userName}!",
                        LittleHeader = null,
                        BigText = $"{CustEmailNotRes.Dear} {userName}, {CustEmailNotRes.AddedText1}{orderId} " +
                        $"{CustEmailNotRes.AddedText2} {CustEmailNotRes.RuntaskerWish}"
                    };
            }
        }

        public ForEmailCallToAction PaidFirstHalf(string userName, int orderId, DateTime orderFinishDate)
        {
            switch(UICultureName)
            {
                case "ru-RU":
                    return new ForEmailCallToAction
                    {
                        Header = $"{CustEmailNotRes.Hello} {userName}!",
                        BigText = $"{CustEmailNotRes.Dear} {userName}, {CustEmailNotRes.FirstPaidText1} №{orderId}. " +
                        $"{CustEmailNotRes.FirstPaidText2} {orderFinishDate.ToString("d MMM yyyy")} " +
                        $"{CustEmailNotRes.FirstPaidText3} {CustEmailNotRes.RuntaskerWish}",
                    };
                //english checked
                default:
                    return new ForEmailCallToAction
                    {
                        Header = $"{CustEmailNotRes.Hello} {userName}!",
                        BigText = $"{CustEmailNotRes.Dear} {userName}, {CustEmailNotRes.FirstPaidText1}{orderId}. " +
                        $"{CustEmailNotRes.FirstPaidText2} {orderFinishDate.ToString("d MMM yyyy")} " +
                        $"{CustEmailNotRes.FirstPaidText3} {CustEmailNotRes.RuntaskerWish}",
                    };
            }
            
        }

        public ForEmailCallToAction DownloadedSolution(string userName, int orderId)
        {
            switch(UICultureName)
            {
                case "ru-RU":
                    return new ForEmailCallToAction
                    {
                        Header = $"{CustEmailNotRes.Hello} {userName}!",
                        BigText = $"{CustEmailNotRes.Dear} {userName}! {CustEmailNotRes.DownloadedText1} №{orderId} "
                       + $"{CustEmailNotRes.DownloadedText2} {CustEmailNotRes.RuntaskerWish}",
                        ActionBtnText = CustEmailNotRes.DownloadedBtnText
                    };

                default:
                    return new ForEmailCallToAction
                    {
                        Header = $"{CustEmailNotRes.Hello} {userName}!",
                        BigText = $"{CustEmailNotRes.Dear} {userName}! {CustEmailNotRes.DownloadedText1} №{orderId} "
                       + $"{CustEmailNotRes.DownloadedText2} {CustEmailNotRes.RuntaskerWish}",
                        ActionBtnText = CustEmailNotRes.DownloadedBtnText
                    };
            }
           
        }

        #endregion
    }
}
