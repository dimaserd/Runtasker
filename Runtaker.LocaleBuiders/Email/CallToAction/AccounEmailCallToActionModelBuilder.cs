using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Email.Account;

namespace Runtasker.LocaleBuilders.Email.CallToAction
{
    public class AccounEmailCallToActionModelBuilder : UICultureSwitcher
    {
        public ForEmailCallToAction UsualRegistration(string userName, int bonus, string roubleSign)
        {
            switch(UICultureName)
            {
                case "ru-RU":
                    return new ForEmailCallToAction
                    {
                        Header = $"{AccountEmRes.Greeting} {userName}!",
                        BigText = $"{AccountEmRes.RegistrationText1} {bonus}{roubleSign} " + 
                        $"{AccountEmRes.RegistrationText2} {AccountEmRes.RuntaskerWish}",
                        ActionBtnText = AccountEmRes.RegistrationButtonText
                    };

                default:
                    return new ForEmailCallToAction
                    {
                        Header = $"{AccountEmRes.Greeting}, {userName}!",
                        BigText = $"{AccountEmRes.RegistrationText1} {bonus}{roubleSign} " +
                        $"{AccountEmRes.RegistrationText2} {AccountEmRes.RuntaskerWish}",
                        ActionBtnText = AccountEmRes.RegistrationButtonText
                    };
            }
            
        }

        public ForEmailCallToAction SocialRegistration(string userName, string provider, int bonus, string roubleSign)
        {
            switch(UICultureName)
            {
                case "ru-RU":
                    return new ForEmailCallToAction
                    {
                        Header = $"{AccountEmRes.Greeting} {userName}!",
                        BigText = $"{AccountEmRes.RegistrationText1} {bonus}{roubleSign}." +
                                 $"{AccountEmRes.HowToLoginViaProvider} : {provider}. {AccountEmRes.RuntaskerWish}",
                        ActionBtnText = AccountEmRes.RegistrationButtonText
                    };

                default:
                    return new ForEmailCallToAction
                    {
                        Header = $"{AccountEmRes.Greeting} {userName}!",
                        BigText = $"{AccountEmRes.RegistrationText1} {bonus}{roubleSign}." +
                                 $"{AccountEmRes.HowToLoginViaProvider} : {provider}. {AccountEmRes.RuntaskerWish}",
                        ActionBtnText = AccountEmRes.RegistrationButtonText
                    };
            }
        }

        public ForEmailCallToAction RegistrationWithCreatedOrder(string userName, string email, string password, string roubleSign)
        {
            switch(UICultureName)
            {
                case "ru-RU":
                    return new ForEmailCallToAction
                    {
                        Header = $"{AccountEmRes.Greeting}, {userName}!",
                        BigText = $"{AccountEmRes.RegWithOrdertText1}" +
                            $"<p>{AccountEmRes.RegWithOrdertText2}:</p>" +
                        $"<p>{AccountEmRes.RegWithOrdertText3} : {email}</p>" +
                        $"<p>{AccountEmRes.RegWithOrdertText4} : {password}</p>",
                        ActionBtnText = $"{AccountEmRes.RegWithOrderBtnText}"
                    };

                default:
                    return new ForEmailCallToAction
                    {
                        Header = $"{AccountEmRes.Greeting}, {userName}!",
                        BigText = $"{AccountEmRes.RegWithOrdertText1} {AccountEmRes.RegWithOrdertText2}:\n" +
                        $"{AccountEmRes.RegWithOrdertText3} : {email}\n" +
                        $"{AccountEmRes.RegWithOrdertText4} : {password}",
                        ActionBtnText = $"{AccountEmRes.RegWithOrderBtnText}"
                    };
            }
        }
    }
}
