using Logic.Extensions.Models;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Email;
using Runtasker.Resources.Notifications.FileMethods;
using System;

namespace Runtasker.Logic.Workers.Notifications
{
    public class FileControllerNotificationMethods : IDisposable
    {
        #region Constructors
        public FileControllerNotificationMethods(string userGuid, MyDbContext db)
        {
            UserGuid = userGuid;
            Db = db;
        }
        #endregion

        #region Properties
        string UserGuid { get; set; }

        MyDbContext Db { get; set; }
        #endregion

        #region Methods like Events
        public void OnCustomerTriedToDownloadSolution(WorkerResult result, Order order)
        {
            if(order.Status == OrderStatus.Paid)
            {
                Notification N = new Notification
                {
                    AboutType = NotificationAboutType.Ordinary,
                    UserGuid = UserGuid,
                    Type = (result.Succeeded) ? NotificationType.Success : NotificationType.Warning,
                    Title = (result.Succeeded) ? $"{FileNotRes.DownloadSolutionTitle1}"
                : $"{FileNotRes.DownloadErrorTitle1}",
                    Text = (result.Succeeded) ? $"{FileNotRes.DownloadSolutionText1} {FileNotRes.BestWishes}"
                : result.ErrorsList[0],
                    Link = null
                };
                
                Db.Notifications.Add(N);
                Db.SaveChanges();

                //Remaster this shit
                ApplicationUser customer = Db.Users.Find(order.UserGuid);

                new CustomerEmailMethods().OnCustomerDownloadedAnOrderSolution(customer, order);
                return;
            }
            

        }


        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        public virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                    Db.Dispose();
                    Db = null;
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~FileControllerNotificationMethods() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        void IDisposable.Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
