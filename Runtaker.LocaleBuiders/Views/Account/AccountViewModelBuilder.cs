﻿using Extensions.String;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Views.Account.Login;
using Runtasker.Resources.Views.Account.Register;
using Runtasker.Resources.Views.Shared.NewLoginPartial;
using Runtasker.Resources.Views.Account.ConfirmEmail;

namespace Runtaker.LocaleBuiders.Views.Account
{
    public class AccountViewModelBuilder : UICultureSwitcher
    {
        public LocaleViewModel ConfirmEmailView(string loginSign)
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", ConfirmEmailRes.Title);
            result.Add("Thanks", ConfirmEmailRes.Thanks);
            result.Add("ClickToLoginLink", 
                string.Format(
                    ConfirmEmailRes.PleaseClickToLoginWithBtnFormat, 
                    string.Format(ConfirmEmailRes.LoginBtnTextFormat.WrapToA(new { href="/Account/Login", @class = "btn btn-success btn-lg" }), loginSign)
                    )
                );

            return result;
        }

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
            result.Add("WelcomeHtml", string.Format(Login.WelcomeTitlePattern, Login.Runtasker.WrapToStrong().WrapToEm()) );
            result.Add("htmlHeader", string.Format(Login.HeaderFormat, Login.HeaderToMark.WrapToStrong().WrapToEm())
                .WrapToH2());
            result.Add("htmlSocialSignIn", string.Format(Login.SocialSignInFormat, Login.SocialSignInToMark.WrapToStrong().WrapToEm()).WrapToH2());
            result.Add("htmlWithoutAccount", string.Format(Login.WithoutAccountFormat, Login.WithoutAccountLinkText.WrapToA("/Account/Register")) );


            
            return result;
        }

        /// <summary>
        /// Завершенный метод
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="balance"></param>
        /// <param name="roubleSign"></param>
        /// <returns></returns>
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
            result.Add("HelloUser", string.Format(NewLoginPartial.HelloUserPattern, userName));
            result.Add("ForgotPass", NewLoginPartial.ForgotPass);
            result.Add("YourBalanceHtml", string.Format(NewLoginPartial.YourBalancePattern, balance, roubleSign));

            
            return result;
        }

        /// <summary>
        /// Завершенный метод
        /// </summary>
        /// <returns></returns>
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

            result.Add("htmlHeader", $"{RegisterRes.htmlHeader1} {RegisterRes.htmlHeader2.WrapToStrong()}".WrapToH2());
            result.Add("htmlHeader2", RegisterRes.Register.WrapToH2());
            result.Add("registerBtn", RegisterRes.Register);
            result.Add("WhyToReg", RegisterRes.WhyToReg);
            result.Add("WhyToRegDesc", RegisterRes.WhyToRegDesc);
            result.Add("AlreadyHaveAc", RegisterRes.AlreadyHaveAc);
            result.Add("ClickToSI", RegisterRes.ClickToSI);
            result.Add("ContCustSup", RegisterRes.ContCustSup);
            result.Add("htmlContactField", string.Format(RegisterRes.ContactTextPattern, RegisterRes.ContactLinkText.WrapToA(href: "/Home/Contact")));

            return result;
        }

    }
}
