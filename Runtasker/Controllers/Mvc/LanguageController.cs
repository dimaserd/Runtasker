using System.Globalization;
using System.Linq;
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
            "ru-RU", "en-GB", "zh-CN"
        };

        #endregion

        #region Help Methods
        bool IsCorrectLang(string language)
        {
            return langs.ToList().Any(x => x == language);
        }
        #endregion

        // GET: Language
        public ActionResult Index()
        {
            return View("NewIndex");
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

        public ActionResult ChangeTheme(string returnUrl)
        {
            UIStatics.UIStaticVariables.IsDarkLayout = !UIStatics.UIStaticVariables.IsDarkLayout;

            HttpCookie cookie = new HttpCookie("IsDarkLayout");
            cookie.Value = UIStatics.UIStaticVariables.IsDarkLayout.ToString().ToLower();
            Response.Cookies.Add(cookie);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }


        public string TestLangCodes()
        {

            return $"ThreadCode: {Thread.CurrentThread.CurrentCulture.DisplayName}\n" + 
                $"Resource LangCode: {LocaleBuilders.Statics.LanguageStatic.LanguageCode}\n" +
                $"JS LangCode: {LocaleBuilders.Statics.LanguageStatic.JSLangCode}";
        }
    }
}