using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runtasker.Controllers.Mvc
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");

            //return View();
        }


        public ActionResult HttpError404()
        {
            return View();
        }


        public ActionResult HttpError500()
        {
            return View();
        }


        public ActionResult General()
        {
            return View();
        }
    }
}