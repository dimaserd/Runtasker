using System.Web.Mvc;

namespace Runtasker.Controllers
{
    public class LandingController : Controller
    {
        // GET: Landing
        public ActionResult Index()
        {
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        #region Sections
        public ActionResult Slider()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Portfolio()
        {
            return View();
        }

        public ActionResult Counters()
        {
            return View();
        }

        public ActionResult Parallax()
        {
            return View();
        }

        public ActionResult Pricing()
        {
            return View();
        }
        #endregion
    }
}