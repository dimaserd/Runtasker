using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Attachments;
using Runtasker.Logic.Workers.Email;
using System;

namespace Runtasker.Logic.Workers
{
    public class ContactWorker : IDisposable
    {
        #region Constructors
        public ContactWorker(string userGuid)
        {
            Construct(userGuid);
        }

        void Construct(string userGuid)
        {
            UserGuid = userGuid;

            Emailer = new ContactEmailMethods();
            Attacher = new ContactAttachmentMethods();
        }
        #endregion


        string UserGuid { get; set; }
        private ContactEmailMethods Emailer { get; set; }
        private ContactAttachmentMethods Attacher { get; set; }

        public void OnContactMessageReceived(ContactViewModel model)
        {
            string attachmentsLink = Attacher.GetAttachmentsLink(model.Files);
            Emailer.Contact(model, attachmentsLink);

            if(UserGuid != null)
            {
                Notification N = new Notification
                {

                };

            }
            
        }

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                    if(Emailer != null)
                    {
                        Emailer.Dispose();
                        Emailer = null;
                    }
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~ContactWorker() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
