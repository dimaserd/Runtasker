using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Repositories;
using Runtasker.Logic.Workers.Notifications;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers.Orders
{
    //Класс который работает с базой с сущностью Order
    public class NewCustomerOrderWorker : IDisposable
    {
        #region Constructors
        public NewCustomerOrderWorker(string userGuid, MyDbContext db)
        {
            UserGuid = userGuid;
            Repository = new OrderRepository(db);
        }
        #endregion

        //содержит поля для дочерних классов отсюда в Dispose
        //мы будем уничтожать объекты созданные в поле для 
        //высвобождения памяти 
        #region Fields
        NewCustomerOrderNotificationMethods _notificater;
        #endregion

        #region Properties

        //репозиторий управляется из вне классом работы
        //поэтому внутри класса не должно быть никаких 
        //saveChanges которые пишут запросы
        OrderRepository Repository {get;set;}

        //Id пользователя которое было получено из контроллера
        //по нему нужно писать запросы к базе данных
        //здесь работа с заказами оплатой и прочим
        string UserGuid { get; set; }

        //Другие классы содержащие работу которые управляются внутри
        //этого класса, все они содержат общий контекст данных
        //а значит все будет решено в один запрос
        #region Workers
        NewCustomerOrderNotificationMethods Notificater
        {
            get
            {
                if(_notificater == null)
                {
                    _notificater = new NewCustomerOrderNotificationMethods(Repository.db);
                }
                return _notificater;
            }
        }
        #endregion

        //свойство которое создано для сокращения запросов к 
        //базе данных чтобы не шло несколько запросов для получения 
        //баланса и прочего а в пределах жизни объекта можно было просто 
        //обращаться к свойству
        ApplicationUser Customer { get; set; }

        #endregion

        #region Public Methods

        #region Add Description Methods

        public AddDescriptionModel GetAddDescriptionModel(int id)
        {
            Order order = Repository.db.Orders.FirstOrDefault
                (o => o.Id == id
                    && o.UserGuid == UserGuid
                    && o.ErrorType == OrderErrorType.NeedDescription
                );

            if (order == null)
            {
                return null;
            }

            return new AddDescriptionModel
            {
                OrderId = order.Id
            };
        }

        public async Task<Order> AddDescriptionToOrder(AddDescriptionModel model)
        {
            Order order = Repository.db.Orders.FirstOrDefault
                (o => o.Id == model.OrderId
                    && o.UserGuid == UserGuid
                    && o.ErrorType == OrderErrorType.NeedDescription
                );

            if (order == null)
            {
                return null;
            }

            order.Description = model.Description;
            order.ErrorType = OrderErrorType.None;
            order.Status = OrderStatus.New;

            //обновление свойств объекта заказа
            Repository.Update(order);

            Notificater.OnCustomerAddedNewDescription(order);

            Repository.Save();
            return order;
        }
        #endregion

        #endregion

        //содержит вспомогательные методы типо 
        //получения баланса пользователя
        #region Help Methods

        #region Balance methods

        //выдает баланс текущего пользователя из свойства
        //Customer-заказчика если оно пустое то устанавливает его
        public decimal? GetUserBalance()
        {
            if (Customer == null)
            {
                Customer = Repository.db.Users.FirstOrDefault(u => u.Id == UserGuid);
            }
            return Customer.Balance;
        }

        #endregion

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                    if (Repository != null)
                    {
                        Repository.Dispose(disposing);
                    }
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.
                Repository = null;

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~NewCustomerOrderWorker() {
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
