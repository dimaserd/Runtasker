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
        public WebUINotificater(string userGuid, MyDbContext context)
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
        MyDbContext Context { get; set; }
        private string UserGuid { get; set; }
        #endregion

        #region Public Methods

        //returned value could be null
        public Notification GetNotification()
        {
            Notification Note = Context.Notifications.FirstOrDefault(n => n.UserGuid == UserGuid);
            if (Note != null)
            {
                Context.Notifications.Remove(Note);
                Context.SaveChanges();
            }
            return Note;
        }

        public async Task<Notification> GetNotificationAsync()
        {

            Notification Note = Context.Notifications.FirstOrDefault(n => n.UserGuid == UserGuid);
            if (Note != null)
            {
                Context.Notifications.Remove(Note);
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
