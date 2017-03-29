using HtmlExtensions.StaticRenderers;
using Logic.Extensions.Models;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Enumerations.OrderWorker;
using Runtasker.Logic.Handlers;
using Runtasker.Logic.Models;
using Runtasker.Logic.Models.Orders.Pay;
using Runtasker.Logic.Workers.Attachments;
using Runtasker.Logic.Workers.Files;
using Runtasker.Logic.Workers.Notifications;
using Runtasker.Logic.Workers.Payments;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers.Orders
{
    public class CustomerOrderWorker : OrdersWorkerBase
    {
        #region Constructors
        public CustomerOrderWorker(MyDbContext context, string userGuid) : base(userGuid)
        {
            Context = context;
            Construct();
        }

        void Construct()
        {            
            Filer = new CustomerFileMethods();
            Notificater = new CustomerOrderNotificationMethods(Context);
            Attachmenter = new CustomerAttachmentWorker();
            
            //здесь все упирается в UserManger
            Paymenter = new CustomerOrderPaymentMethods(UserGuid, Context);
            ErrorHandler = new CustomerOrderErrorEvents(UserGuid);
            
        }
        #endregion

        #region Properties
        MyDbContext Context { get; set; }

        CustomerOrderNotificationMethods Notificater { get; set; }

        CustomerFileMethods Filer { get; set; }

        CustomerAttachmentWorker Attachmenter { get; set; }

        CustomerOrderPaymentMethods Paymenter { get; set; }

        CustomerOrderErrorEvents ErrorHandler { get; set; }

        

        #region Internal properties for passing objects
        ApplicationUser Customer { get; set; }
        #endregion

        #endregion

        #region Help Methods

        public string GetAdminGuid()
        {
            
            return Context.Users.FirstOrDefault(u => u.Email == "dimaserd84@gmail.com").Id;
            
        }

        #region Balance methods

        public decimal? GetUserBalance()
        {
            using (MyDbContext context = new MyDbContext())
            {
                if (Customer == null)
                {
                    Customer = context.Users.FirstOrDefault(u => u.Id == UserGuid);
                }
                return Customer.Balance;
            }
        }

        #endregion

        public void CheckForAnInvitation(Order order)
        {
            Invitation I = Context.Invitations.FirstOrDefault(i => i.ReceiverGuid == order.UserGuid);

            if (I == null)
            {
                return;
            }

            if (I.Status == InvitationStatus.Paid)
            {
                return;
            }

            I.Status = InvitationStatus.Paid;
            Context.SaveChanges();

            //Payment methods
            Paymenter.OnInvitedCustomerFinishedOrder(I);

            //Notifications methods
            Notificater.OnInvitedCustomerRatedAnOrderSolution(I, 300, HtmlSigns.Rouble);

        }
        #endregion

        #region Public Methods

        #region Add Description Methods
        public AddDescriptionModel GetAddDescriptionModel(int id)
        {
            Order order = Context.Orders.FirstOrDefault
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
                OrderId = order.Id,
                Description = order.Description
            };
        }

        public Order AddDescriptionToOrder(AddDescriptionModel model)
        {
            Order order = Context.Orders.FirstOrDefault
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


            Notificater.OnCustomerAddedNewDescription(order);
            Context.SaveChanges();
            return order;

        }
        #endregion

        #region Add Files Methods

        public OrderAddFilesModel GetAddFilesModel(int id)
        {

            Order order = Context.Orders.FirstOrDefault(
                    o => o.Id == id
                    && o.Status == OrderStatus.HasError
                    && o.ErrorType == OrderErrorType.NeedFiles);

            if (order == null)
            {
                return null;
            }

            return new OrderAddFilesModel
            {
                OrderId = order.Id
            };

        }
        //TO DO Orders Files changing methods 
        public WorkerResult AddFilesToOrder(OrderAddFilesModel model)
        {
            Order order = Context.Orders.FirstOrDefault
                    (o => o.Id == model.OrderId
                     && o.ErrorType == OrderErrorType.NeedFiles
                     && o.Status == OrderStatus.HasError
                    );

            if (order == null)
            {
                return new WorkerResult("Order not found!");
            }


            //file methods
            WorkerResult filerResult = Filer.OnCustomerAddedNewFilesToOrder(model);
            if (!filerResult.Succeeded)
            {
                return filerResult;
            }

            order.Status = OrderStatus.New;
            order.ErrorType = OrderErrorType.None;

            Context.SaveChanges();
            //Notification methods
            Notificater.OnCustomerAddedNewFilesToOrder(order);
            return new WorkerResult
            {
                Succeeded = true
            };
        }
        #endregion

        #region Pay Methods

        #region PayHalf Methods
        public PayHalfModel GetPayHalfModel(int id)
        {
            Order order = Context.Orders.FirstOrDefault(
                    o => o.UserGuid == UserGuid
                    && o.Status == OrderStatus.Valued);

            if (order == null)
            {
                return null;
            }

            return new PayHalfModel
            {
                OrderId = order.Id,
                Sum = (order.Sum / 2),
                RequiredSum = (order.Sum / 2)

            };
        }

        public WorkerResult PayHalfOfOrder(PayHalfModel model)
        {
            Order order = Context.Orders.FirstOrDefault(
                    o => o.UserGuid == UserGuid
                    && o.Status == OrderStatus.Valued);

            if (order == null)
            {
                return new WorkerResult("No order matched!");
            }

            decimal sumThatUserNeedToPay = order.Sum / 2;

            if (model.Sum != sumThatUserNeedToPay)
            {
                return new WorkerResult("You should pay a half of an order!");
            }

            if (GetUserBalance() < sumThatUserNeedToPay)
            {
                //we don't have an error message because we will show a notification
                WorkerResult result = new WorkerResult
                {
                    Succeeded = false
                };

                ErrorHandler.OnCustomerTriedToPayWithoutMoney(Customer, order);

                return result;
            }

            //Changing order properties
            order.PaidSum = model.Sum;
            order.Status = OrderStatus.HalfPaid;
            Context.SaveChanges();

            //Payment methods
            Paymenter.OnCustomerPaidFirstHalfOfAnOrder(order);

            //Notifications methods
            Notificater.OnCustomerPaidHalfOfAnOrder(order);

            return new WorkerResult
            {
                Succeeded = true
            };
        }
        #endregion

        #region PayAnotherHalf methods

        public PayAnotherHalfModel GetPayAnotherHalfModel(int orderId)
        {
            Order order = Context.Orders.FirstOrDefault(o => o.Id == orderId
            && o.Status == OrderStatus.Finished
            && o.UserGuid == UserGuid);

            if (order == null)
            {
                return null;
            }

            return new PayAnotherHalfModel
            {
                OrderId = order.Id,
                Sum = order.PaidSum,
                RequiredSum = order.PaidSum

            };

        }

        public WorkerResult PayAnotherHalfOfAnOrder(PayAnotherHalfModel model)
        {
            Order order = Context.Orders.FirstOrDefault(o => o.Id == model.OrderId
            && o.Status == OrderStatus.Finished
            && o.UserGuid == UserGuid);

            if (order == null)
            {
                return new WorkerResult($"Finished Order №{model.OrderId} not found!");
            }

            if (model.Sum != order.Sum / 2)
            {
                return new WorkerResult($"You should pay {order.Sum / 2} roubles!");
            }

            if (GetUserBalance() < order.Sum / 2)
            {

                WorkerResult result = new WorkerResult
                {
                    Succeeded = false
                };

                ErrorHandler.OnCustomerTriedToPayWithoutMoney(Customer, order);

                return result;
            }
            //Changing order properties
            order.PaidSum += model.Sum;
            order.Status = OrderStatus.FullPaid;

            Context.SaveChanges();

            //Payment methods
            Paymenter.OnCustomerPaidSecondHalfOfAnOrder(order);

            //Notifications methods
            Notificater.OnCustomerPaidAnotherHalfOfAnOrder(order);


            return new WorkerResult
            {
                Succeeded = true
            };
        }
        #endregion

        #region Pay OnlineHelp methods

        public async Task<PayOnlineHelp> GetPayOnlineHelpModelAsync(int orderId)
        {
            Order order = await Context.Orders
                .FirstOrDefaultAsync(x => x.Id == orderId &&
                x.PaidSum == 0 && 
                x.UserGuid == UserGuid &&
                x.WorkType == OrderWorkType.OnlineHelp &&
                x.Status == OrderStatus.Valued
                );

            if(order != null)
            {
                return new PayOnlineHelp
                {
                    OrderId = order.Id,
                    Sum = order.Sum
                };
            }

            return null;
        }

        /// <summary>
        /// Оплата онлайн помощи
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<WorkerResult> PayOnlineHelpAsync(PayOnlineHelp model)
        {
            Order order = await Context.Orders
                .FirstOrDefaultAsync(x => x.Id == model.OrderId &&
                x.Sum == model.Sum &&
                x.PaidSum == 0 &&
                x.UserGuid == UserGuid &&
                x.WorkType == OrderWorkType.OnlineHelp &&
                x.Status == OrderStatus.Valued
                );

            if(order == null)
            {
                return new WorkerResult("Произошла ошибка при оплате онлайн-помощи!");
            }
            
            //регистрируем оплату
            Paymenter.OnCutomerPaidOnlineHelp(order);


            //изменяем данные о заказе
            order.Status = OrderStatus.FullPaid;
            order.PaidSum = model.Sum;

            //уведомления не сделаны
            Notificater.OnCustomerPaidOnlineHelp(order);
            

            await Context.SaveChangesAsync();

            return new WorkerResult
            {
                Succeeded = true
            };

        }


        #endregion

        #endregion

        #region Rating Methods
        public RatingOrderModel GetRatingOrderModel(int orderId)
        {
            Order order = Context.Orders.FirstOrDefault(o => o.Id == orderId
            && o.Status == OrderStatus.Downloaded
            && o.UserGuid == UserGuid);

            if (order == null)
            {
                return null;
            }

            return new RatingOrderModel
            {
                OrderId = order.Id
            };
        }

        public WorkerResult RatingOrder(RatingOrderModel model)
        {

            Order order = Context.Orders.FirstOrDefault(o => o.Id == model.OrderId
                && o.Status == OrderStatus.Downloaded
                && o.UserGuid == UserGuid);


            if (order == null)
            {
                return new WorkerResult("Order not found");
            }



            //changing order fields
            order.Status = OrderStatus.Appreciated;
            order.Comment = model.Text;
            order.Rating = model.Rating;
            Context.SaveChanges();

            //Checking for an invitation
            CheckForAnInvitation(order);

            //Notifications methods
            Notificater.OnCustomerRatedAnOrderSolution(order);

            return new WorkerResult
            {
                Succeeded = true
            };
        }
        #endregion

        #region GetMyOrders Methods
        public IEnumerable<Order> GetMyOrders()
        {
            using (MyDbContext context = new MyDbContext())
            {
                return context.Orders.Where(o => o.UserGuid == UserGuid).ToList();
            }
        }

        public async Task<IEnumerable<Order>> GetMyOrdersAsync()
        {
            using (MyDbContext context = new MyDbContext())
            {
                return await context.Orders.Where(o => o.UserGuid == UserGuid)
                    .Include(x => x.Messages).ToListAsync();
            }
        }
        #endregion

        #region Create Order Methods
        public Order CreateOrder(OrderCreateModel orderModel)
        {
            return CreateOrderSubMethod(OrderCreationType.Ordinary, orderModel);
        }

        public async Task<Order> CreateOrderAsync(OrderCreateModel orderModel)
        {
            return await CreateOrderSubMethodAsync(OrderCreationType.Ordinary, orderModel);
            
        }

        /// <summary>
        /// Вроде сделано
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Order> CreateOnlineHelpOrderAsync(OrderCreateModel model)
        {
            return await CreateOrderSubMethodAsync(OrderCreationType.Ordinary, model);
        }

        #region Sub Methods
        //в дальнейшем сделай так чтобы методы создания заказа
        //обращались к этому методу и его асинхронной копии
        public Order CreateOrderSubMethod(OrderCreationType creationType, OrderCreateModel orderModel)
        {
            using (MyDbContext context = new MyDbContext())
            {
                Order order = new Order
                {
                    Description = orderModel.Description,
                    FinishDate = orderModel.FinishDate,
                    Status = OrderStatus.New,
                    PublishDate = DateTime.Now,
                    WorkType = orderModel.WorkType,
                    Subject = orderModel.Subject,
                    OtherSubject = orderModel.OtherSubject,
                    UserGuid = UserGuid
                };
                context.Orders.Add(order);
                context.SaveChanges();

                //записываем файлы заказа
                Filer.OnCustomerCreatedAnOrder(orderModel.FileUpload, order);

                if (creationType == OrderCreationType.Ordinary)
                {
                    //вызываем класс отвечающий за создание уведомлений
                    Notificater.OnCustomerAddedOrder(order, OrderCreationType.Ordinary);
                }
                    
                    
                return order;
            }
        }

        /// <summary>
        /// Создает заказ асинхронно, первым параметром вы указываете тип создания
        /// заказа, по нему программа выберет какого вида уведомления нужно создать 
        /// и отправить пользователю
        /// </summary>
        /// <param name="creationType">Этот параметр используется для выбора уведомлений</param>
        /// <param name="orderModel"></param>
        /// <returns>
        /// Создает заказ асинхронно, первым параметром вы указываете тип создания
        /// заказа, по нему программа выберет какого вида уведомления нужно создать 
        /// и отправить пользователю
        /// </returns>
        public async Task<Order> CreateOrderSubMethodAsync(OrderCreationType creationType, OrderCreateModel orderModel)
        {
            Order order = orderModel.ToOrder(UserGuid);
            Context.Orders.Add(order);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                MyExceptionHandler.CatchDbEntityValidationException(e);
            }


            //записываем файлы заказа
            Filer.OnCustomerCreatedAnOrder(orderModel.FileUpload, order);

            if (creationType == OrderCreationType.Ordinary)
            {
                //если заказ является заявкой на онлайн помощь 
                if(order.WorkType == OrderWorkType.OnlineHelp)
                {
                    //вызываем другой метод уведомлений
                    Notificater.OnCustomerAddedOnlineOrder(order);
                }
                else
                {
                    //вызываем класс отвечающий за создание уведомлений
                    Notificater.OnCustomerAddedOrder(order, OrderCreationType.Ordinary);
                }
                
            }


            return order;

        }
        
        
        #endregion
        #endregion


        public Order DeleteOrder(int id)
        {
            Order order = Context.Orders.FirstOrDefault(o => o.UserGuid == UserGuid && o.Status == OrderStatus.New && o.Id == id);
            if (order == null)
            {
                return null;
            }
            Context.Orders.Remove(order);
            Notificater.OnCustomerDeletedOrder(order);
            Context.SaveChanges();
            return order;
        }


        #endregion
    }
}
