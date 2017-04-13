using HtmlExtensions.HtmlEntities;
using HtmlExtensions.Renderers;
using Logic.Extensions.HtmlEmail;
using Runtasker.LocaleBuilders.Email.CallToAction;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Logic.Entities;
using Runtasker.Logic.HtmlConstantEmails.Models;
using Runtasker.Resources.Email.Account;
using Runtasker.Settings;

namespace Runtasker.Logic.HtmlConstantEmails
{
    public class AccountEmailConstants
    {
        #region Constructors
        public AccountEmailConstants()
        {
            Construct();
        }

        void Construct()
        {
            ModelBuilder = new AccounEmailCallToActionModelBuilder();
            HtmlSigns = new HtmlSignsRenderer();
        }
        #endregion

        #region Properties
        AccounEmailCallToActionModelBuilder ModelBuilder { get; set; }

        HtmlSignsRenderer HtmlSigns { get; set; }
        #endregion

        #region Public Methods

        public EmailModel GetEmailForRegistrationWithCreatedOrder(ApplicationUser user, string password, string callBackUrl)
        {
            ForEmailCallToAction model = ModelBuilder
                .RegistrationWithCreatedOrder(user.Name, user.Email, password, HtmlSigns.Rouble);

            string Text = new HtmlEmailBase
                (
                    callToAction: new BigEmailCallToActionBase
                    (
                        header: model.Header,
                        littleHeader: null,
                        bigText: model.BigText,
                        link: new HtmlLink
                        (
                            hrefParam: callBackUrl,
                            textParam: model.ActionBtnText
                        )
                    )
                ).ToString();

            return new EmailModel
            {
                Subject = AccountEmRes.RegWithOrderSubject1,
                Body = Text
            };
        }

        public EmailModel GetEmailForRegistration(ApplicationUser user, string callBackUrl)
        {
            ForEmailCallToAction model = ModelBuilder.UsualRegistration(user.Name, UISettings.RegistrationBonus, HtmlSigns.Rouble);
            string Text = new HtmlEmailBase
                (
                    callToAction: new BigEmailCallToActionBase
                    (
                        header: model.Header,
                        littleHeader: null,
                        bigText: model.BigText,    
                        link: new HtmlLink
                        (
                            hrefParam: callBackUrl,
                            textParam: model.ActionBtnText
                        )
                    )//Call to action
                ).ToString();

            return new EmailModel
            {
                Subject = AccountEmRes.RegistrationSubject,
                Body = Text
            };
        }


        public EmailModel GetEmailForRegistrationFromSocialProvider(ApplicationUser user, string callBackUrl, string provider)
        {
            ForEmailCallToAction model = ModelBuilder.SocialRegistration(user.Name, provider, UISettings.RegistrationBonus, HtmlSigns.Rouble);
            string Text =  new HtmlEmailBase
                (
                    callToAction: new BigEmailCallToActionBase
                    (
                        header: model.Header,
                        littleHeader: null,
                        bigText: model.BigText,
                        link: new HtmlLink
                        (
                            hrefParam: callBackUrl,
                            textParam: model.ActionBtnText
                        )
                    )//Call to action
                ).ToString();

            return new EmailModel
            {
                Body = Text,
                Subject = AccountEmRes.RegistrationSubject
            };
        }

        public EmailModel GetEmailForPasswordResetting(string callBackUrl)
        {
            return new EmailModel
            {
                Subject = AccountEmRes.ResetPassSubject,
                Body = $"{AccountEmRes.ResetPassText1} <a href=\"{callBackUrl}\">{AccountEmRes.ResetPassText2}</a>"
            };
        }
        #endregion
    }
}
