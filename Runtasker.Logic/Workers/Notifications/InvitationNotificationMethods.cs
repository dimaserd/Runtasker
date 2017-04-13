using HtmlExtensions.StaticRenderers;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Email;
using Runtasker.Settings;
using System.Linq;

namespace Runtasker.Logic.Workers.Notifications
{
    /// <summary>
    /// Здесь вообще ничего не готово
    /// </summary>
    public class InvitationNotificationMethods
    {
        #region Конструкторы
        public InvitationNotificationMethods(MyDbContext context)
        {
            Construct(context);
        }

        void Construct(MyDbContext context)
        {
            Context = context;

            Emailer = new InvitationEmailMethods();
        }
        #endregion

        #region Свойства
        MyDbContext Context { get; set; }

        InvitationEmailMethods Emailer {get;set;}
        #endregion

        #region Методы по событиям
        public void OnUserSentAnInvitation(Invitation I)
        {
            Notification invitatorN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                Title = $"You have sent an invitation to {I.ReceiverEmail}!",
                Text = "When user registered and got his first order finished, you will get your "
                + $"{UISettings.RegistrationBonus}{HtmlSigns.Rouble}. Best wishes from Runtasker team!",
                Type = NotificationType.Success,
                UserGuid = I.SenderGuid,
                Link = null
            };
            Context.Notifications.Add(invitatorN);
            Context.SaveChanges();

            Emailer.OnUserSentAnInvitation(I.ReceiverEmail, GetInvitorEmail(I), I.Id);
            
        }

        public void OnInvitedUserRegistered(ApplicationUser registeredUser, Invitation I)
        {
            
            Notification invitatorN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                Title = $"User {registeredUser.Email} registered through your invitation!",
                Text = $"When {registeredUser.Email} got his first order finished, you will get your "
                + $"{UISettings.RegistrationBonus}{HtmlSigns.Rouble}. Best wishes from Runtasker team!",
                Type = NotificationType.Success,
                UserGuid = I.SenderGuid,
                Link = null
            };
        }

        public void OnUserSentAnInvitationToHimSelf(Invitation I)
        {
            Notification N = new Notification
            {
                AboutType = NotificationAboutType.Balance,
                Title = $"Nice try, nice try!",
                Text = $"But we have predicted this scenario! Get your 50{HtmlSigns.Rouble} from Runtasker! "
                + "If you spot any error, or something! Please contact us! Best wishes from Runtasker team!",
                Type = NotificationType.Success,
                UserGuid = I.SenderGuid,
                Link = null
            };
            Context.Notifications.Add(N);
            Context.SaveChanges();
        }
        #endregion

        #region Вспомогательные методы
        public string GetInvitorEmail(Invitation I)
        {
            return Context.Users.FirstOrDefault(u => u.Id == I.SenderGuid).Email;
        }
        #endregion
    }
}
