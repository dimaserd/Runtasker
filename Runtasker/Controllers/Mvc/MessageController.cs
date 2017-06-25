using Microsoft.AspNet.Identity;
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
    public class MessageController : Controller
    {
        #region Contructors

        public MessageController()
        {
            _context = new MyDbContext();
        }
        #endregion

        #region Поля
        private MessageWorker _messageWorker;

        private MessageOrderWorker _messageOrderWorker;

        
        private MyDbContext _context;

        private UserManager<ApplicationUser> _userManager;
       
        #endregion

        #region Свойства

        private string UserGuid
        {
            get
            {
                if (Request.IsAuthenticated)
                {
                    return User.Identity.GetUserId();
                }
                else
                {
                    return "";
                }
            }
        }

        public MessageWorker Messager
        {
            get
            {
                if (_messageWorker == null)
                {
                    _messageWorker = new MessageWorker(Context,UserGuid);
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
                    _messageOrderWorker = new MessageOrderWorker(UserGuid, Context);
                }
                return _messageOrderWorker;
            }
        }

        public MyDbContext Context { get { return _context; } }
        #endregion

        #region Some Functions
        private string GetName(string userGuid)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userGuid).Name;
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

        public ActionResult ChatAboutOrder(int orderId)
        {
            ViewData["userGuid"] = UserGuid;
            ViewData["senderName"] = User.Identity.GetName();


            ViewData["receiverName"] = OrderMessager.GetChatterName();
            ViewData["toGuid"] = OrderMessager.GetChattterGuid();
            ViewData["orderId"] = orderId.ToString();

            IEnumerable<Message> model = OrderMessager.GetChatAboutOrder(orderId);

            return View(viewName: "~/Views/Shared/NewChatAboutOrder.cshtml", model: model);
        }

        #endregion

        #endregion


        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }

                if(_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }

            base.Dispose(disposing);
        }
        #endregion
    }
}