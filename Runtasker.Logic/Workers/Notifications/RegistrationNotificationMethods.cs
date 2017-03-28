using HtmlExtensions.HtmlEntities;
using HtmlExtensions.StaticRenderers;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Email;

namespace Runtasker.Logic.Workers.Notifications
{
    public class RegistrationNotificationMethods
    {
        #region Contructors
        public RegistrationNotificationMethods(MyDbContext context)
        {
            Construct(context);
        }

        void Construct(MyDbContext context)
        {
            Context = context;
            Emailer = new EmailWorkerBase();
            
        }
        #endregion

        #region Private Fields
        MyDbContext Context { get; set; }
        EmailWorkerBase Emailer { get; set; }
        
        #endregion

        #region Public Methods
        public void OnUserRegisteredFromSocialNetwork(ApplicationUser user)
        {
            Notification N = new Notification
            {
                Title = "Congratulations! You have successfully registered!",
                Text = $"We gift {user.Balance} {FASigns.Rouble} you as a bonus! Go and create your first order! ",
                Type = NotificationType.Info,
                UserGuid = user.Id,
                Link = new HtmlLink
                (
                    hrefParam: $"/Orders/Create",
                    textParam: $"{GISigns.PlusSign} Create Order",
                    buttonSizeParam: HtmlButtonSize.Large,
                    buttonTypeParam: HtmlButtonType.Success
                ).ToString()
            };
            Context.Notifications.Add(N);
            Context.SaveChanges();
        }
        #endregion
    }
}
