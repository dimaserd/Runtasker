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
using Runtasker.Settings;
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
        #region Конструктор
        public CustomerOrderWorker(MyDbContext context, string userGuid) : base(context,userGuid)
        {
            Construct();
        }

        void Construct()
        {            
            Filer = new CustomerFileMethods(Context);
            Notificater = new CustomerOrderNotificationMethods(Context);
            Attachmenter = new CustomerAttachmentWorker();
            
            //здесь все упирается в UserManger
            Paymenter = new CustomerOrderPaymentMethods(UserGuid, Context);
            ErrorHandler = new CustomerOrderErrorEvents(UserGuid, Context);
            
        }
        #endregion

        #region Поля
        ApplicationUser _customer;
        #endregion

        #region Свойства
       
        CustomerOrderNotificationMethods Notificater { get; set; }

        CustomerFileMethods Filer { get; set; }

        CustomerAttachmentWorker Attachmenter { get; set; }

        CustomerOrderPaymentMethods Paymenter { get; set; }

        CustomerOrderErrorEvents ErrorHandler { get; set; }
        

        #region Внутренние свойства
        ApplicationUser Customer
        {
            get
            {
                if(_customer == null)
                {
                    _customer = Context.Users.FirstOrDefault(u => u.Id == UserGuid);
                }

                return _customer;
            }
        }
        #endregion

        #endregion

        #region Публичные методы

        #region Создание заказа
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
        /// </summary>
        /// <returns></returns>
        public async Task<Order> CreateOnlineHelpOrderAsync(OrderCreateModel model)
        {
            return await CreateOrderSubMethodAsync(OrderCreationType.Ordinary, model);
        }
        #endregion

        #region Исправление ошибок

        #region Добавление описания
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

        public async Task<Order> AddDescriptionToOrderAsync(AddDescriptionModel model)
        {
            Order order = await Context.Orders.FirstOrDefaultAsync
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

            await Context.SaveChangesAsync();
            return order;

        }
        #endregion

        #region Добавление файлов

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
        /// <summary>
        /// Добавляет файлы в заказ через объект Attachment связанный с заказом
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WorkerResult AddFilesToOrder(OrderAddFilesModel model)
        {
            Order order = Context.Orders.Include(x => x.Attachments).FirstOrDefault
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
        #endregion

        #region Методы оплаты

        #region Оплата первой половины
        public PayHalfModel GetPayHalfModel(int id)
        {
            //получаем заказ из базы данных
            //заказ не должен быть онлайн помощью
            Order order = Context.Orders.FirstOrDefault(
                    
                    o => o.UserGuid == UserGuid
                    && o.Id == id
                    && o.Status == OrderStatus.Estimated
                    && o.WorkType != OrderWorkType.OnlineHelp);

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
            //получаем заказ из базы данных
            //заказ не должен быть онлайн помощью
            Order order = Context.Orders.FirstOrDefault(

                    o => o.UserGuid == UserGuid
                    && o.Id == model.OrderId
                    && o.Status == OrderStatus.Estimated
                    && o.WorkType != OrderWorkType.OnlineHelp);

            if (order == null)
            {
                return new WorkerResult("No order matched!");
            }

            //пользователь должен оплатить только половину
            decimal sumThatUserNeedToPay = order.Sum / 2;

            if (model.Sum != sumThatUserNeedToPay)
            {
                return new WorkerResult("You should pay a half of an order!");
            }

            //если баланс пользователя меньше чем сумма котоурю ему нужно оплатить
            if (Customer.Balance < sumThatUserNeedToPay)
            {
                //не добавляем текст ошибки так как будет показано уведомление
                WorkerResult result = new WorkerResult
                {
                    Succeeded = false
                };

                //вызываем метод который обрабатывает ошибку
                ErrorHandler.OnCustomerTriedToPayWithoutMoney(Customer, order);

                return result;
            }

            //меняем свойства заказа
            order.PaidSum = model.Sum;
            order.Status = OrderStatus.HalfPaid;
            Context.SaveChanges();

            //Вызываю метод записывающий оплату в базу данных
            Paymenter.OnCustomerPaidFirstHalfOfAnOrder(order);

            //вызываем метод уведомляющий пользователя о прошедшей оплате заказа
            Notificater.OnCustomerPaidHalfOfAnOrder(order);

            return new WorkerResult
            {
                Succeeded = true
            };
        }
        #endregion

        #region Оплата второй половины

        public PayAnotherHalfModel GetPayAnotherHalfModel(int orderId)
        {
            Order order = Context.Orders.FirstOrDefault(o => o.Id == orderId
            && o.Status == OrderStatus.Finished
            && o.UserGuid == UserGuid
            && o.WorkType != OrderWorkType.OnlineHelp);

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
            //получаем заказ из базы данных
            //заказ не должен быть онлайн помощью
            Order order = Context.Orders.FirstOrDefault(o => o.Id == model.OrderId
            && o.Status == OrderStatus.Finished
            && o.UserGuid == UserGuid
            && o.WorkType != OrderWorkType.OnlineHelp);


            decimal sumThatUserNeedToPay = order.Sum / 2;

            if (order == null)
            {
                return new WorkerResult($"Finished Order №{model.OrderId} not found!");
            }

            if (model.Sum != order.Sum / 2)
            {
                return new WorkerResult($"You should pay {sumThatUserNeedToPay} roubles!");
            }

            //если баланс пользователя ниже чем сумма которую ему нужно оплатить
            if (Customer.Balance < sumThatUserNeedToPay)
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

        #region Онлайн-Помощь

        public async Task<PayOnlineHelp> GetPayOnlineHelpModelAsync(int orderId)
        {
            Order order = await Context.Orders
                .FirstOrDefaultAsync(x => x.Id == orderId &&
                x.PaidSum == 0 && 
                x.UserGuid == UserGuid &&
                x.WorkType == OrderWorkType.OnlineHelp &&
                x.Status == OrderStatus.Estimated
                );

            if(order != null)
            {
                return new PayOnlineHelp
                {
                    OrderId = order.Id,
                    Sum = order.Sum,
                    RequiredSum = order.Sum
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
                x.Status == OrderStatus.Estimated
                );

            if(order == null)
            {
                return new WorkerResult("Произошла ошибка при оплате онлайн-помощи!");
            }

            //пользователь должен оплатить заказ полностью
            decimal sumThatUserNeedToPay = order.Sum;

            //если переданная сумма не совпадает с суммой которую нужно оплатить
            if (model.Sum != sumThatUserNeedToPay)
            {
                //возвращаем ошибку
                return new WorkerResult("You should pay a half of an order!");
            }

            //если баланс пользователя ниже чем сумма которую ему нужно оплатить
            if (Customer.Balance < sumThatUserNeedToPay)
            {
                //Не добавляем описание ошибки так как будет показано уведомление
                WorkerResult result = new WorkerResult
                {
                    Succeeded = false
                };

                //вызываем метод который обрабатывает ошибку
                ErrorHandler.OnCustomerTriedToPayWithoutMoney(Customer, order);

                return result;
            }

            //регистрируем оплату
            Paymenter.OnCutomerPaidOnlineHelp(order);


            //изменяем данные о заказе
            order.Status = OrderStatus.FullPaid;
            order.PaidSum = model.Sum;

            //уведомления не сделаны
            Notificater.OnCustomerPaidOnlineHelp(order);
            
            //сохраняем изменения
            await Context.SaveChangesAsync();

            return new WorkerResult
            {
                Succeeded = true
            };

        }


        #endregion

        #endregion

        #region Методы оценки заказа
        public RatingOrderModel GetRatingOrderModel(int orderId)
        {
            Order order = Context.Orders.FirstOrDefault(o => o.Id == orderId
            && (o.Status == OrderStatus.Downloaded || (o.Status == OrderStatus.FullPaid && o.WorkType == OrderWorkType.OnlineHelp))
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
            && (o.Status == OrderStatus.Downloaded || (o.Status == OrderStatus.FullPaid && o.WorkType == OrderWorkType.OnlineHelp))
            && o.UserGuid == UserGuid);

            if (order == null)
            {
                return new WorkerResult("Order not found");
            }



            //изменяем свойства заказа
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

        #region Получение заказов
        public IEnumerable<Order> GetMyOrders()
        {
            return Context.Orders.Where(o => o.UserGuid == UserGuid).ToList();
        }

        public async Task<IEnumerable<Order>> GetMyOrdersAsync()
        {
                return await Context.Orders.Where(o => o.UserGuid == UserGuid)
                    .Include(x => x.Messages).ToListAsync();
        }
        #endregion

        #region Удаление заказа

        /// <summary>
        /// Пока нигде не используется и не реализован должным образом
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        #endregion

        #region Вспомогательные методы

        public string GetAdminGuid()
        {
            return Context.Users.FirstOrDefault(u => u.Email == AdminSettings.AdminEmail).Id;
        }

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

        #region Создание заказа
        public Order CreateOrderSubMethod(OrderCreationType creationType, OrderCreateModel orderModel)
        {

            Order order = orderModel.ToOrder(UserGuid);

            Context.Orders.Add(order);
            Context.SaveChanges();

            //записываем файлы заказа
            Filer.OnCustomerCreatedAnOrder(orderModel.FileUpload, order);

            if (creationType == OrderCreationType.Ordinary)
            {
                //вызываем класс отвечающий за создание уведомлений
                Notificater.OnCustomerAddedOrder(order, OrderCreationType.Ordinary);
            }


            return order;
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
                if (order.WorkType == OrderWorkType.OnlineHelp)
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
    }
}
