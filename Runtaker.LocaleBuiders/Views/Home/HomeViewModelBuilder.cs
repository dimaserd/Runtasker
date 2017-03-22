using Extensions.String;
using Runtasker.LocaleBuilders.Enumerations;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Views.Home.Index;
using Runtasker.Resources.Views.Home.KnowPrice;

namespace Runtasker.LocaleBuilders.Views.Home
{
    public class HomeViewModelBuilder : UICultureSwitcher
    {
        public LocaleViewModel KnowPriceView()
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("NameError", KnowPriceRes.NameError);
            result.Add("EmailError", KnowPriceRes.EmailError);
            result.Add("DescriptionError", KnowPriceRes.DescriptionError);
            result.Add("OtherSubjectError", KnowPriceRes.OtherSubjectError);
            result.Add("BtnText", KnowPriceRes.BtnText);
            result.Add("HomeNav", KnowPriceRes.HomeNav);
            result.Add("ActiveNav", KnowPriceRes.ActiveNav);
            result.Add("ActionDescMini", KnowPriceRes.ActionDescMini);

            switch (UILang)
            {
                case Lang.Russian:

                    result.Add("HtmlActionDesc", $"{KnowPriceRes.ActionDesc1} <strong><em>{KnowPriceRes.ActionDesc2}</em></strong> {KnowPriceRes.ActionDesc3}");
                    result.Add("DescriptionPlaceholder", KnowPriceRes.DescriptionPlaceholder);
                    break;

                default:
                    result.Add("HtmlActionDesc", $"{KnowPriceRes.ActionDesc1} <strong><em>{KnowPriceRes.ActionDesc2}</strong></em> {KnowPriceRes.ActionDesc3}");
                    result.Add("DescriptionPlaceholder", KnowPriceRes.DescriptionPlaceholder);
                    break;

                    
            }

            return result;
        }

        //засунь в HtmlSigns
        public LocaleViewModel HomeView(string htmlSign = "&raquo;") 
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", Index.Title);
            result.Add("Greeting", Index.Greeting);
            result.Add("TryOurServicesHtml", string.Format(Index.TryOurServicesPattern,
                Index.TryOurServicesMini.WrapToSpan(new { @class = "hidden-xs" }),
                htmlSign));

            return result;
        }

        public LocaleViewModel CommentsView(string htmlSign = "&raquo;")
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", Index.Title);
            result.Add("Greeting", Index.Greeting);
            result.Add("TryOurServicesHtml", string.Format(Index.TryOurServicesPattern,
                Index.TryOurServicesMini.WrapToSpan(new { @class = "hidden-xs" }),
                htmlSign));

            return result;
        }

    }
}
