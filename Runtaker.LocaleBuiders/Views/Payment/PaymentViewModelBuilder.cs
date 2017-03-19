using Extensions.String;
using Runtaker.LocaleBuiders.Enumerations;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Views.Payment.Index;
using Runtasker.Resources.Views.Payment.Robokassa;
using Runtasker.Resources.Views.Payment.Yandex;

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
            result.Add("InfoText", IndexRes.InfoText);
            result.Add("Title", IndexRes.Title);
            result.Add("NavHome", IndexRes.NavHome);
            result.Add("NavActive", IndexRes.NavActive);

            switch (UILang)
            {
                case Lang.Russian:
                    result.Add("HtmlTopUpViaRoboKassa", $"{IndexRes.TopUp.WrapToSpan(new { @class ="hidden-xs" })} {IndexRes.ViaRobokassa}");
                    result.Add("HtmlTopUpViaYandexMoney", $"{IndexRes.TopUp.WrapToSpan(new { @class = "hidden-xs" })} {IndexRes.ViaYandexMoney}");
                    return result;

                default:
                    result.Add("HtmlTopUpViaRoboKassa", $"{IndexRes.TopUp.WrapToSpan(new { @class = "hidden-xs" })} {IndexRes.ViaRobokassa}");
                    result.Add("HtmlTopUpViaYandexMoney", $"{IndexRes.TopUp.WrapToSpan(new { @class = "hidden-xs" })} {IndexRes.ViaYandexMoney}");
                    return result;
            }
        }

        public LocaleViewModel Robokassa(decimal amount, decimal withdrawAmount, string roubleSign)
        {
            switch(UILang)
            {
                case Lang.Russian:
                    LocaleViewModel rusResult = new LocaleViewModel();
                    rusResult.Add("Title", RkassaRes.Title1);
                    rusResult.Add("ActionDesc", $"{RkassaRes.ActionDesc1} {amount}{roubleSign}");
                    rusResult.Add("ActionDescMini", $"{RkassaRes.ActionDescMini}");
                    rusResult.Add("Info", $"{RkassaRes.Info1} {withdrawAmount}{roubleSign} {RkassaRes.Info2}");
                    rusResult.Add("BackToPM", RkassaRes.BackToPM);
                    return rusResult;

                default:
                    LocaleViewModel result = new LocaleViewModel();
                    result.Add("Title", RkassaRes.Title1);
                    result.Add("ActionDesc", $"{RkassaRes.ActionDesc1} {amount}{roubleSign}");
                    result.Add("ActionDescMini", $"{RkassaRes.ActionDescMini}");
                    result.Add("Info", $"{RkassaRes.Info1} {withdrawAmount}{roubleSign} {RkassaRes.Info2}");
                    result.Add("BackToPM", RkassaRes.BackToPM);
                    return result;
            }
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
    }
}
