using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Runtasker.LocaleBuilders.Enumerations;
using Runtasker.LocaleBuilders.Statics;
using Runtasker.Logic;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Runtasker.Controllers.Base
{
    public class BaseMvcController : Controller
    {
        #region Поля
        MyDbContext _db = new MyDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        #endregion

        #region Свойства
        protected Lang CurrentLang
        {
            get
            {
                return LanguageStatic.Language;
            }
        }

        protected MyDbContext Db
        {
            get
            {
                return _db;
            }
        }

        protected ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected string UserGuid
        {
            get { return User.Identity.GetUserId(); }
        }
        #endregion

        

        protected void DisposeObjects(IList<IDisposable> toDisposes)
        {
            for (int i = 0; i < toDisposes.Count; i++)
            {
                if (toDisposes[i] != null)
                {
                    toDisposes[i].Dispose();
                    toDisposes[i] = null;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            List<IDisposable> toDisposes = new List<IDisposable>
            {
                _db, _signInManager, _userManager
            };

            if (disposing)
            {
                DisposeObjects(toDisposes);
            }
            base.Dispose(disposing);
        }
    }
}