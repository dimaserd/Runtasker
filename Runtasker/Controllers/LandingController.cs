using HtmlExtensions.Renderers;
using Runtasker.LocaleBuilders.Views.Landing;
using System.Web.Mvc;

namespace Runtasker.Controllers
{
    public class LandingController : Controller
    {
        #region Properties
        HtmlSignsRenderer HtmlSigns
        {
            get
            {
                if(_htmlSigns == null)
                {
                    _htmlSigns = new HtmlSignsRenderer();
                }
                return _htmlSigns;
            }
        }

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

        HtmlSignsRenderer _htmlSigns;
        #endregion

        #region Http methods
        // GET: Landing
        public ActionResult Index()
        {
            
            return View();
        }

        #region Sections
        public ActionResult Slider()
        {
            ViewData["localeModel"] = ModelBuilder.SliderView();
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
            ViewData["localeModel"] = ModelBuilder.CountersView();
            return View();
        }

        public ActionResult Parallax()
        {
            return View();
        }

        public ActionResult Pricing()
        {
            ViewData["localeModel"] = ModelBuilder.PricingView(50, 200, 500, HtmlSigns.Rouble);
            return View();
        }

        public ActionResult Team()
        {
            return View();
        }
        #endregion

        #endregion
    }
}