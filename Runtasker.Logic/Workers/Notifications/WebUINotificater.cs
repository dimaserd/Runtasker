using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers.Notifications
{
    public class WebUINotificater : IDisposable
    {
        #region Constructors
        public WebUINotificater(string userGuid, IMyDbContext context)
        {
            UserGuid = userGuid;
            Context = context;
            Preparations();
        }
        #endregion

        #region Preparation Methods
        //Removing all notifications that were already seen
        private void Preparations()
        {
            
        }
        #endregion

        #region Private Properties
        IMyDbContext Context { get; set; }
        private string UserGuid { get; set; }
        #endregion

        #region Public Methods

        //returned value could be null
        public Notification GetNotification()
        {
            Notification Note = Context.Notifications.FirstOrDefault(n => n.UserGuid == UserGuid && n.Status == NotificationStatus.Unseen);
            if (Note != null)
            {
                Note.Status = NotificationStatus.Seen;

                Context.SaveChanges();
            }
            return Note;
        }

        public async Task<Notification> GetNotificationAsync()
        {

            Notification Note = Context.Notifications.FirstOrDefault(n => n.UserGuid == UserGuid && n.Status == NotificationStatus.Unseen);
            if (Note != null)
            {
                Note.Status = NotificationStatus.Seen;

                await Context.SaveChangesAsync();
            }
            return Note;

        }
        #endregion

        #region IDisposable
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
