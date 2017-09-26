using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Orders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Extensions.Models;
using Runtasker.LocaleBuilders.Views.Order;
using Runtasker.Logic.Models.Orders;
using Runtasker.Logic.Models.Orders.Pay;
using HtmlExtensions.StaticRenderers;
using Microsoft.AspNet.Identity.Owin;
using Runtasker.Logic.Enumerations.Notifications.Anonymous;
using Extensions.Decimal;
using Runtasker.Controllers.Base;
using System.Data.Entity;

namespace Runtasker.Controllers
{
    [Authorize]
    public class OrdersController : BaseMvcController
    {
        #region Поля
        CustomerOrderWorker _orderWorker;

        CustomerOrderErrorEvents _errorWorker;
        #endregion

        #region Свойства

        CustomerOrderWorker OrderWorker
        {
            get
            {
                if(_orderWorker == null)
                {
                    _orderWorker = new CustomerOrderWorker(Db, UserManager, UserGuid);
                }
                return _orderWorker; 
            }
        }

        CustomerOrderErrorEvents ErrorWorker
        {
            get
            {
                if(_errorWorker == null)
                {
                    _errorWorker = new CustomerOrderErrorEvents(UserGuid, Db);
                }
                return _errorWorker;
            }
        }

        
        #endregion

        #region Http Обработчики

        // GET: Orders
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            if(!Request.IsAuthenticated)
            {
                return RedirectToAction("KnowPrice", "Home");
            }

            if(User.IsInRole("Performer") || User.IsInRole("Admin") || User.IsInRole("VkPerformer"))
            {
                return RedirectToAction("Index", "Performer");
            }
            return View(model : await OrderWorker.NewGetMyOrdersAsync());
        }

