using Microsoft.AspNet.Identity;
using Runtasker.Logic;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.MessageWorker;
using Runtasker.Logic.Workers.Orders;
using Runtasker.Logic.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Runtasker.Logic.Workers.Files;
using Runtasker.Logic.Workers.Admin;
using System.Threading.Tasks;
using Logic.Extensions.Models;

namespace Runtasker.Controllers
{
    [Authorize(Roles = "VkPerformer,Performer,Admin")]
    public class PerformerController : Controller
    {
        #region Private Fields

        PerformerMessageOrderWorker _orderMesWorker;

        AdminCustomerMethods _adminWorker;

        PerformerOrderWorker _performerOrderWorker;

        SuperFileWorker _superFileWorker;

        MyDbContext Context = new MyDbContext();

        #endregion

        #region Private Properties

        PerformerMessageOrderWorker OrderMesWorker
        {
            get
            {
                if (_orderMesWorker == null)
                {
                    _orderMesWorker = new PerformerMessageOrderWorker(Context);
                }
                return _orderMesWorker;
            }
            set { _orderMesWorker = value; }
        }

        PerformerOrderWorker OrdersWorker
        {
            get
            {
                if(_performerOrderWorker == null)
                {
                    _performerOrderWorker = new PerformerOrderWorker(Context, UserGuid);
                }
                return _performerOrderWorker;
            }
        }

        AdminCustomerMethods AdminWorker
        {
            get
            {
                if(_adminWorker == null)
                {
                    _adminWorker = new AdminCustomerMethods(Context);
                }

                return _adminWorker;
            }
        }

        SuperFileWorker SuperFileWorker
        {
            get
            {
                if(_superFileWorker == null)
                {
                    _superFileWorker = new SuperFileWorker(Context);
                }
                return _superFileWorker;
            }
        }
        

        string UserGuid { get { return User.Identity.GetUserId(); } }
        #endregion

        // GET: Performer
        public async Task<ActionResult> Index()
        {
            if(!User.IsInRole("Performer") && User.IsInRole("VkPerformer"))
            {
                return RedirectToAction("Index", "VkOrders");
            }
            List<Order> orders = await OrdersWorker.GetOrdersAsync();

            return View(orders);
        }

        

        #region Admin Methods
        public ActionResult Users()
        {
            IEnumerable<ApplicationUser> model = AdminWorker.GetCustomers();

            return View(model);
        }

        public async Task<ActionResult> Statistics(string id)
        {
            CustomerInfo model = await AdminWorker.GetCustomerInfoAsync(id);
            return View(model);
        }

        #region Delete Methods

        #region Delete Order
        [HttpGet]
        public ActionResult DeleteAllOrders(string id)
        {
            IEnumerable<Order> model = AdminWorker.Delete.GetDeleteAllUserOrdersModel(id);
            return View(viewName: "DeleteOrders", model: model);
        }

        [HttpGet]
        public ActionResult DeleteFinishedOrders(string id)
        {
            IEnumerable<Order> model = AdminWorker.Delete.GetDeleteFinishedUserOrdersModel(id);
            return View(viewName: "DeleteOrders", model: model);
        }

        [HttpPost]
        public ActionResult DeleteOrders(IEnumerable<Order> model)
        {
            AdminWorker.Delete.DeleteOrders(model);

            return RedirectToAction("Index");
        }

        #endregion

        #region Delete User
        [HttpGet]
        public ActionResult DeleteUser(string id)
        {
            ApplicationUser model = AdminWorker.Delete.GetDeleteUserModel(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteUser(ApplicationUser user)
        {
            if(ModelState.IsValid)
            {
                AdminWorker.Delete.DeleteUserConfirmed(user);
            }
            return RedirectToAction("Users");
        }
        #endregion

        //TODO
        #region Delete Message Methods
        public ActionResult DeleteMessage(int id)
        {

            return View();
        }
        #endregion
        #endregion


        #region WriteEmail methods
        [HttpGet]
        public ActionResult WriteEmail(string id)
        {
            AdminWorker.SetCustomerField(id);

            ViewData["user"] = AdminWorker.GetCustomer();
            ActionEmailToCustomer model = AdminWorker.GetWriteEmailModel();

            return View(model);
        }

        //TODO
        [HttpPost]
        public ActionResult WriteEmail(ActionEmailToCustomer model)
        {
            AdminWorker.WriteEmailToCustomer(model);
            return RedirectToAction("Users");
        }
        #endregion

       
        #endregion

        #region Orders methods

        #region Value Order Methods
        [HttpGet]
        public ActionResult ValueOrder(int id)
        {
            ValueOrderModel model = OrdersWorker.GetValueOrderModel(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(500);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ValueOrder(ValueOrderModel model)
        {
            Order order = OrdersWorker.ValueOrder(model);
            if(order == null)
            {
                return new HttpStatusCodeResult(500);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Add Error to Order Methods
        [HttpGet]
        public ActionResult AddError(int id)
        {
            AddErrorToOrderModel model = OrdersWorker.GetAddErrorToOrderModel(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(500);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult AddError(AddErrorToOrderModel model)
        {
            Order order = OrdersWorker.AddErrorToOrder(model);
            if(order == null)
            {
                return new HttpStatusCodeResult(500);
            }
            return RedirectToAction("Index");
        }

        #endregion

        //TODO
        #region Claim For Date Methods
        #endregion

        #region Solve Order Methods
        [HttpGet]
        public ActionResult Solve(int id)
        {
            SolveOrderModel model = OrdersWorker.GetSolveOrderModel(id);

            if(model == null)
            {
                return new HttpStatusCodeResult(500);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Solve(SolveOrderModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            WorkerResult result = OrdersWorker.SolveOrder(model);

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

        #region Choose Order Methods
        [HttpGet]
        public ActionResult Choose(int id)
        {
            ChooseOrderModel model = OrdersWorker.GetChooseOrderModel(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(500);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Choose(ChooseOrderModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            WorkerResult result = OrdersWorker.ChooseOrder(model);
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

       
        //уточнить у заказчика
        public ActionResult ChatAboutOrder(int id)
        {
            //First we have to set an order to reduce calls to database
            OrderMesWorker.SetOrder(id);

            ViewData["userGuid"] = UserGuid;
            ViewData["senderName"] = User.Identity.GetName();


            ViewData["receiverName"] = OrderMesWorker.GetChatterName();
            ViewData["toGuid"] = OrderMesWorker.GetUserGuidToChat();
            ViewData["orderId"] = id.ToString();

            IEnumerable<Message> model = OrderMesWorker.GetMessagesAboutOrder();
            return View(model: model);
        }

        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}