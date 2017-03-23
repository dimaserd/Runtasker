﻿using Extensions.String;
using Runtasker.LocaleBuilders.Enumerations;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Entities.Order;
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

        public LocaleViewModel CountersView()
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("ExecutedWorks", CountersRes.ExecutedWorks);
            result.Add("HappyClients", CountersRes.HappyClients);
            result.Add("Performers", CountersRes.Performers);
            result.Add("YearsInBusiness", CountersRes.YearsInBusiness);
            

            return result;
        }

        public LocaleViewModel PricingView(int ordinaryPrice, int essayPrice, int courseWorkPrice, string roubleSign)
        {
            LocaleViewModel result = new LocaleViewModel();

            //типы работы из другого файла ресурсов
            result.Add("CourseWork", OrderResource.CourseWork);
            result.Add("Ordinary", OrderResource.Ordinary);
            result.Add("Essay", OrderResource.Essay);

            //цены от скольки-то рублей
            result.Add("OrdinaryFromPrice", string.Format(PricingRes.FromPriceFormat, ordinaryPrice, roubleSign));
            result.Add("EssayFromPrice", string.Format(PricingRes.FromPriceFormat, essayPrice, roubleSign));
            result.Add("CourseWorkFromPrice", string.Format(PricingRes.FromPriceFormat, courseWorkPrice, roubleSign));


            result.Add("Title", PricingRes.Title);
            result.Add("PayWithBonusMoney", PricingRes.PayWithBonusMoney);
            result.Add("DiscussWithPerformer", PricingRes.DiscussWithPerformer);
            result.Add("FreeCorrections", PricingRes.FreeCorrections);
            result.Add("ShortTimeExecution", PricingRes.ShortTimeExecution);
            result.Add("ReturningFundsWarranty", PricingRes.ReturningFundsWarranty);
            result.Add("FindOutThePrice", PricingRes.FindOutThePrice);
            result.Add("FreeUniqueTextUpgrade", PricingRes.FreeUniqueTextUpgrade);

            return result;
        }
    }
}
