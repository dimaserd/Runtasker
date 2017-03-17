using Extensions.String;
using Runtaker.LocaleBuiders.Enumerations;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Views.Account.Login;
using Runtasker.Resources.Views.Account.Register;
using Runtasker.Resources.Views.Shared.NewLoginPartial;

namespace Runtaker.LocaleBuiders.Views.Account
{
    public class AccountViewModelBuilder : UICultureSwitcher
    {
        

        public LocaleViewModel LoginView()
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", Login.Title);
            result.Add("HomeNav", Login.HomeNav);
            result.Add("ActiveNav", Login.SignIn);
            result.Add("RememberMe", Login.RememberMe);
            result.Add("SignIn", Login.SignIn);
            result.Add("ForgotYourPass", Login.ForgotYourPass);
            result.Add("Pattern", NewLoginPartial.Pattern);


            switch (UILang)
            {
                case Lang.Russian:
                    result.Add("WelcomeHtml", $"{Login.WelcomeTitle1} {Login.Runtasker.WrapToStrong().WrapToEm()}"); 
                    result.Add("htmlHeader", $"{Login.htmlHeader1} {Login.htmlHeader2.WrapToStrong().WrapToEm()}".WrapToH2());
                    result.Add("htmlSocialSignIn", $"{Login.ViaSocials1} {Login.ViaSocials2.WrapToStrong().WrapToEm()}".WrapToH2());
                    result.Add("htmlWithoutAccount", $"{Login.htmlWithoutAccount1} {Login.htmlWithoutAccount2.WrapToA("/Account/Register")}, {Login.htmlWithoutAccount3}");
                    
                    return result;

                default:
                    result.Add("WelcomeHtml", $"{Login.WelcomeTitle1} {Login.Runtasker.WrapToStrong().WrapToEm()}");
                    result.Add("htmlHeader", $"{Login.htmlHeader1} {Login.htmlHeader2.WrapToStrong().WrapToEm()}".WrapToH2());
                    result.Add("htmlSocialSignIn", $"{Login.ViaSocials1} {Login.ViaSocials2.WrapToStrong().WrapToEm()}".WrapToH2());
                    result.Add("htmlWithoutAccount", $"{Login.htmlWithoutAccount1} {Login.htmlWithoutAccount2.WrapToA("/Account/Register")}, {Login.htmlWithoutAccount3}");
                    
                    return result;
            }
            
        }

        public LocaleViewModel SignInView(string userName, string balance, string roubleSign)
        {
            LocaleViewModel result = new LocaleViewModel();

            //общие 
            result.Add("SignIn", NewLoginPartial.SignIn);
            result.Add("Email", NewLoginPartial.Email);
            result.Add("Password", NewLoginPartial.Password);
            result.Add("Recharge", NewLoginPartial.Recharge);
            result.Add("ViewProfile", NewLoginPartial.ViewProfile);
            result.Add("SignOut", NewLoginPartial.SignOut);
            result.Add("RememberMe", NewLoginPartial.RememberMe);
            result.Add("CreateAccount", NewLoginPartial.CreateAccount);
            result.Add("SetPassword", NewLoginPartial.SetPassword);
            result.Add("Pattern", NewLoginPartial.Pattern);

            switch (UILang)
            {
                case (Lang.English):
                    
                    result.Add("HelloUser", $"{NewLoginPartial.Hello}, {userName}!");
                    result.Add("ForgotPass", $"{NewLoginPartial.ForgotPass}?");
                    result.Add("YourBalanceHtml", $"{NewLoginPartial.YourBalance1} {balance}{roubleSign}");

                    break;

                case (Lang.Russian):
                    result.Add("HelloUser", $"{NewLoginPartial.Hello}, {userName}!");
                    result.Add("ForgotPass", $"{NewLoginPartial.ForgotPass}?");
                    result.Add("YourBalanceHtml", $"{NewLoginPartial.YourBalance1} {balance}{roubleSign}");

                    break;

                case (Lang.Chinese):
                    result.Add("HelloUser", $"{NewLoginPartial.Hello}, {userName}!");
                    result.Add("ForgotPass", $"{NewLoginPartial.ForgotPass}");
                    //не сделано!
                    result.Add("YourBalanceHtml", $"{NewLoginPartial.YourBalance1} {balance}{roubleSign}");

                    break;
            }
            return result;
        }

        public LocaleViewModel RegisterView()
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", RegisterRes.Title);
            result.Add("HomeNav", RegisterRes.HomeNav);
            result.Add("ActiveNav", RegisterRes.ActiveNav);
            //плюсы регистрации
            result.Add("RegPlusesHeader", RegisterRes.RegPlusesHeader);
            result.Add("RegPlus1", RegisterRes.RegPlus1);
            result.Add("RegPlus2", RegisterRes.RegPlus2);
            result.Add("RegPlus3", RegisterRes.RegPlus3);
            result.Add("RegPlus4", RegisterRes.RegPlus4);
            result.Add("RegPlus5", RegisterRes.RegPlus5);
            result.Add("RegPlus6", RegisterRes.RegPlus6);

            switch (UILang)
            {
                case Lang.Russian:
                    
                    
                    result.Add("htmlHeader", $"<h2>{RegisterRes.htmlHeader1} <strong>{RegisterRes.htmlHeader2}</strong></h2>");
                    result.Add("htmlHeader2", $"<h2>{RegisterRes.Register}</h2>");
                    result.Add("registerBtn", RegisterRes.Register);
                    result.Add("WhyToReg", RegisterRes.WhyToReg);
                    result.Add("WhyToRegDesc", RegisterRes.WhyToRegDesc);
                    result.Add("AlreadyHaveAc", RegisterRes.AlreadyHaveAc);
                    result.Add("ClickToSI", RegisterRes.ClickToSI);
                    result.Add("ContCustSup", RegisterRes.ContCustSup);
                    result.Add("htmlContactField", $"{RegisterRes.ContactText1} {RegisterRes.ContactText2.WrapToA(href: "/Home/Contact")}.");
                    break;

                default:
                    result.Add("htmlHeader", $"{RegisterRes.htmlHeader1} {RegisterRes.htmlHeader2.WrapToStrong()}".WrapToH2());
                    result.Add("htmlHeader2", RegisterRes.Register.WrapToH2());
                    result.Add("registerBtn", RegisterRes.Register);
                    result.Add("WhyToReg", RegisterRes.WhyToReg);
                    result.Add("WhyToRegDesc", RegisterRes.WhyToRegDesc);        
                    result.Add("AlreadyHaveAc", RegisterRes.AlreadyHaveAc);
                    result.Add("ClickToSI", RegisterRes.ClickToSI);
                    result.Add("ContCustSup", RegisterRes.ContCustSup);
                    result.Add("htmlContactField", $"{RegisterRes.ContactText1} {RegisterRes.ContactText2.WrapToA(href: "/Home/Contact")}.");
                    break;
            }

            return result;
        }
    }
}
