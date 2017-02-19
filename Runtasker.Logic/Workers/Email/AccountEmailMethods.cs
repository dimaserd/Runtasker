using Microsoft.AspNet.Identity;
using Runtasker.Logic.HtmlConstantEmails;
using Runtasker.Logic.HtmlConstantEmails.Models;

namespace Runtasker.Logic.Workers.Email
{
    public class AccountEmailMethods : EmailWorkerBase
    {
        #region Constructors
        public AccountEmailMethods() : base ()
        {
            Construct();
        }

        void Construct()
        {
            HtmlConstants = new AccountEmailConstants();
        }
        #endregion

        #region Properties
        AccountEmailConstants HtmlConstants { get; set; }
        #endregion

        #region Public Methods
        public void OnUserRegisteredWithCreatedOrder(ApplicationUser user, string password, string callBackUrl)
        {
            EmailModel model = HtmlConstants.GetEmailForRegistrationWithCreatedOrder(user, password, callBackUrl);
            IdentityMessage m = new IdentityMessage
            {
                Subject = model.Subject,
                Body = model.Body,
                Destination = user.Email
            };
            SendEmail(m);
        }

        public void OnUserRegistered(ApplicationUser user, string callBackUrl)
        {
            EmailModel model = HtmlConstants.GetEmailForRegistration(user, callBackUrl);

            IdentityMessage m = new IdentityMessage
            {
                Subject = model.Subject,
                Body = model.Body,
                Destination = user.Email
            };
            SendEmail(m);
        }

        public void OnUserRegisteredFromSocialProvider(ApplicationUser user, string callBackUrl, string provider)
        {
            EmailModel model = HtmlConstants.GetEmailForRegistrationFromSocialProvider(user, callBackUrl, provider);

            IdentityMessage m = new IdentityMessage
            {
                Subject = model.Subject,
                Body = model.Body,
                Destination = user.Email
            };
            SendEmail(m);
        }

        public void OnUserForgottenPass(ApplicationUser user, string callBackUrl)
        {
            EmailModel model = HtmlConstants.GetEmailForPasswordResetting(callBackUrl);
            IdentityMessage m = new IdentityMessage
            {
                Destination = user.Email,
                Body = model.Body,
                Subject = model.Subject
            };
            SendEmail(m);
        }
        #endregion
    }
}