        [AllowAnonymous]
        public ActionResult MiniPanel()
        {
            IEnumerable<Order> model = OrderWorker.GetMyOrders().ToList();
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> MiniPanelAsync()
        {
            IEnumerable<Order> model = await OrderWorker.GetMyOrdersAsync();
            return View(viewName: "MiniPanel", model:model);
        }

        #region Исправление ошибок

        #region Добавление файлов

        [HttpGet]
        public ActionResult AddFiles(int id)
        {
            OrderAddFilesModel model = OrderWorker.GetAddFilesModel(id);
            if(model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult AddFiles(OrderAddFilesModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            WorkerResult result = OrderWorker.AddFilesToOrder(model);

            if(!result.Succeeded)
            {
                foreach(string error in result.ErrorsList)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }

            return RedirectToAction("Index");
        }

        #endregion

        //тестирование методов добавления описания новым 
        //более совершенным классом
        #region Добавление описания
        
        [HttpGet]
        public ActionResult AddDescription(int id)
        {
            //Закоментируй если будешь работать по новому
            AddDescriptionModel model = OrderWorker.GetAddDescriptionModel(id);

            if(model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddDescription(AddDescriptionModel model)
        {
            if(ModelState.IsValid)
            {
                
                Order order = await OrderWorker.AddDescriptionToOrderAsync(model);
                
                if(order == null)
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }
        #endregion

        #endregion

        #region Оплата

        #region Оплата первой половины
        
        [HttpGet]
        public ActionResult PayHalf(int id)
        {
            PayHalfModel model = OrderWorker.GetPayHalfModel(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["localeModel"] = OrderViewModelBuilder.PayHalfView(model.OrderId, FASigns.CreditCard.ToString());
            return View(model);
        }

        [HttpPost]
        public ActionResult PayHalf(PayHalfModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["localeModel"] = OrderViewModelBuilder.PayHalfView(model.OrderId, FASigns.CreditCard.ToString());
                return View(model);
            }

            WorkerResult result = OrderWorker.PayHalfOfOrder(model);

            if (!result.Succeeded)
            {
                foreach (string error in result.ErrorsList)
                {
                    ModelState.AddModelError("", error);
                }
                ViewData["localeModel"] = OrderViewModelBuilder.PayHalfView(model.OrderId, FASigns.CreditCard.ToString());
                return View(model);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Оплата второй половины


        [HttpGet]
        public ActionResult PayAnotherHalf(int id)
        {
            PayAnotherHalfModel model = OrderWorker.GetPayAnotherHalfModel(id);

            ViewData["localeModel"] = OrderViewModelBuilder.PayAnotherHalfView(model.OrderId, model.RequiredSum.ToMoney(), HtmlSigns.Rouble);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //After user pays redirect him to dowmload solution method
        [HttpPost]
        public ActionResult PayAnotherHalf(PayAnotherHalfModel model)
        {

            ViewData["localeModel"] = OrderViewModelBuilder.PayAnotherHalfView(model.OrderId, model.RequiredSum.ToMoney(), HtmlSigns.Rouble);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            WorkerResult result = OrderWorker.PayAnotherHalfOfAnOrder(model);
            if(!result.Succeeded)
            {
                foreach(string error in result.ErrorsList)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Оплата Онлайн-Помощи
        [HttpGet]
        public async Task<ActionResult> PayOnlineHelp(int id)
        {
            PayOnlineHelp model = await OrderWorker.GetPayOnlineHelpModelAsync(id);

            ViewData["localeModel"] = OrderViewModelBuilder.PayOnlineHelp(model.RequiredSum.ToMoney(), HtmlSigns.Rouble);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> PayOnlineHelp(PayOnlineHelp model)
        {

            ViewData["localeModel"] = OrderViewModelBuilder.PayOnlineHelp(model.RequiredSum.ToMoney(), HtmlSigns.Rouble);

            if (ModelState.IsValid)
            {
                WorkerResult result = await OrderWorker.PayOnlineHelpAsync(model);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }
        #endregion

        #endregion
        //TODO
        #region Оценка заказа
        [HttpGet]
        public ActionResult Rating(int id)
        {
            RatingOrderModel model = OrderWorker.GetRatingOrderModel(id);

            if(model == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["localeModel"] = OrderViewModelBuilder.RatingView(model.OrderId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Rating(RatingOrderModel model)
        {
            ViewData["localeModel"] = OrderViewModelBuilder.RatingView(model.OrderId);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            WorkerResult result = OrderWorker.RatingOrder(model);
            if(!result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Создание заказа

        #region Обычный заказ
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("KnowPrice", "Home");
            }

            if (User.IsInRole("Performer") || User.IsInRole("Admin") || User.IsInRole("VkPerformer"))
            {
                return RedirectToAction("Index", "Performer");
            }

            if (!User.Identity.IsEmailConfirmed())
            {
                ErrorWorker.OnCustomerTriedToAddAnOrderWithUnconfirmedAccount();
                return RedirectToAction("Index", "Home");
            }
            ViewData["localeModel"] = OrderViewModelBuilder.CreateOrderView();

            return View(model : new OrderCreateModel() { FinishDate = DateTime.Now});
        }

        // POST: Orders/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrderCreateModel createOrder)
        {    
            //для каждого задания свои сроки выполнения
            switch (createOrder.WorkType)
            {
                case OrderWorkType.Ordinary:
                    if ((createOrder.FinishDate - DateTime.Now).TotalDays < 3 -2)
                    {
                        ModelState.AddModelError("", Resources.Views.Orders.Create.Create.FinishDateErrorOrdinary);
                    }
                    break;

                case OrderWorkType.Essay:
                    if ((createOrder.FinishDate - DateTime.Now).TotalDays < 7 - 2)
                    {
                        ModelState.AddModelError("", Resources.Views.Orders.Create.Create.FinishDateErrorEssay);
                    }
                    break;

                case OrderWorkType.CourseWork:
                    if ((createOrder.FinishDate - DateTime.Now).TotalDays < 30 - 2)
                    {
                        ModelState.AddModelError("", Resources.Views.Orders.Create.Create.FinishDateErrorCourseWork);
                    }
                    break;

                default:
                    break;
            }

            if (ModelState.IsValid)
            {
                Order order = await OrderWorker.CreateOrderAsync(createOrder);
                return RedirectToAction("Index");
                
            }

            ViewData["localeModel"] = OrderViewModelBuilder.CreateOrderView();
            return View(model: createOrder);
        
        }
        #endregion

        #region Онлайн-помощь

        [HttpGet]
        [AllowAnonymous]
        public ActionResult OnlineHelp()
        {
            

            if(!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Landing", new { notType = AnonymousNotificationType.TriedToOrderOnlineHelp });
            }

            if(User.IsInRole("Admin") || User.IsInRole("Performer"))
            {
                return RedirectToAction("Index", "Performer");
            }

            if (!User.Identity.IsEmailConfirmed())
            {
                ErrorWorker.OnCustomerTriedToAddAnOrderWithUnconfirmedAccount();
                return RedirectToAction("Index", "Home");
            }

            ViewData["localeModel"] = OrderViewModelBuilder.OnlineHelpView();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> OnlineHelp(OnlineOrderRequest model)
        {
            //изменяем телефон
            if(model.PhoneNumber != User.Identity.GetPhoneNumber())
            {
                //получаем пользователя
                ApplicationUser user = await UserManager.FindByIdAsync(UserGuid);
                
                if (user != null)
                {
                    //меняем номер телефона пользователя
                    user.PhoneNumber = model.PhoneNumber;

                    //соханяем изменения
                    await UserManager.UpdateAsync(user);
                    
                    //обновляем куки
                    SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                }
            }

            if (ModelState.IsValid)
            {
                Order order = await OrderWorker.CreateOnlineHelpOrderAsync(model.ToOrderCreateModel());

                return RedirectToAction("Index");

            }

            return View(model);
        }

        #endregion

        #endregion

        #region Удаление заказа
        [HttpGet]
        public ActionResult DeleteOrder(int id)
        {
            Order order = Db.Orders.FirstOrDefault(x => x.Id == id && x.UserGuid == UserGuid);

            if (order.Status != OrderStatus.DeletedByCustomer && order.Status != OrderStatus.DeletedByAdmin)
            {
                return View(order);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOrder(int id, string customerId)
        {
            Order order = Db.Orders.FirstOrDefault(x => x.Id == id && x.UserGuid == UserGuid);

            if (order.Status != OrderStatus.DeletedByCustomer && order.Status != OrderStatus.DeletedByAdmin)
            {
                order.LastStatus = order.Status;
                order.Status = OrderStatus.DeletedByCustomer;
                Db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        #endregion


        #region Методы корзины
        [HttpGet]
        public async Task<ActionResult> Trash()
        {
            List<Order> ordersInTrash = await
                Db.Orders
                .Where(x => x.Status == OrderStatus.DeletedByCustomer || x.Status == OrderStatus.DeletedByAdmin)
                .ToListAsync();

            return View(ordersInTrash);
        }

        [HttpGet]
        public ActionResult Restore(int id)
        {
            Order order = Db.Orders.FirstOrDefault(x => x.Id == id && 
            x.UserGuid == UserGuid &&
            (x.Status == OrderStatus.DeletedByCustomer || x.Status == OrderStatus.DeletedByAdmin));

            return View(order);
        }

        [HttpPost]
        public ActionResult Restore(int id, string customerId)
        {
            Order order = Db.Orders.FirstOrDefault(x => x.Id == id &&
            x.UserGuid == UserGuid &&
            (x.Status == OrderStatus.DeletedByCustomer || x.Status == OrderStatus.DeletedByAdmin));

            if(order != null)
            {
                order.Status = order.LastStatus;
                Db.SaveChanges();

                return RedirectToAction("Trash");
            }


            return RedirectToAction("Trash");
        }
        #endregion

        public ActionResult DownloadSolution(int id)
        {
            return RedirectToAction("DownloadSolution", "File", routeValues: new { id = id});
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            IDisposable[] toDisposes = new IDisposable[]
            {
                _orderWorker,
            };

            if (disposing)
            {
                DisposeObjects(toDisposes);
            }

            base.Dispose(disposing);
        }
    }
}
