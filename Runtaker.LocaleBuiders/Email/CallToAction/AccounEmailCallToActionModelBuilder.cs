using Runtasker.LocaleBuilders.Enumerations;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Email.Account;

namespace Runtasker.LocaleBuilders.Email.CallToAction
{
    public class AccounEmailCallToActionModelBuilder : UICultureSwitcher
    {
        public ForEmailCallToAction UsualRegistration(string userName, int bonus, string roubleSign)
        {
            return new ForEmailCallToAction
            {
                Header = string.Format(AccountEmRes.GreetingFormat, userName),
                BigText = string.Format(AccountEmRes.RegistrationTextFormat, bonus, roubleSign) +
                        $" {AccountEmRes.RuntaskerWish}",
                ActionBtnText = AccountEmRes.RegistrationButtonText
            };
        }

        public ForEmailCallToAction SocialRegistration(string userName, string provider, int bonus, string roubleSign)
        {
            return new ForEmailCallToAction
            {
                Header = string.Format(AccountEmRes.GreetingFormat, userName),
                BigText = string.Format(AccountEmRes.SocialRegistrationTextFormat, bonus, roubleSign) +
                                 $" {string.Format(AccountEmRes.LoginViaProviderFormat, provider)} {AccountEmRes.RuntaskerWish}",
                ActionBtnText = AccountEmRes.RegistrationButtonText
            };
        }

        /// <summary>
        /// Не доделано
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="roubleSign"></param>
        /// <returns></returns>
        public ForEmailCallToAction RegistrationWithCreatedOrder(string userName, string email, string password, string roubleSign)
        {
            switch(UILang)
            {
                case Lang.Russian:
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
