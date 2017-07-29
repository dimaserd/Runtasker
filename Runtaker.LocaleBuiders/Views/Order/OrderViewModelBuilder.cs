using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Views.Home.KnowPrice;
using Runtasker.Resources.Views.Orders.Create;
using Runtasker.Resources.Views.Orders.OnlineHelp;
using Runtasker.Resources.Views.Orders.Pay;
using Runtasker.Resources.Views.Orders.Rating;

namespace Runtasker.LocaleBuilders.Views.Order
{
    /// <summary>
    /// Возвращает локализованные модели для представлений контроллера Orders
    /// </summary>
    public static class OrderViewModelBuilder
    {
        public static LocaleViewModel RatingView(int orderId)
        {
            LocaleViewModel result = new LocaleViewModel();

            

            result.Add("LeaveReviewBtnText", string.Format(RatingRes.LeaveReviewFormat, orderId));
            result.Add("CommentError", RatingRes.CommentError);
            result.Add("RatingError", RatingRes.RatingError);
            result.Add("CancelBtnText", RatingRes.Cancel);
            result.Add("SendBtnText", RatingRes.Send);
            result.Add("CommentPlaceHolder", RatingRes.PlaceHolder);


            return result;
        }

        #region Создание заказа
        public static LocaleViewModel CreateOrderView()
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", Create.Title);
            result.Add("HomeNav", Create.HomeNav);
            result.Add("MyOrdersNav", Create.MyOrdersNav);
            result.Add("CreateNav", Create.CreateNav);
            result.Add("Header", Create.Header);
            result.Add("MiniHeader", Create.MiniHeader);
            result.Add("CreateOrder", Create.CreateOrder);
            result.Add("FileUploading", Create.FileUploading);
            result.Add("BackToList", Create.BackToList);

            //для подсказочного уведомления
            result.Add("OnlineHelpAlertTitle", Create.OnlineHelpAlertTitle);
            result.Add("OnlineHelpAlertText", Create.OnlineHelpAlertText);
            result.Add("OnlineHelpBtnText", Create.OnlineHelpBtnText);

            //Стащено с другого файла ресурсов
            result.Add("NameError", KnowPriceRes.NameError);
            result.Add("EmailError", KnowPriceRes.EmailError);
            result.Add("DescriptionError", KnowPriceRes.DescriptionError);
            result.Add("OtherSubjectError", KnowPriceRes.OtherSubjectError);

            

            return result;
        }

        public static LocaleViewModel OnlineHelpView()
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", OnlineHelp.Title);
            result.Add("DescriptionPlaceholder", OnlineHelp.DescriptionPlaceholder);

            result.Add("HomeNav", OnlineHelp.HomeNav);
            result.Add("MyOrdersNav", OnlineHelp.MyOrdersNav);
            result.Add("ActiveNav", OnlineHelp.ActiveNav);
            result.Add("SubmitBtnText", OnlineHelp.SubmitBtnText);
            result.Add("Header", OnlineHelp.Header);
            result.Add("HelpAlertTitle", OnlineHelp.HelpAlertTitle);
            result.Add("HelpAlertText", OnlineHelp.HelpAlertText);
            //не сделанное или стащенное

            result.Add("MiniHeader", Create.MiniHeader);
            
            result.Add("CreateOrder", Create.CreateOrder);
            result.Add("FileUploading", Create.FileUploading);
            result.Add("BackToList", Create.BackToList);
            //Стащено с другого файла ресурсов
            result.Add("NameError", KnowPriceRes.NameError);
            result.Add("EmailError", KnowPriceRes.EmailError);
            result.Add("DescriptionError", KnowPriceRes.DescriptionError);
            result.Add("OtherSubjectError", KnowPriceRes.OtherSubjectError);

            
            return result;
        }
        #endregion

        #region Оплата

        public static LocaleViewModel PayHalfView(int orderId, string paySign)
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", string.Format(PayRes.PayHalfFormat, orderId));
            result.Add("HomeNav", PayRes.HomeNav);
            result.Add("MyOrdersNav", PayRes.MyOrdersNav);
            result.Add("ActiveNav", PayRes.Pay);
            result.Add("PayBtnInnerHtml", string.Format(PayRes.PayBtnFormat, paySign));

            result.Add("HelpTextFormat", PayRes.PayHelpTextFormat);
            return result;
        }


        public static LocaleViewModel PayAnotherHalfView(int orderId, string sumToPay, string roubleSign)
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", string.Format(PayRes.PaySecondHalfFormat, orderId));
            result.Add("PayRoublesHtml", string.Format(PayRes.PayRoublesFormat, sumToPay, roubleSign));

            //элементы навигации
            result.Add("HomeNav", PayRes.HomeNav);
            result.Add("MyOrdersNav", PayRes.MyOrdersNav);
            result.Add("ActiveNav", PayRes.Pay);

            result.Add("ActionBtnText", PayRes.Pay);

            return result;
        }
        
        public static LocaleViewModel PayOnlineHelp(string sumToPay, string roubleSign)
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", PayRes.PayOnlineHelpTitle);
            result.Add("PayRoublesHtml", string.Format(PayRes.PayRoublesFormat, sumToPay, roubleSign));

            //элементы навигации
            result.Add("HomeNav", PayRes.HomeNav);
            result.Add("MyOrdersNav", PayRes.MyOrdersNav);
            result.Add("ActiveNav", PayRes.Pay);

            result.Add("ActionBtnText", PayRes.Pay);

            return result;
        }
        #endregion
    }
}
