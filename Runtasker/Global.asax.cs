using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using Runtasker.Logic.Entities;

namespace Runtasker
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];

            if (cookie == null && !HttpContext.Current.Request.IsAuthenticated)
            {
                string[] languages = HttpContext.Current.Request.UserLanguages;
                if (languages == null)
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                    return;
                }
                string lang = ( languages[0].ToLower().Contains("ru") ) ? "ru-RU" : "en";



                HttpCookie newCookie = new HttpCookie("Language");
                newCookie.Value = lang;
                Response.Cookies.Add(newCookie);

                Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                return;
            }


            if (cookie == null && HttpContext.Current.Request.IsAuthenticated)
            {
                string lang = HttpContext.Current.User.Identity.GetLanguage();

                HttpCookie newCookie = new HttpCookie("Language");
                newCookie.Value = lang;
                Response.Cookies.Add(newCookie);


                Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                return;
            }

            if (cookie != null && cookie.Value != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(cookie.Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cookie.Value);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
        }
    }
}
