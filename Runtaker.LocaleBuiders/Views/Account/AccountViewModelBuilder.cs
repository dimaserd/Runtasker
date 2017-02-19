using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Views.Account.Login;
using Runtasker.Resources.Views.Account.Register;

namespace Runtaker.LocaleBuiders.Views.Account
{
    public class AccountViewModelBuilder : UICultureSwitcher
    {
        public LocaleViewModel LoginView()
        {
            switch(UICultureName)
            {
                case "ru-RU":
                    LocaleViewModel rusResult = new LocaleViewModel();
                    rusResult.Add("Title", Login.Title);
                    rusResult.Add("HomeNav", Login.HomeNav);
                    rusResult.Add("ActiveNav", Login.SignIn);
                    rusResult.Add("htmlHeader", $"<h2>{Login.htmlHeader1} <strong>{Login.htmlHeader2}</strong></h2>");
                    rusResult.Add("htmlSocialSignIn", $"<h2>{Login.ViaSocials1} <strong>{Login.ViaSocials2}</strong></h2>");
                    rusResult.Add("htmlWithoutAccount", $"{Login.htmlWithoutAccount1} <a href=\"/Account/Register\">{Login.htmlWithoutAccount2}</a>, {Login.htmlWithoutAccount3}");
                    rusResult.Add("RememberMe", Login.RememberMe);
                    rusResult.Add("SignIn", Login.SignIn);
                    rusResult.Add("ForgotYourPass", Login.ForgotYourPass);
                    return rusResult;

                default:
                    LocaleViewModel result = new LocaleViewModel();
                    result.Add("Title", Login.Title);
                    result.Add("HomeNav", Login.HomeNav);
                    result.Add("ActiveNav", Login.SignIn);
                    result.Add("htmlHeader", $"<h2>{Login.htmlHeader1} <strong>{Login.htmlHeader2}</strong></h2>");
                    result.Add("htmlSocialSignIn", $"<h2>{Login.ViaSocials1} <strong>{Login.ViaSocials2}</strong></h2>");
                    result.Add("htmlWithoutAccount", $"{Login.htmlWithoutAccount1} <a href=\"/Account/Register\">{Login.htmlWithoutAccount2}</a>, {Login.htmlWithoutAccount3}");
                    result.Add("RememberMe", Login.RememberMe);
                    result.Add("SignIn", Login.SignIn);
                    result.Add("ForgotYourPass", Login.ForgotYourPass);
                    return result;
            }
            
        }

        public LocaleViewModel RegisterView()
        {
            switch(UICultureName)
            {
                case "ru-RU":
                    LocaleViewModel rusResult = new LocaleViewModel();
                    rusResult.Add("Title", RegisterRes.Title);
                    rusResult.Add("HomeNav", RegisterRes.HomeNav);
                    rusResult.Add("ActiveNav", RegisterRes.ActiveNav);
                    rusResult.Add("htmlHeader", $"<h2>{RegisterRes.htmlHeader1} <strong>{RegisterRes.htmlHeader2}</strong></h2>");
                    rusResult.Add("htmlHeader2", $"<h2>{RegisterRes.Register}</h2>");
                    rusResult.Add("registerBtn", RegisterRes.Register);
                    rusResult.Add("WhyToReg", RegisterRes.WhyToReg);
                    rusResult.Add("WhyToRegDesc", RegisterRes.WhyToRegDesc);
                    rusResult.Add("RegPlusesHeader", RegisterRes.RegPlusesHeader);
                    rusResult.Add("RegPlus1", RegisterRes.RegPlus1);
                    rusResult.Add("RegPlus2", RegisterRes.RegPlus2);
                    rusResult.Add("RegPlus3", RegisterRes.RegPlus3);
                    rusResult.Add("RegPlus4", RegisterRes.RegPlus4);
                    rusResult.Add("AlreadyHaveAc", RegisterRes.AlreadyHaveAc);
                    rusResult.Add("ClickToSI", RegisterRes.ClickToSI);
                    rusResult.Add("ContCustSup", RegisterRes.ContCustSup);
                    rusResult.Add("htmlContactField", $"{RegisterRes.ContactText1} <a href=\"/Home/Contact\">{RegisterRes.ContactText2}</a>.");
                    return rusResult;

                default:
                    LocaleViewModel result = new LocaleViewModel();
                    result.Add("Title", RegisterRes.Title);
                    result.Add("HomeNav", RegisterRes.HomeNav);
                    result.Add("ActiveNav", RegisterRes.ActiveNav);
                    result.Add("htmlHeader", $"<h2>{RegisterRes.htmlHeader1} <strong>{RegisterRes.htmlHeader2}</strong></h2>");
                    result.Add("htmlHeader2", $"<h2>{RegisterRes.Register}</h2>");
                    result.Add("registerBtn", RegisterRes.Register);
                    result.Add("WhyToReg", RegisterRes.WhyToReg);
                    result.Add("WhyToRegDesc", RegisterRes.WhyToRegDesc);
                    result.Add("RegPlusesHeader", RegisterRes.RegPlusesHeader);
                    result.Add("RegPlus1", RegisterRes.RegPlus1);
                    result.Add("RegPlus2", RegisterRes.RegPlus2);
                    result.Add("RegPlus3", RegisterRes.RegPlus3);
                    result.Add("RegPlus4", RegisterRes.RegPlus4);
                    result.Add("AlreadyHaveAc", RegisterRes.AlreadyHaveAc);
                    result.Add("ClickToSI", RegisterRes.ClickToSI);
                    result.Add("ContCustSup", RegisterRes.ContCustSup);
                    result.Add("htmlContactField", $"{RegisterRes.ContactText1} <a href=\"/Home/Contact\">{RegisterRes.ContactText2}</a>.");
                    return result;
            }
        }
    }
}
