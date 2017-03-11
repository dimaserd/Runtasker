using Runtasker.LocaleBuilders.Views.Landing;
using System.Web.Mvc;

namespace Runtasker.Controllers
{
    public class LandingController : Controller
    {
        #region Properties
        LandingLocaleViewModelBuilder ModelBuilder
        {
            get
            {
                if(_modelBuilder == null)
                {
                    _modelBuilder = new LandingLocaleViewModelBuilder();
                }
                return _modelBuilder;
            }
        }
        #endregion

        #region Fields
        LandingLocaleViewModelBuilder _modelBuilder;
        #endregion
        // GET: Landing
        public ActionResult Index()
        {
            
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
            ViewData["localeModel"] = ModelBuilder.PortfolioView();
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

        public ActionResult Team()
        {
            return View();
        }
        #endregion
    }
}