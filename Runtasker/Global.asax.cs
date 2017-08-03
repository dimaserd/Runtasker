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
            ChangeThreadLanguage();
        }

        private void ChangeThreadLanguage()
        {
            //получаю куку о языке
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];

            //если она не существует и запрос анонимный
            if (cookie == null && !HttpContext.Current.Request.IsAuthenticated)
            {
                //получаю сведения о языках пользователя от браузера
                string[] languages = HttpContext.Current.Request.UserLanguages;

                if (languages == null)
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                    return;
                }
                //обход фишки с сафари браузерами
                string lang = (languages[0].ToLower().Contains("ru")) ? "ru-RU" : "en";


                //создаю куку с настройками языка и добавляю в пользовательские куки
                HttpCookie newCookie = new HttpCookie("Language");
                newCookie.Value = lang;
                Response.Cookies.Add(newCookie);

                //меняю культуру потока
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

        private void ChangeLayout()
        {
            //получаю куку о языке
            HttpCookie cookie = HttpContext.Current.Request.Cookies["IsDarkLayout"];

            if (cookie == null)
            {
                HttpCookie newCookie = new HttpCookie("IsDarkLayout");
                newCookie.Value = false.ToString().ToLower();
                Response.Cookies.Add(newCookie);
                return;
            }
            else
            {
                if(cookie.Value.ToLower().Equals(true.ToString().ToLower()))
                {
                    UIStatics.UIStaticVariables.IsDarkLayout = true;
                }
                else
                {
                    UIStatics.UIStaticVariables.IsDarkLayout = false;
                }
            }
        }
    }
}
