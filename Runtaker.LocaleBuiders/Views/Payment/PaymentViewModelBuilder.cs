using Extensions.String;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Views.Payment.Index;
using Runtasker.Resources.Views.Payment.Robokassa;
using Runtasker.Resources.Views.Payment.Succeeded;
using Runtasker.Resources.Views.Payment.Yandex;
using Runtasker.Resources.Views.Payment.YandexKassa;

namespace Runtasker.LocaleBuilders.Views.Payment
{
    public class PaymentViewModelBuilder : UICultureSwitcher
    {
        public LocaleViewModel Index()
        {
            LocaleViewModel result = new LocaleViewModel();
            result.Add("ActionDesc", IndexRes.ActionDesc);
            result.Add("ActionDescMini", IndexRes.ActionDescMini);
            result.Add("Amount", IndexRes.Amount);
            result.Add("ErrorText", IndexRes.ErrorText);
            //result.Add("InfoText", IndexRes.InfoText);
            result.Add("InfoText", IndexRes.NewInfoText);

            result.Add("Title", IndexRes.Title);
            result.Add("NavHome", IndexRes.NavHome);
            result.Add("NavActive", IndexRes.NavActive);


            result.Add("HtmlTopUpViaRoboKassa", string.Format(IndexRes.TopUpFormat, IndexRes.TopUpToHide.WrapToSpan(new { @class = "hidden-xs" }), IndexRes.ViaRobokassa));
            result.Add("HtmlTopUpViaYandexMoney", string.Format(IndexRes.TopUpFormat, IndexRes.TopUpToHide.WrapToSpan(new { @class = "hidden-xs" }), IndexRes.ViaYandexMoney));
            result.Add("HtmlTopUpViaYandexKassa", string.Format(IndexRes.TopUpFormat, IndexRes.TopUpToHide.WrapToSpan(new { @class = "hidden-xs" }), IndexRes.ViaYandexKassa));

            
            return result;
        }

        public LocaleViewModel Robokassa(decimal amount, decimal withdrawAmount, string roubleSign)
        {
            LocaleViewModel result = new LocaleViewModel();
            result.Add("Title", RkassaRes.Title);
            result.Add("ActionDesc", string.Format(RkassaRes.ActionDescFormat, amount, roubleSign));
            result.Add("ActionDescMini", $"{RkassaRes.ActionDescMini}");
            result.Add("Info", string.Format(RkassaRes.InfoFormat, withdrawAmount, roubleSign));
            result.Add("BackToPM", RkassaRes.BackToPM);

            result.Add("NavHome", RkassaRes.NavHome);
            result.Add("NavActive", RkassaRes.NavActive);
            result.Add("NavPayments", RkassaRes.NavPayments);

            return result;
        }

        public LocaleViewModel Yandex()
        {
            LocaleViewModel result = new LocaleViewModel();
            result.Add("ActionDesc", YandexRes.ActionDesc);
            result.Add("ActionDescMini", YandexRes.ActionDescMini);
            result.Add("BackToPM", YandexRes.BackToPM);
            result.Add("AmountErrorText", YandexRes.AmountErrorText);
            result.Add("InfoText", YandexRes.InfoText);
            result.Add("Pay", YandexRes.Pay);
            result.Add("Amount", YandexRes.Amount);
            result.Add("Title", YandexRes.Title);
            result.Add("YandexMoneyPM", YandexRes.YandexMoneyPM);
            result.Add("PaymentMethod", YandexRes.PaymentMethod);
            result.Add("BankingCardPM", YandexRes.BankingCardPM);
            result.Add("YandexDesc", YandexRes.YandexDesc);
            result.Add("NavHome", YandexRes.NavHome);
            result.Add("NavPayment", YandexRes.NavPayment);
            result.Add("NavActive", YandexRes.NavActive);

            return result;
        }

        public LocaleViewModel YandexKassa()
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", YandexKassaRes.Title);
            result.Add("NavHome", YandexKassaRes.NavHome);
            result.Add("NavPayment", YandexKassaRes.NavPayment);
            result.Add("NavActive", YandexKassaRes.NavActive);
            result.Add("ActionDesc", YandexKassaRes.ActionDesc);
            result.Add("ActionDescMini", YandexKassaRes.ActionDescMini);
            result.Add("Amount", YandexKassaRes.Amount);
            result.Add("AmountErrorText", YandexKassaRes.AmountErrorText);
            result.Add("PayBtnText", YandexKassaRes.Pay);
            result.Add("BackToPMBtnText", YandexKassaRes.BackToPM);
            

            //Нет информационного текста в самом ресурсе
            result.Add("InfoText", "");

            return result;
        }

        public LocaleViewModel Succeeded(decimal sum, string roubleSign)
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", SucceededRes.Title);
            result.Add("PaymentDescTextHtml", string.Format(SucceededRes.PaymentDescTextFormat, sum, roubleSign));
            result.Add("GoToMyOrdersBtnText", SucceededRes.GoToMyOrders);
            result.Add("HomeNav", SucceededRes.HomeNav);
            result.Add("ToppingUpNav", SucceededRes.ToppingUpNav);
            result.Add("ActiveNav", SucceededRes.Title);
            //result.Add("PayOrderBtnText", string.Format(SucceededRes.PayOrderTexFormat, orderId));


            return result;
        }
    }
}
