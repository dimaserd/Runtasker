using HtmlExtensions.Renderers;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Email;
using System.Linq;

namespace Runtasker.Logic.Workers.Notifications
{
    public class InvitationNotificationMethods
    {
        #region Constructors
        public InvitationNotificationMethods(MyDbContext context)
        {
            Construct(context);
        }

        void Construct(MyDbContext context)
        {
            Context = context;

            Emailer = new InvitationEmailMethods();

            GISigns = new GlyphiconRenderer();
            FASigns = new FontAwesomeRenderer();
        }
        #endregion

        #region Properties
        MyDbContext Context { get; set; }

        InvitationEmailMethods Emailer {get;set;}

        GlyphiconRenderer GISigns { get; set; }
        FontAwesomeRenderer FASigns { get; set; }
        #endregion

        #region Public Methods like Events
        public void OnUserSentAnInvitation(Invitation I)
        {
            Notification invitatorN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                Title = $"You have sent an invitation to {I.ReceiverEmail}!",
                Text = "When user registered and got his first order finished, you will get your "
                + $"300{FASigns.Rouble}. Best wishes from Runtasker team!",
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
                + $"300{FASigns.Rouble}. Best wishes from Runtasker team!",
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
                Text = $"But we have predicted this scenario! Get your 50{FASigns.Rouble} from Runtasker! "
                + "If you spot any error, or something! Please contact us! Best wishes from Runtasker team!",
                Type = NotificationType.Success,
                UserGuid = I.SenderGuid,
                Link = null
            };
            Context.Notifications.Add(N);
            Context.SaveChanges();
        }
        #endregion

        #region Help Methods
        public string GetInvitorEmail(Invitation I)
        {
            return Context.Users.FirstOrDefault(u => u.Id == I.SenderGuid).Email;
        }
        #endregion
    }
}
