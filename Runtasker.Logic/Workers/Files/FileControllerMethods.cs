using Logic.Extensions.Models;
using Logic.Extensions.Namers;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Notifications;
using System;
using System.Linq;

namespace Runtasker.Logic.Workers.Files
{
    public class FileControllerMethods : FileWorkerBase, IDisposable
    {
        #region Constructors
        public FileControllerMethods(string userGuid, MyDbContext db) : base ()
        {
            Construct(userGuid, db);
        }

        void Construct(string userGuid, MyDbContext db)
        {
            UserGuid = userGuid;
            //поле с контекстом данных
            _db = db;
            Notificater = new FileControllerNotificationMethods(userGuid, Db);
            Namer = new AttachmentNamer();
            
            
        }
        #endregion

        #region Fields
        MyDbContext _db;
        #endregion

        #region Properties
        string UserGuid { get; set; }

        MyDbContext Db
        {
            get
            {
                return _db;
            }
        }

        FileControllerNotificationMethods Notificater { get; set; }

        //For getting marks in attachmnent
        AttachmentNamer Namer { get; set; }
        #endregion

        #region Public Methods
        public WorkerResult GetDownloadSolutionResult(int orderId)
        {
            WorkerResult result = null;
            
            Order order = Db.Orders.FirstOrDefault(o => o.UserGuid == UserGuid
                && o.Id == orderId);


            if (order == null)
            {
                result = new WorkerResult("Order which solution you attempted to download not found!");
            }

            if (order.Status == OrderStatus.Paid 
                    || order.Status == OrderStatus.Downloaded
                    || order.Status == OrderStatus.Appreciated)
            {
                  result = new WorkerResult
                  {
                      Succeeded = true
                  };
            }

            if (result == null)
            {
                 result = new WorkerResult("Be patient! Your order is not executed yet!");
            }


            //Notification methods
            Notificater.OnCustomerTriedToDownloadSolution(result, order);
        
            //Изменения полей в заказе должны идти после того как 
            //будут отправлены уведомления 
            //так как метод выше используюет свойство статус заказа
            if (order.Status == OrderStatus.Paid)
            {
                 //если пользователь в первый раз скачивает решение заказа
                 //и его заказ оплачен значит он сможет скачать решение заказа
                 //соотвественно меняем значение в базе
                 order.Status = OrderStatus.Downloaded;
            }
            
            Db.SaveChanges();
            
            return result;
        }

        public Attachment GetOrderSolutionAttachment(int orderId)
        {
            string mark = Namer.Mark.GetForOrderSolution(orderId);
            
            Attachment solution = Db.Attachments.FirstOrDefault(a => a.Mark == mark);
            return solution;
                  
        }


        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(_db != null)
                    {
                        _db.Dispose();
                        _db = null;
                    }
                    if(Notificater != null)
                    {
                        Notificater.Dispose(disposing);
                        Notificater = null;
                    }
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~FileControllerMethods() {
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
