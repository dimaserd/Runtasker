using HtmlExtensions.HtmlEntities;
using HtmlExtensions.Renderers;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Email;
using Runtasker.Resources.Notifications.AccountMethods;
using Runtasker.Settings;

namespace Runtasker.Logic.Workers.Notifications
{
    //Some kind of exception it will be used internally in AccountController
    //others will be hidden in workers
    //This class is some kind of bunch of 'events'(methods)
    public class AccountNotificationMethods
    {
        #region Constructors
        public AccountNotificationMethods(MyDbContext context)
        {
            Construct(context);
        }

        void Construct(MyDbContext context)
        {
            Context = context;

            Emailer = new AccountEmailMethods();

            FASigns = new FontAwesomeRenderer();
            GISigns = new GlyphiconRenderer();
            HtmlSigns = new HtmlSignsRenderer();
        }
        #endregion

        #region Private Fields
        private AccountEmailMethods Emailer { get; set; }

        MyDbContext Context { get; set; }

        FontAwesomeRenderer FASigns { get; set; }
        GlyphiconRenderer GISigns { get; set; }
        HtmlSignsRenderer HtmlSigns { get; set; }
        #endregion

        #region Public Methods

        #region Registration and Confirmation
        public void OnUserRegistered(ApplicationUser user, string callBackUrl)
        {
            Emailer.OnUserRegistered(user, callBackUrl);
        }

        public void OnUserRegisteredFromSocialProvider(ApplicationUser user, string callBackUrl, string providerName)
        {
            Emailer.OnUserRegisteredFromSocialProvider(user, callBackUrl, providerName);
        }

        public void OnUserConfirmedEmail(string userGuid)
        {
            Notification N = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                Type = NotificationType.Success,
                UserGuid = userGuid,
                Title = AccountRes.RegisteredTitle,
                Text = string.Format(AccountRes.RegisteredTextFormat, UISettings.RegistrationBonus, HtmlSigns.Rouble)
                + $" {AccountRes.RuntaskerWish}",
                Link = new HtmlLink
                (
                    hrefParam: "/Orders/Create",
                    textParam: $"{GISigns.PlusSign} {AccountRes.RegisteredButtonText}",
                    buttonSizeParam: HtmlButtonSize.Large,
                    buttonTypeParam: HtmlButtonType.Success
                ).ToString()
            };
            Context.Notifications.Add(N);
            Context.SaveChanges();
        }

        public void OnUserRegisteredFromInvitation(ApplicationUser user, Invitation I)
        {
            Notification inviterN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                Type = NotificationType.Success,
                UserGuid = I.SenderGuid,
                Title = $"{AccountRes.FromInvitationTitle1} {user.Email} {AccountRes.FromInvitationTitle2}",
                Text = $"{AccountRes.FromInvitationText1} {UISettings.RegistrationBonus}{FASigns.Rouble} {AccountRes.FromInvitationText2} "
                + AccountRes.RuntaskerWish,
                Link = null
            };
            Context.Notifications.Add(inviterN);
            Context.SaveChanges();
        }
        #endregion

        #region Other methods
        public void OnUserForgottenPassword(ApplicationUser user, string callBackUrl)
        {
            Emailer.OnUserForgottenPass(user, callBackUrl);
        }
        #endregion

        #endregion
    }
}
