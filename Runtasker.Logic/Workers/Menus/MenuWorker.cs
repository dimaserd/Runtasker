using Runtasker.Logic.Entities;
using Runtasker.Logic.Models.Orders;
using Runtasker.Logic.Statics.Actions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers.Menus
{
    /// <summary>
    /// Класс инкапсулирующий некоторые запросы к апи контролеру меню ()
    /// </summary>
    public class MenuWorker : IDisposable
    {
        #region Конструкторы
        public MenuWorker(MyDbContext db, string userId)
        {
            Db = db;
            UserId = userId;
        }
        #endregion

        #region Свойства
        public MyDbContext Db { get; set; }

        public string UserId { get; set; }
        #endregion

        #region Методы

       
        public async Task<UserOrdersInfo> GetUserOrdersInfoForCustomerAsync()
        {
            
            List<OrderMessagesInfo> infos = (await Db.Orders.Where(o => o.UserGuid == UserId
             && o.Status != OrderStatus.DeletedByCustomer
               && o.Status != OrderStatus.DeletedByAdmin).ToListAsync())
               .Select(x => new OrderMessagesInfo
               {
                   Id = x.Id,
                   ActionLink = CustomerActions.GetActionLinkFromOrder(x),
                   UnreadCount = x.Messages.Count(m => m.ReceiverId == UserId && m.Status == MessageStatus.New)
               }).ToList();

            

            return new UserOrdersInfo
            {
                UnreadCount = infos.Sum(x => x.UnreadCount),
                HasOrders = infos.Count > 0,
                OrderMessageInfos = infos
            };
        }

        public async Task<UserOrdersInfo> GetUserOrdersInfoForAdminAsync()
        {
            

            List<OrderMessagesInfo> infos = (await Db.Orders.Where(o => o.Status != OrderStatus.DeletedByCustomer
               && o.Status != OrderStatus.DeletedByAdmin
               && o.Status != OrderStatus.Appreciated).ToListAsync())
               .Select(x => new OrderMessagesInfo
               {
                   Id = x.Id,
                   ActionLink = AdminActions.GetActionLinkFromOrder(x),
                   UnreadCount = x.Messages.Count(m => m.ReceiverId == UserId && m.Status == MessageStatus.New)
               }).ToList();


            return new UserOrdersInfo
            {
                UnreadCount = infos.Sum(x => x.UnreadCount),
                HasOrders = infos.Count > 0,
                OrderMessageInfos = infos
            };
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
                    // TODO: освободить управляемое состояние (управляемые объекты).
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        
        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
