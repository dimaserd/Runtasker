using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Views.Home.KnowPrice;

namespace Runtaker.LocaleBuiders.Views.Home
{
    public class HomeViewModelBuilder : UICultureSwitcher
    {
        public LocaleViewModel KnowPriceView()
        {
            switch (UICultureName)
            {
                case "ru-RU":
                    LocaleViewModel rusResult = new LocaleViewModel();
                    rusResult.Add("NameError", KnowPriceRes.NameError);
                    rusResult.Add("EmailError", KnowPriceRes.EmailError);
                    rusResult.Add("DescriptionError", KnowPriceRes.DescriptionError);
                    rusResult.Add("BtnText", KnowPriceRes.BtnText);
                    rusResult.Add("HomeNav", KnowPriceRes.HomeNav);
                    rusResult.Add("ActiveNav", KnowPriceRes.ActiveNav);
                    rusResult.Add("ActionDescMini", KnowPriceRes.ActionDescMini);
                    rusResult.Add("HtmlActionDesc", $"{KnowPriceRes.ActionDesc1} <strong><em>{KnowPriceRes.ActionDesc2}</em></strong> {KnowPriceRes.ActionDesc3}");
                    rusResult.Add("DescriptionPlaceholder", KnowPriceRes.DescriptionPlaceholder);

                    return rusResult;

                default:
                    LocaleViewModel result = new LocaleViewModel();
                    result.Add("NameError", KnowPriceRes.NameError);
                    result.Add("EmailError", KnowPriceRes.EmailError);
                    result.Add("DescriptionError", KnowPriceRes.DescriptionError);
                    result.Add("BtnText", KnowPriceRes.BtnText);
                    result.Add("HomeNav", KnowPriceRes.HomeNav);
                    result.Add("ActiveNav", KnowPriceRes.ActiveNav);
                    result.Add("ActionDescMini", KnowPriceRes.ActionDescMini);
                    result.Add("DescriptionPlaceholder", KnowPriceRes.DescriptionPlaceholder);

                    result.Add("HtmlActionDesc", $"{KnowPriceRes.ActionDesc1} <strong><em>{KnowPriceRes.ActionDesc2}</strong></em> {KnowPriceRes.ActionDesc3}");
                    return result;
            }
        }
    }
}
