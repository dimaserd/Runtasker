using System.Collections.Generic;
using Runtasker.Logic.Entities;
using System.Data.Entity;
using Logic.Extensions.Repositories.Interfaces;

namespace Runtasker.Logic.Repositories
{
    public class NotificationRepository : BaseRepository, IRepository<Notification>, IRepositoryExtensive<Notification>
    {
        #region Constructors
        public NotificationRepository(MyDbContext context) : base(context)
        {

        }

        
        #endregion

        #region IRepository Methods
        public void Add(Notification notification)
        {
            db.Notifications.Add(notification);
        }

        public void Remove(int id)
        {
            Notification notification = db.Notifications.Find(id);
            if (notification != null)
            {
                db.Notifications.Remove(notification);
            }
        }

        public Notification GetItem(int id)
        {
            return db.Notifications.Find(id);
        }

        public IEnumerable<Notification> GetList()
        {
            return db.Notifications;
        }

        public void RemoveRange(IEnumerable<Notification> items)
        {
            db.Notifications.RemoveRange(items);
        }

        public void AddRange(IEnumerable<Notification> items)
        {
            db.Notifications.AddRange(items);
        }

        public void Update(Notification item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        
        #endregion
    }
}
