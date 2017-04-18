using Extensions.String;
using Runtasker.LocaleBuilders.Enumerations;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Entities.Order;
using Runtasker.Resources.Views.Landing.AboutUs;
using Runtasker.Resources.Views.Landing.Counters;
using Runtasker.Resources.Views.Landing.Portfolio;
using Runtasker.Resources.Views.Landing.Pricing;
using Runtasker.Resources.Views.Landing.Slider;

namespace Runtasker.LocaleBuilders.Views.Landing
{
    public class LandingLocaleViewModelBuilder : UICultureSwitcher
    {
        public LocaleViewModel PortfolioView()
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("ShowAll", PortfolioRes.ShowAll);
            result.Add("Physics", PortfolioRes.Physics);
            result.Add("Mathematics", PortfolioRes.Mathematics);
            result.Add("Programming", PortfolioRes.Programming);

            switch (UILang)
            {
                case Lang.Russian:
                    result.Add("HeaderHtml", $"{PortfolioRes.Header1} {PortfolioRes.Runtasker.WrapToStrong().WrapToEm()}");
                    result.Add("ViewImageHmtl", $"{PortfolioRes.ViewImage1.WrapToStrong()} {PortfolioRes.ViewImage2}");
                    result.Add("CallOutBtnHtmlLink", $"{PortfolioRes.CalloutQuestion1}? {PortfolioRes.Callout1.WrapToA(new { href = "/Home/KnowPrice", target = "_blank", @class = "btn btn-primary btn-block" })}");
                    break;

                default:
                    result.Add("HeaderHtml", $"{PortfolioRes.Header1} {PortfolioRes.Runtasker.WrapToStrong().WrapToEm()}");
                    result.Add("ViewImageHmtl", $"{PortfolioRes.ViewImage1.WrapToStrong()} {PortfolioRes.ViewImage2}");
                    result.Add("CallOutBtnHtmlLink", $"{PortfolioRes.CalloutQuestion1}? {PortfolioRes.Callout1.WrapToA(new { href = "/Home/KnowPrice", target = "_blank", @class = "btn btn-primary btn-block" })}");
                    break;
            }

            return result;
        }

        public LocaleViewModel SliderView()
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("MainHeader", SliderRes.MainHeader);
            result.Add("ForMainHeader", SliderRes.ForMainHeader);
            result.Add("Slogan", SliderRes.Slogan);

            
            return result;
        }

        public LocaleViewModel AboutUsView()
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("HeaderHtml", string.Format(AboutUsRes.HeaderFormat, AboutUsRes.Runtasker.WrapToStrong()));
            result.Add("MiniDescription", AboutUsRes.MiniDescription);
            result.Add("RuntaskerSlogan", AboutUsRes.Slogan);
            result.Add("TextParagraph", AboutUsRes.TextParagraph);
            result.Add("WhyOrderFromUsHtml", string.Format(AboutUsRes.WhyOrderFromUsFormat, AboutUsRes.OrderFromUsToMark.WrapToStrong()));
            result.Add("WorksQuality", AboutUsRes.WorksQuality);
            result.Add("WorksQualityText", AboutUsRes.WorksQualityText);
            result.Add("StrictTimetables", AboutUsRes.StrictTimetables);
            result.Add("StrictTimetablesText", AboutUsRes.StrictTimetablesText);
            result.Add("EditingAndFinilizing", AboutUsRes.EditingAndFinilizing);
            result.Add("EditingAndFinilizingText", AboutUsRes.EditingAndFinilizingText);
            result.Add("TechnicalSupport", AboutUsRes.TechnicalSupport);
            result.Add("TechnicalSupportText", AboutUsRes.TechnicalSupportText);

            return result;
        }

        public LocaleViewModel CountersView()
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("ExecutedWorks", CountersRes.ExecutedWorks);
            result.Add("HappyClients", CountersRes.HappyClients);
            result.Add("Performers", CountersRes.Performers);
            result.Add("YearsInBusiness", CountersRes.YearsInBusiness);
            

            return result;
        }

        public LocaleViewModel PricingView(int ordinaryPrice, int essayPrice, int courseWorkPrice, int onlineHelpPrice, string roubleSign)
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", PricingRes.Title);

            //типы работы из другого файла ресурсов
            result.Add("CourseWork", OrderResource.CourseWork);
            result.Add("Ordinary", OrderResource.Ordinary);
            result.Add("Essay", OrderResource.Essay);
            result.Add("OnlineHelp", OrderResource.OnlineHelp);

            //Цены от скольки-то рублей
            result.Add("OrdinaryFromPrice", string.Format(PricingRes.FromPriceFormat, ordinaryPrice, roubleSign));
            result.Add("EssayFromPrice", string.Format(PricingRes.FromPriceFormat, essayPrice, roubleSign));
            result.Add("CourseWorkFromPrice", string.Format(PricingRes.FromPriceFormat, courseWorkPrice, roubleSign));
            result.Add("OnlineHelpFromPrice", string.Format(PricingRes.FromPriceFormat, onlineHelpPrice, roubleSign));


            

            //Плюсы
            //для всех видов работ
            result.Add("PayWithBonusMoney", PricingRes.PayWithBonusMoney);
            result.Add("DiscussWithPerformer", PricingRes.DiscussWithPerformer);
            result.Add("FreeCorrections", PricingRes.FreeCorrections);
            result.Add("ShortTimeExecution", PricingRes.ShortTimeExecution);
            result.Add("ReturningFundsWarranty", PricingRes.ReturningFundsWarranty);

            //для рефератов и курсовых
            result.Add("FreeUniqueTextUpgrade", PricingRes.FreeUniqueTextUpgrade);

            //для Онлайн-помощи
            result.Add("AnyTypeOfConnection", PricingRes.AnyTypeOfConnection);

            //Действия
            result.Add("OrderOnlineHelp", PricingRes.OrderOnlineHelp);
            result.Add("FindOutThePrice", PricingRes.FindOutThePrice);

            return result;
        }
    }
}
