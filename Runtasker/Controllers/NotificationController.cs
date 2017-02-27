using Microsoft.AspNet.Identity;
using Runtasker.Logic;
using Runtasker.Logic.Contexts;
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Info;
using Runtasker.Logic.Workers.Notifications;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Runtasker.Controllers
{
    [AllowAnonymous]
    public class NotificationController : Controller
    {
        #region Private Fields
        IMyDbContext _db = new MyDbContext();

        AccountInfoModels _infoModels;

        WebUINotificater _notificater;

        string UserGuid
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
            get
            {
                if(_notificater == null)
                {
                    _notificater = new WebUINotificater(UserGuid, _db);
                }
                return _notificater;
            }
            
        }

        AccountInfoModels InfoModels
        {
            get
            {
                if(_infoModels == null)
                {
                    _infoModels = new AccountInfoModels();
                }
                return _infoModels;
            }
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

        public ActionResult Info(string type)
        {
            if(type == "toConfirmEmail")
            {
                InfoModel infoModel = InfoModels.ToConfirmEmail;
                return View(infoModel);
            }

            return RedirectToAction("Index", "Home");
        }

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                IDisposable[] toDisposes = new IDisposable[]
                {
                    _notificater, _db
                };
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
        #endregion
    }
}