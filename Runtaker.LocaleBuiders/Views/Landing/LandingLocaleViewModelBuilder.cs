using Extensions.String;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Views.Landing.Counters;
using Runtasker.Resources.Views.Landing.Portfolio;
using Runtasker.Resources.Views.Landing.Slider;

namespace Runtasker.LocaleBuilders.Views.Landing
{
    public class LandingLocaleViewModelBuilder : UICultureSwitcher
    {
        public LocaleViewModel PortfolioView()
        {
            LocaleViewModel result = new LocaleViewModel();

            switch (UICultureName)
            {
                case "ru-RU":
                    result.Add("HeaderHtml", $"{PortfolioRes.Header1} {PortfolioRes.Runtasker.WrapToStrong().WrapToEm()}");
                    result.Add("ShowAll", PortfolioRes.ShowAll);
                    result.Add("Physics", PortfolioRes.Physics);
                    result.Add("Mathematics", PortfolioRes.Mathematics);
                    result.Add("Programming", PortfolioRes.Programming);
                    result.Add("ViewImageHmtl", $"{PortfolioRes.ViewImage1.WrapToStrong()} {PortfolioRes.ViewImage2}");
                    result.Add("CallOutBtnHtmlLink", $"{PortfolioRes.CalloutQuestion1}? {PortfolioRes.Callout1.WrapToA(new { href = "/Home/KnowPrice", target = "_blank", @class = "btn btn-primary btn-lg" })}");
                    break;

                default:
                    result.Add("HeaderHtml", $"{PortfolioRes.Header1} {PortfolioRes.Runtasker.WrapToStrong().WrapToEm()}");
                    result.Add("ShowAll", PortfolioRes.ShowAll);
                    result.Add("Physics", PortfolioRes.Physics);
                    result.Add("Mathematics", PortfolioRes.Mathematics);
                    result.Add("Programming", PortfolioRes.Programming);
                    result.Add("ViewImageHmtl", $"{PortfolioRes.ViewImage1.WrapToStrong()} {PortfolioRes.ViewImage2}");
                    result.Add("CallOutBtnHtmlLink", $"{PortfolioRes.CalloutQuestion1}? {PortfolioRes.Callout1.WrapToA(new { href = "/Home/KnowPrice", target = "_blank", @class = "btn btn-primary btn-lg" })}");
                    break;
            }

            return result;
        }

        public LocaleViewModel SliderView()
        {
            LocaleViewModel result = new LocaleViewModel();

            switch (UICultureName)
            {
                case "ru-RU":
                    result.Add("MainHeader", SliderRes.MainHeader);
                    result.Add("ForMainHeader", SliderRes.ForMainHeader);
                    result.Add("Slogan", SliderRes.Slogan);
                    break;

                default:
                    result.Add("MainHeader", SliderRes.MainHeader);
                    result.Add("ForMainHeader", SliderRes.ForMainHeader);
                    result.Add("Slogan", SliderRes.Slogan);
                    break;
            }

            return result;
        }

        public LocaleViewModel CountersView()
        {
            LocaleViewModel result = new LocaleViewModel();

            switch (UICultureName)
            {
                case "ru-RU":
                    result.Add("ExecutedWorks", CountersRes.ExecutedWorks);
                    result.Add("HappyClients", CountersRes.HappyClients);
                    result.Add("Performers", CountersRes.Performers);
                    result.Add("YearsInBusiness", CountersRes.YearsInBusiness);
                    break;

                default:
                    result.Add("ExecutedWorks", CountersRes.ExecutedWorks);
                    result.Add("HappyClients", CountersRes.HappyClients);
                    result.Add("Performers", CountersRes.Performers);
                    result.Add("YearsInBusiness", CountersRes.YearsInBusiness);
                    break;
            }

            return result;
        }
    }
}
