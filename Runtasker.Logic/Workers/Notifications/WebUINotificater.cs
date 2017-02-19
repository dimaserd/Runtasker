using Runtasker.Logic.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers.Notifications
{
    public class WebUINotificater : IDisposable
    {
        #region Constructors
        public WebUINotificater(string userGuid)
        {
            UserGuid = userGuid;
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
        
        private string UserGuid { get; set; }
        #endregion

        #region Public Methods

        //returned value could be null
        public Notification GetNotification()
        {
            using (MyDbContext context = new MyDbContext())
            {
                Notification Note = context.Notifications.FirstOrDefault(n => n.UserGuid == UserGuid);
                if (Note != null)
                {
                    context.Notifications.Remove(Note);
                    context.SaveChanges();
                }
                return Note;
            }
        }

        public async Task<Notification> GetNotificationAsync()
        {
            using (MyDbContext context = new MyDbContext())
            {
                Notification Note = context.Notifications.FirstOrDefault(n => n.UserGuid == UserGuid);
                if (Note != null)
                {
                    context.Notifications.Remove(Note);
                    await context.SaveChangesAsync();
                }
                return Note;
            }
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
