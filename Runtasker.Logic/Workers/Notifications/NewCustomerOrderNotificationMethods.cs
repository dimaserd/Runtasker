using HtmlExtensions.Renderers;
using Runtasker.LocaleBuilders.Models;
using Runtasker.LocaleBuilders.Notification;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Repositories;
using Runtasker.Logic.Workers.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers.Notifications
{
    //класс его типа должны работать с репозиториями и писать 
    //запросы в базу данных
    public class NewCustomerOrderNotificationMethods : IDisposable
    {
        #region Contructors
        //по этому в своем конструкторе они принимают репозиторий
        //все что им нужно для добавления запросов
        //потому что запросы идут в базу из класса работы (Work)
        //а формируются здесь
        public NewCustomerOrderNotificationMethods(MyDbContext db)
        {
            //Создаем репозиторий из контекста которые пришел к нам
            //из узправляющего класса, для того чтобы в течение одного
            //запроса из контроллера было выполнено как можно меньше запросов
            //к базе данных
            Repository = new NotificationRepository(db);

            Construct();
        }

        void Construct()
        {
            Emailer = new CustomerEmailMethods();
            FASigns = new FontAwesomeRenderer();
            GISigns = new GlyphiconRenderer();

            ModelBuilder = new CustomerOrderNotificationBuilder();
        }
        #endregion

        #region Private Properties
        NotificationRepository Repository { get; set; }

        CustomerEmailMethods Emailer { get; set; }

        FontAwesomeRenderer FASigns { get; set; }
        GlyphiconRenderer GISigns { get; set; }

        CustomerOrderNotificationBuilder ModelBuilder { get; set; }
        #endregion

        #region Public Methods
        public void OnCustomerAddedNewDescription(Order order)
        {
            //получение локализованного текста
            ForNotification model = ModelBuilder.AddedDescription(order.Id);

            Notification customerN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                Title = model.Title,
                Text = model.Text,
                Type = NotificationType.Info,
                UserGuid = order.UserGuid,
                Link = null
            };

            Notification performerN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                Text = $"В заказе №{order.Id} было изменено описание, проверьте",
                Title = $"Пользователь изменил описание заказа по вашей просьбе! "
                + $"Проверьте и помните, что его нужно выполнить {order.FinishDate.ToString("d MMM yyyy")}",
                Type = NotificationType.Info,
                UserGuid = GetPerformerGuid(),
                Link = null
            };
            
            Repository.Add(customerN);
            Repository.Add(performerN);
                
            Emailer.OnCustomerChangedDescription(order, GetPerformerEmail());
        }
        #endregion

        #region Help Methods
        string GetPerformerEmail()
        {
            return "dimaserd84@gmail.com";
        }

        string GetPerformerGuid()
        {
            string email = GetPerformerEmail();
            
            return Repository.db.Users.FirstOrDefault(u => u.Email == email).Id;
            
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        //Удалять нужно только то что создано внутри этого класса
        //то что передано из вне с помощью контсруктора трогать не нужно
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Emailer.Dispose();
                    ModelBuilder.Dispose();
                    Repository.Dispose(disposing);
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.
                Emailer = null;
                ModelBuilder = null;
                Repository = null;

                disposedValue = true;

            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~NewCustomerOrderNotificationMethods() {
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
