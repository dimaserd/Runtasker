using Extensions.String;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Views.Home.Contact;
using Runtasker.Resources.Views.Home.Index;
using Runtasker.Resources.Views.Home.KnowPrice;

namespace Runtasker.LocaleBuilders.Views.Home
{
    public class HomeViewModelBuilder : UICultureSwitcher
    {
        public LocaleViewModel ContactView()
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", ContactViewRes.ContactUs);
            result.Add("BtnText", ContactViewRes.SendMessage);
            result.Add("HomeNav", ContactViewRes.HomeNav);
            result.Add("ActiveNav", ContactViewRes.ContactUs);
            result.Add("Header", ContactViewRes.ContactUs);
            result.Add("AlertTextHtml", string.Format(ContactViewRes.AlertFormat, ContactViewRes.AlertToMark.WrapToStrong()));
            result.Add("MessagePlaceholderText", ContactViewRes.MessagePlaceholder);
            
            return result;
        }

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
            result.Add("DescriptionPlaceholder", KnowPriceRes.DescriptionPlaceholder);
            result.Add("ActionDescHtml", string.Format(KnowPriceRes.ActionDescFormat, KnowPriceRes.ActionDescToMark.WrapToEm().WrapToStrong()));

            

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
