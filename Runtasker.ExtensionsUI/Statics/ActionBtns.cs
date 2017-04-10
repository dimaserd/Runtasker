using HtmlExtensions.StaticRenderers;
using Runtasker.ExtensionsUI.UIExtensions.Orders;
using Runtasker.Resources.UIExtensions.Orders;
using System;

namespace Runtasker.ExtensionsUI.Statics
{
    public static class ActionBtns
    {
        public static HtmlActionButtonLink RatingBtn(int orderId, string btnClass)
        {
            return new HtmlActionButtonLink
            (
                buttonText: string.Format(OrderEntityRes.RatingWorkFormat, GISigns.Star),
                buttonLink: $"Orders/Rating/{orderId}",
                //для джаваскрипт функции чтобы вызвать это все в модале
                htmlAttributes: new { id = $"{orderId}", @class = $"{btnClass} rateLink" }
            );
        }

        #region Для заказа с ошибкой

        public static HtmlActionButtonLink AddDescriptionBtn(int orderId, string btnClass)
        {
            return new HtmlActionButtonLink
                        (
                            buttonLink: $"/Orders/AddDescription/{orderId}",
                            buttonText: OrderEntityRes.AddDescription,
                            buttonClass: btnClass
                        );
        }

        public static HtmlActionButtonLink AddFilesBtn(int orderId, string btnClass)
        {
            return new HtmlActionButtonLink
                        (
                            buttonLink: $"/Orders/AddFiles/{orderId}",
                            buttonText: OrderEntityRes.AddFiles,
                            buttonClass: btnClass
                        );
        }

        #endregion

        #region Кнопки для оплаты заказа
        public static HtmlActionButtonLink PayFirstHalfBtn(int orderId, string sumToPayString, string btnClass)
        {
            return new HtmlActionButtonLink
                (
                    buttonLink: $"/Orders/PayHalf/{orderId}",
                    buttonText: string.Format(OrderEntityRes.PayFormat, FASigns.CreditCard, sumToPayString, HtmlSigns.Rouble),
                    buttonClass: btnClass
                );
        }

        public static HtmlActionButtonLink PaySecondHalfBtn(int orderId, string sumToPayString, string btnClass)
        {
            return new HtmlActionButtonLink
                    (
                       buttonLink: $"/Orders/PayAnotherHalf/{orderId}",
                       buttonText: string.Format(OrderEntityRes.PayFormat, FASigns.CreditCard, sumToPayString, HtmlSigns.Rouble),
                       buttonClass: btnClass
                    );
        }
        
        public static HtmlActionButtonLink PayOnlineHelpBtn(int orderId, string sumToPayString, string btnClass)
        {
            return new HtmlActionButtonLink
                (
                    buttonLink: $"/Orders/PayOnlineHelp/{orderId}",
                    buttonText: string.Format(OrderEntityRes.PayFormat, FASigns.CreditCard, sumToPayString, HtmlSigns.Rouble),
                    buttonClass: btnClass
                );
        }
        #endregion

        public static HtmlActionButtonLink ChatBtn(int orderId, int unreadCount, string btnClass)
        {
            return new HtmlActionButtonLink
                (
                    buttonLink: "#",
                    buttonText: string.Format(OrderEntityRes.ChatAboutOrderFormat, GISigns.Envelope, GISigns.Count(unreadCount)),
                    //для вызова функции javacript которая откроет чат в модале
                    htmlAttributes: new { id = orderId, @class = $"{btnClass} orderChat" }
                )
        }

        public static HtmlActionButtonLink EstimationBtn(string btnClass)
        {
            return new HtmlActionButtonLink
                (
                    buttonLink: "#",
                    buttonText: OrderEntityRes.Estimation,
                    buttonClass: btnClass,
                    disabled: true
                );
        }

        public static HtmlActionButtonLink ExecutingBtn(string btnClass)
        {
            return new HtmlActionButtonLink
                (
                    buttonLink: "#",
                    buttonText: OrderEntityRes.Executing,
                    buttonClass: btnClass,
                    disabled: true
                );
        }

        public static HtmlActionButtonLink FinishedBtn(string btnClass)
        {
            return new HtmlActionButtonLink
                (
                    buttonLink: "#",
                    buttonText: string.Format(OrderEntityRes.FinishedFormat, null),
                    buttonClass: btnClass,
                    disabled: true
                );
        }

        public static HtmlActionButtonLink WaitingForHelpEventBtn(DateTime helpDate, string btnClass)
        {
            return new HtmlActionButtonLink
                    (
                        buttonLink: "#",
                        disabled: true,
                        buttonText: string.Format(OrderEntityRes.WaitingForHelpEventFormat, helpDate.ToShortDateString()),
                        buttonClass: btnClass
                    );
        }

        public static HtmlActionButtonLink DownloadSolutionBtn(int orderId, string btnClass)
        {
            return new HtmlActionButtonLink
                (
                    buttonLink: $"File/DownloadSolution/{orderId}",
                    buttonText: string.Format(OrderEntityRes.DownloadSolutionFormat, FASigns.Download),
                    buttonClass: btnClass
                );
        }
    }
}
