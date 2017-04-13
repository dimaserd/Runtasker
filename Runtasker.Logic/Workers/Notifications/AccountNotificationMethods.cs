using HtmlExtensions.HtmlEntities;
using HtmlExtensions.Renderers;
using HtmlExtensions.StaticRenderers;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Email;
using Runtasker.Resources.Notifications.AccountMethods;
using Runtasker.Settings;

namespace Runtasker.Logic.Workers.Notifications
{
    /// <summary>
    /// используется только в Account контроллере
    /// </summary>
    public class AccountNotificationMethods
    {
        #region Конструктор
        public AccountNotificationMethods(MyDbContext context)
        {
            Context = context;
            Construct();
        }

        void Construct()
        {
            Emailer = new AccountEmailMethods();
        }
        #endregion

        #region Свойства
        AccountEmailMethods Emailer { get; set; }

        MyDbContext Context { get; set; }
        #endregion

        #region Методы

        #region Регистрация

        public void OnUserRegistered(ApplicationUser user, string callBackUrl)
        {
            Emailer.OnUserRegistered(user, callBackUrl);
        }

        public void OnUserRegisteredFromSocialProvider(ApplicationUser user, string callBackUrl, string providerName)
        {
            Emailer.OnUserRegisteredFromSocialProvider(user, callBackUrl, providerName);
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

        #region Другие методы

        public void OnUserForgottenPassword(ApplicationUser user, string callBackUrl)
        {
            Emailer.OnUserForgottenPass(user, callBackUrl);
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
        #endregion

        #endregion
    }
}
