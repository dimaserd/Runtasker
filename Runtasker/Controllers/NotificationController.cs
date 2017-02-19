using Microsoft.AspNet.Identity;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Notifications;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Runtasker.Controllers
{
    [AllowAnonymous]
    public class NotificationController : Controller
    {
        #region Private Fields

        private WebUINotificater _notificater;

        public string UserGuid
        {
            get
            {
                if(Request.IsAuthenticated)
                {
                    return User.Identity.GetUserId();
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region Properties
        public WebUINotificater Notificater
        {
            get { return _notificater ?? new WebUINotificater(UserGuid); }
            set { _notificater = value; }
        }
        #endregion
        // GET: Notification
        
        public ActionResult Index()
        {
            Notification model = Notificater.GetNotification();
            return View(model);
        }

        public ActionResult GetNotification()
        {
            Notification model = Notificater.GetNotification();
            return PartialView(viewName: "Index", model : model);
        }

        public async Task<ActionResult> GetNotificationAsync()
        {
            Notification model = await Notificater.GetNotificationAsync();
            return PartialView(viewName: "Index", model: model);
        }

        public ActionResult Info(InfoModel model)
        {
            return View(model);
        }

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if(_notificater != null)
            {
                _notificater.Dispose(disposing);
                _notificater = null;
            }
            
            base.Dispose(disposing);
        }
        #endregion
    }
}