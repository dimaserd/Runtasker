using Microsoft.AspNet.Identity;
using Runtasker.Controllers.Base;
using Runtasker.Logic;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.MessageWorker;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Runtasker.Controllers
{
    [Authorize]
    public class MessageController : BaseMvcController
    {
        #region Конструкторы

        public MessageController()
        {
            
        }
        #endregion

        #region Поля
        private MessageWorker _messageWorker;

        private MessageOrderWorker _messageOrderWorker;        
        #endregion

        #region Свойства

        

        public MessageWorker Messager
        {
            get
            {
                if (_messageWorker == null)
                {
                    _messageWorker = new MessageWorker(Db,UserGuid);
                }
                return _messageWorker;
            }
        }
        

        public MessageOrderWorker OrderMessager
        {
            get
            {   
                if(_messageOrderWorker == null)
                {
                    _messageOrderWorker = new MessageOrderWorker(UserGuid, Db);
                }
                return _messageOrderWorker;
            }
        }

        #endregion

        #region Some Functions
        private string GetName(string userGuid)
        {
            return Db.Users.FirstOrDefault(u => u.Id == userGuid).Name;
        }
        #endregion

        #region Http обработчика
        [AllowAnonymous]
        public ActionResult Modal()
        {
            return View();
        }

        public ActionResult Index()
        {
            MessageIndexViewModel model = Messager.GetMessageIndexModel();
            return View(model);
        }

        public ActionResult ActivePanel()
        {
            PanelViewModel model = Messager.GetPanelModel();
            return View(viewName : "NewActivePanel", model: model);
        }

        public async Task<ActionResult> ActivePanelAsync()
        {
            PanelViewModel model = await Messager.GetPanelModelAsync();
            return View(viewName: "NewActivePanel", model: model);
        }

        public async Task<ActionResult> NewActivePanelAsync()
        {
            PanelViewModel model = await Messager.GetPanelModelAsync();
            return View(viewName: "ActivePanel", model: model);
        }


        #region Методы чата
        //TO DO Chat                                                 
        public ActionResult Chat(string toGuid)
        {
            List<Message> model = Messager.GetChat(toGuid).ToList();
            ViewData["userGuid"] = UserGuid;
            ViewData["senderName"] = User.Identity.GetName();


            ViewData["receiverName"] = GetName(toGuid);
            ViewData["toGuid"] = toGuid;

            return View(model);
        }

        public async Task<ActionResult> ChatAboutOrder(int orderId)
        {
            string receiverName = string.Empty;
            string toId = string.Empty;

            IEnumerable<Message> model;
            if (User.IsInRole("Customer"))
            {
                model = await OrderMessager.GetChatAboutOrderAsCustomerAsync(orderId);
                receiverName = OrderMessager.GetAdminName();
                toId = OrderMessager.GetAdminId();
            }
            else
            {
                model = await OrderMessager.GetChatAboutOrderAsPerformerAsync(orderId);
                var customer = await OrderMessager.GetCustomerAsync(orderId);
                receiverName = customer.Name;
                toId = customer.Id;
            }

            ViewData["userGuid"] = UserGuid;
            ViewData["senderName"] = User.Identity.GetName();

            ViewData["orderId"] = orderId;
            ViewData["receiverName"] = receiverName;
            ViewData["toGuid"] = toId;

            return View(viewName: "~/Views/Shared/NewChatAboutOrder.cshtml", model: model);
        }

        #endregion

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