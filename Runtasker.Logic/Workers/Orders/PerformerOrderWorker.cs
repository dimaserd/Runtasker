using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Files;
using Runtasker.Logic.Workers.Notifications;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Logic.Extensions.Models;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers.Orders
{
    public class PerformerOrderWorker
    {
        #region Constructors

        public PerformerOrderWorker(string userGuid)
        {
            Construct(null, null, null, userGuid);
        }

        public PerformerOrderWorker(MyDbContext context, string userGuid)
        {
            Construct(context, null, null, userGuid);
        }


        void Construct(MyDbContext context, PerformerFileMethods filer, PerformerOrderNotificationMethods notificater, string userGuid)
        {
            Context = context ?? new MyDbContext();
            //passing Context for attachments worker it need it
            Filer = filer ?? new PerformerFileMethods(Context);
            UserGuid = userGuid;
            Notificater = new PerformerOrderNotificationMethods(Context, userGuid);
        }
        #endregion

        #region Properties

        public PerformerOrderNotificationMethods Notificater { get; private set; }
        MyDbContext Context { get; set; }
        PerformerFileMethods Filer { get; set; }
        string UserGuid { get; set; }

        #endregion

        #region Public Methods

        #region Value Methods
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

        //Changes sum of an order than sends notifications
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
            //changing fields
            order.Sum = model.Sum;
            order.Status = OrderStatus.Valued;
            Context.SaveChanges();

            //notification methods
            Notificater.OnPerformerValuedAnOrder(model.OrderId);
            return order;
        }
        #endregion

        #region AddError Methods

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

        #region Choose Methods
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
                WorkerResult result = new WorkerResult
                {
                    Succeeded = false
                };
                result.ErrorsList.Add("Оцененных заказов с указанным Id не найдено!");

                return result;
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

        #region Solve Methods
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

            //Saving files and writing attachments
            Filer.OnPerformerSolvedAnOrder(model);
            order.Status = OrderStatus.Finished;
            Context.SaveChanges();

            //Notifications methods
            Notificater.OnPerformerExecutedAnOrder(order);

            return new WorkerResult
            {
                Succeeded = true
            };
        }
        #endregion

        public async Task<List<Order>> GetOrdersAsync()
        {
            OtherUserInfo performerInfo = await Context.OtherUserInfos.FirstOrDefaultAsync(i => i.UserId == UserGuid);
            List<Order> allOrders =  Context.Orders.Include(x => x.Messages).ToList();

            return performerInfo
                .GetOrdersForPerformerByInfo(allOrders).ToList();
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

        #region Help Methods
        
        #endregion
    }
}
