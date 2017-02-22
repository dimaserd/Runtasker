using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System;
using Runtasker.Logic;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Orders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Extensions.Models;
using Runtaker.LocaleBuiders.Views.Order;

namespace Runtasker.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        #region Private Fields

        CustomerOrderWorker _orderWorker;

        MyDbContext _db = new MyDbContext();

        CustomerOrderErrorEvents _errorWorker;

        OrderViewModelBuilder _modelBuilder;
        #endregion

        #region Properties
        CustomerOrderWorker OrderWorker
        {
            get
            {
                if(_orderWorker == null)
                {
                    _orderWorker = new CustomerOrderWorker(_db, UserGuid);
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
                    _errorWorker = new CustomerOrderErrorEvents(UserGuid);
                }
                return _errorWorker;
            }
        }

        OrderViewModelBuilder ModelBuilder
        {
            get
            {
                if(_modelBuilder == null)
                {
                    _modelBuilder = new OrderViewModelBuilder();
                }
                return _modelBuilder;
            }
        }
        string UserGuid
        {
            get { return User.Identity.GetUserId(); }
        }
        #endregion


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
            return View(model : await OrderWorker.GetMyOrdersAsync());
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

        #region PayHalf Methods
        //Orders should be paid only if current user
        //has money on his balance 
        //if not redirect him to Payment Controller Methods
        [HttpGet]
        public ActionResult PayHalf(int id)
        {
            PayHalfModel model = OrderWorker.GetPayHalfModel(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //TODO
        [HttpPost]
        public ActionResult PayHalf(PayHalfModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            WorkerResult result = OrderWorker.PayHalfOfOrder(model);

            if (!result.Succeeded)
            {
                foreach (string error in result.ErrorsList)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Add Files Methods

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
        #region AddDescription Methods
        
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
                
                Order order = OrderWorker.AddDescriptionToOrder(model);
                
                if(order == null)
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }
        #endregion

        #region PayAnotherHalf Methods

        
        [HttpGet]
        public ActionResult PayAnotherHalf(int id)
        {
            PayAnotherHalfModel model = OrderWorker.GetPayAnotherHalfModel(id);
            if(model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //After user pays redirect him to dowmload solution method
        [HttpPost]
        public ActionResult PayAnotherHalf(PayAnotherHalfModel model)
        {
            if(!ModelState.IsValid)
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

        //TODO
        #region Rating Methods
        [HttpGet]
        public ActionResult Rating(int id)
        {
            RatingOrderModel model = OrderWorker.GetRatingOrderModel(id);

            if(model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Rating(RatingOrderModel model)
        {
            if(!ModelState.IsValid)
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

        #region Create Methods
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
            ViewData["localeModel"] = ModelBuilder.CreateOrderView();

            return View(model : new OrderCreateModel() { FinishDate = DateTime.Now});
        }

        // POST: Orders/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Subject,WorkType,Description,FinishDate,FileUpload,Attachments,OtherSubject")] OrderCreateModel createOrder)
        {
            
            //для каждого задания свои сроки выполнения
            switch (createOrder.WorkType)
            {
                case OrderWorkType.Ordinary:
                    if ((createOrder.FinishDate - DateTime.Now).TotalDays < 3)
                    {
                        ModelState.AddModelError("", Resources.Views.Orders.Create.Create.FinishDateErrorOrdinary);
                    }
                    break;

                case OrderWorkType.Essay:
                    if ((createOrder.FinishDate - DateTime.Now).TotalDays < 7)
                    {
                        ModelState.AddModelError("", Resources.Views.Orders.Create.Create.FinishDateErrorEssay);
                    }
                    break;

                case OrderWorkType.CourseWork:
                    if ((createOrder.FinishDate - DateTime.Now).TotalDays < 30)
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

            ViewData["localeModel"] = ModelBuilder.CreateOrderView();
            return View(model: createOrder);
        
        }
        #endregion

        

        

        public ActionResult DownloadSolution(int id)
        {
            return RedirectToAction("DownloadSolution", "File", routeValues: new { id = id});
        }

        protected override void Dispose(bool disposing)
        {
            IDisposable[] toDisposes = new IDisposable[]
            {
                _db, _orderWorker, _modelBuilder
            };

            if (disposing)
            {
                for(int i = 0; i < toDisposes.Length; i++)
                {
                    if(toDisposes[i] != null)
                    {
                        toDisposes[i].Dispose();
                        toDisposes[i] = null;
                    }
                }
                

                
            }
            base.Dispose(disposing);
        }
    }
}
