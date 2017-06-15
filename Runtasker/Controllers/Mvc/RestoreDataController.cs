using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runtasker.Controllers
{
    public class RestoreDataController : Controller
    {
        // GET: RestoreData
        public ActionResult Index()
        {
            if(UserHasRightsToBeHere)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
            
        }

        #region GetData methods
        [HttpGet]
        public ActionResult GetData()
        {
            if (UserHasRightsToBeHere)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Help Methods
        public bool UserHasRightsToBeHere
        {
            get
            {
                return Request.IsAuthenticated &&
                (User.IsInRole("Admin") || User.IsInRole("Dev"));
            }
            
        }
        #endregion
    }
}