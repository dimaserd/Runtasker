using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Runtasker.Controllers
{
    public class LanguageController : Controller
    {
        #region Constants
        string[] langs = new string[]
        {
            "ru-RU", "en"
        };

        #endregion

        #region Help Methods
        bool IsCorrectLang(string language)
        {
            bool result = false;
            foreach(string lang in langs)
            {
                if(language == lang)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
        #endregion

        // GET: Language
        public ActionResult Index()
        {
            return View("NewIndex");
        }

        public ActionResult Change(string LanguageAbbrevation)
        {
            if (LanguageAbbrevation != null && IsCorrectLang(LanguageAbbrevation))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbrevation);

                HttpCookie cookie = new HttpCookie("Language");
                cookie.Value = LanguageAbbrevation;
                Response.Cookies.Add(cookie);
            }
            
            

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangeWithRedirect(string LanguageAbbrevation, string returnUrl)
        {
            if (LanguageAbbrevation != null && IsCorrectLang(LanguageAbbrevation))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbrevation);

                HttpCookie cookie = new HttpCookie("Language");
                cookie.Value = LanguageAbbrevation;
                Response.Cookies.Add(cookie);
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}