using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Files;
using Runtasker.Logic.Workers.Notifications;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Logic.Extensions.Models;
using System.Threading.Tasks;
using Runtasker.Logic.Models.ManageModels;

namespace Runtasker.Logic.Workers.Orders
{
    public class PerformerOrderWorker : OrdersWorkerBase
    {
        #region Конструктор

        public PerformerOrderWorker(MyDbContext context, string userGuid) : base(context, userGuid)
        {
            Construct();
        }

        void Construct()
        {
            Filer = new PerformerFileMethods(Context);
            Notificater = new PerformerOrderNotificationMethods(Context, UserGuid);
        }

        #endregion

        #region Свойства

        PerformerOrderNotificationMethods Notificater { get; set; }
        PerformerFileMethods Filer { get; set; }
        
        #endregion

        #region Публичные методы

        #region Методы оценки заказа

        public ValueOrderModel GetValueOrderModel(int id)
        {
            Order order = Context.Orders.FirstOrDefault(o => o.Id == id && o.Sum == 0 && o.Status == OrderStatus.New);
            if(order == null)
            {
                return null;
            }

            return new ValueOrderModel
            {
                OrderId = order.Id
            };
        }
  
        public Order ValueOrder(ValueOrderModel model)
        {
            Order order = Context.Orders.FirstOrDefault(
                o => o.Id == model.OrderId
                && o.Sum == 0 
                && o.Status == OrderStatus.New);

            if(order == null)
            {
                return null;
            }
            //изменяем свойства заказа
            order.Sum = model.Sum;
            order.Status = OrderStatus.Estimated;

            Context.Entry(order).State = EntityState.Modified;

            //записываем изменения в базе данных
            Context.SaveChanges();

            //Вызываем методы создающие уведомления
            if(order.WorkType == OrderWorkType.OnlineHelp)
            {
                //для заказа являющегося онлайн помощью
                Notificater.OnPerformerEstimatedOnlineHelp(order);
            }
            else
            {
                //для обычного заказа
                Notificater.OnAdminEstimatedAnOrder(order);
            }

            //возвращаем заказ
            return order;
        }

        #endregion

        #region Добавление ошибок

        public AddErrorToOrderModel GetAddErrorToOrderModel(int id)
        {
            Order order = Context.Orders.FirstOrDefault
            (
                o => o.Id == id
                && o.Status == OrderStatus.New
            );
            if(order == null)
            {
                return null;
            }

            return new AddErrorToOrderModel
            {
                OrderId = order.Id
            };
        }

        public Order AddErrorToOrder(AddErrorToOrderModel model)
        {
            Order order = Context.Orders.FirstOrDefault
            (
                o => o.Id == model.OrderId
                && o.Status == OrderStatus.New
            );

            if(order == null)
            {
                return null;
            }

            order.ErrorType = model.ErrorType;
            order.Status = OrderStatus.HasError;
            Context.SaveChanges();

            //Then notification methods go
            Notificater.OnPerformerAddedErrorToOrder(order);
            return order;

        }

        #endregion

        #region Выбор заказа
        //For now performers can select orders when orders are halfpaid
        public ChooseOrderModel GetChooseOrderModel(int id)
        {
            Order order = Context.Orders.FirstOrDefault(o => o.Id == id
            && o.Status == OrderStatus.HalfPaid
            && o.ErrorType == OrderErrorType.None);

            if(order == null)
            {
                return null;
            }

            return new ChooseOrderModel
            {
                OrderId = order.Id,
                PerformerGuid = UserGuid
            };
        }

        public WorkerResult ChooseOrder(ChooseOrderModel model)
        {
            Order order = Context.Orders.FirstOrDefault(o => o.Id == model.OrderId
            && o.Status == OrderStatus.HalfPaid
            && o.ErrorType == OrderErrorType.None);

            if(order == null)
            {
                return new WorkerResult("Оцененных заказов с указанным Id не найдено!");
            }

            //изначально заказ создается с одинаковыми Id
            if(order.PerformerGuid != order.UserGuid)
            {
                return new WorkerResult("Данный заказ уже занят другим исполнителем!");
            }

            order.PerformerGuid = model.PerformerGuid;
            order.Status = OrderStatus.Executing;
            Context.SaveChanges();

            //Notifications Methods
            Notificater.OnPerformerStartedExecutingAnOrder(order);
            return new WorkerResult
            {
                Succeeded = true
            };

        }
        #endregion

        #region Решение заказа
        public SolveOrderModel GetSolveOrderModel(int orderId)
        {
            Order order = Context.Orders.FirstOrDefault(o => o.Id == orderId
            && o.PerformerGuid == UserGuid
            && o.Status == OrderStatus.Executing);

            if(order == null)
            {
                return null;
            }

            return new SolveOrderModel
            {
                OrderId = order.Id,
                PerformerGuid = UserGuid
            };
        }

        public WorkerResult SolveOrder(SolveOrderModel model)
        {
            Order order = Context.Orders.FirstOrDefault(o => o.Id == model.OrderId
            && o.PerformerGuid == UserGuid
            && o.Status == OrderStatus.Executing);

            if(order == null)
            {                
                return new WorkerResult("Выполняемый вами заказ не найден!");
            }

            if(order.Status == OrderStatus.Finished)
            {
                return new WorkerResult("Вы не можете загрузить еще одно решение!");
            }

            //Перезаписываем файлы связанные с заказом
            Filer.OnPerformerSolvedAnOrder(model);

            //изменяем свойство статуса заказа
            order.Status = OrderStatus.Finished;

            Context.Entry(order).State = EntityState.Modified;

            Context.SaveChanges();

            //Вызываю метод уведомлений
            Notificater.OnPerformerExecutedAnOrder(order);

            return new WorkerResult
            {
                Succeeded = true
            };
        }
        #endregion

        public async Task<List<Order>> GetOrdersAsync()
        {
            OtherUserInfo performerInfo = (await Context.Users.FirstOrDefaultAsync(x => x.Id == UserGuid)).GetOtherInfo();
            List<Order> allOrders = await Context.Orders.Include(x => x.Messages).ToListAsync();

            return performerInfo
                .GetOrdersBySpecialization(allOrders).ToList();
        }

        public Order ChooseOrder(int id)
        {
            Order order = Context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return null;
            }
            if (order.IsFree())
            {
                order.PerformerGuid = UserGuid;
                order.Status = OrderStatus.Executing;
                return order;
            }

            return null;

        }

        public string GetLinkToAllFilesFromOrder(int id)
        {
            Attachment at = Context.Attachments.FirstOrDefault(a => a.Mark == $"[all]{id}");
            return $"/File/DownloadByKey?key={at.Id}";
        }
        #endregion

        
    }
}
