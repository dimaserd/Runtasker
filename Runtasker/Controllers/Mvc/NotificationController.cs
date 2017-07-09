﻿using Runtasker.Controllers.Base;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Enumerations.InfoModels;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Info;
using Runtasker.Logic.Workers.Notifications;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Runtasker.Controllers
{
    [AllowAnonymous]
    public class NotificationController : BaseMvcController
    {
        #region Поля
        
        WebUINotificater _notificater;

        
        #endregion

        #region Свойства
        public WebUINotificater Notificater
        {
            get
            {
                if(_notificater == null)
                {
                    _notificater = new WebUINotificater(UserGuid, Db);
                }
                return _notificater;
            }
            
        }
        #endregion

        #region Http обработчики
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

        public ActionResult Info(InfoModelType? infoType = null)
        {
            if(infoType != null)
            {
                InfoModel infoModel = AccountInfoModels.GetInfoModel(infoType);
                return View(infoModel);
            }
            
            return RedirectToAction("Index", "Home");
        }
        
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                IDisposable[] toDisposes = new IDisposable[]
                {
                    _notificater
                };
                DisposeObjects(toDisposes);
            }
            
            base.Dispose(disposing);
        }
        #endregion
    }
}