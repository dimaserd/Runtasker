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
            switch (UILang)
            {
                case Lang.Russian:
                    LocaleViewModel rusResult = new LocaleViewModel();
                    rusResult.Add("ActionDesc", IndexRes.ActionDesc);
                    rusResult.Add("ActionDescMini", IndexRes.ActionDescMini);
                    rusResult.Add("Amount", IndexRes.Amount);
                    rusResult.Add("ErrorText", IndexRes.ErrorText);
                    rusResult.Add("InfoText", IndexRes.InfoText);
                    rusResult.Add("Title", IndexRes.Title);
                    rusResult.Add("NavHome", IndexRes.NavHome);
                    rusResult.Add("NavActive", IndexRes.NavActive);
                    rusResult.Add("HtmlTopUpViaRoboKassa", $"<span class=\"hidden-xs\">{IndexRes.TopUp}</span> {IndexRes.ViaRobokassa}");
                    rusResult.Add("HtmlTopUpViaYandexMoney", $"<span class=\"hidden-xs\">{IndexRes.TopUp}</span> {IndexRes.ViaYandexMoney}");
                    return rusResult;

                default:
                    LocaleViewModel result = new LocaleViewModel();
                    
                    result.Add("ActionDesc", IndexRes.ActionDesc);
                    result.Add("ActionDescMini", IndexRes.ActionDescMini);
                    result.Add("Amount", IndexRes.Amount);
                    result.Add("ErrorText", IndexRes.ErrorText);
                    result.Add("InfoText", IndexRes.InfoText);
                    result.Add("Title", IndexRes.Title);
                    result.Add("NavHome", IndexRes.NavHome);
                    result.Add("NavActive", IndexRes.NavActive);
                    result.Add("HtmlTopUpViaRoboKassa", $"<span class=\"hidden-xs\">{IndexRes.TopUp}</span> {IndexRes.ViaRobokassa}");
                    result.Add("HtmlTopUpViaYandexMoney", $"<span class=\"hidden-xs\">{IndexRes.TopUp}</span> {IndexRes.ViaYandexMoney}");
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
            switch(UILang)
            {
                case Lang.Russian:
                    LocaleViewModel rusResult = new LocaleViewModel();
                    rusResult.Add("ActionDesc", YandexRes.ActionDesc);
                    rusResult.Add("ActionDescMini", YandexRes.ActionDescMini);
                    rusResult.Add("BackToPM", YandexRes.BackToPM);
                    rusResult.Add("AmountErrorText", YandexRes.AmountErrorText);
                    rusResult.Add("InfoText", YandexRes.InfoText);
                    rusResult.Add("Pay", YandexRes.Pay);
                    rusResult.Add("Amount", YandexRes.Amount);
                    rusResult.Add("Title", YandexRes.Title);
                    rusResult.Add("YandexMoneyPM", YandexRes.YandexMoneyPM);
                    rusResult.Add("PaymentMethod", YandexRes.PaymentMethod);
                    rusResult.Add("BankingCardPM", YandexRes.BankingCardPM);
                    rusResult.Add("YandexDesc", YandexRes.YandexDesc);
                    rusResult.Add("NavHome", YandexRes.NavHome);
                    rusResult.Add("NavPayment", YandexRes.NavPayment);
                    rusResult.Add("NavActive", YandexRes.NavActive);
                    return rusResult;

                default:
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
}
