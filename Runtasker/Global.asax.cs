using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using Runtasker.Logic.Entities;
using Runtasker.Statics.Settings;

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

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            // Log the exception.

            ILogger logger = Container.Resolve<ILogger>();
            logger.Error(exception);

            Response.Clear();

            HttpException httpException = exception as HttpException;

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");

            if (httpException == null)
            {
                routeData.Values.Add("action", "Index");
            }
            else //It's an Http Exception, Let's handle it.
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // Page not found.
                        routeData.Values.Add("action", "HttpError404");
                        break;
                    case 500:
                        // Server error.
                        routeData.Values.Add("action", "HttpError500");
                        break;

                    // Here you can handle Views to other error codes.
                    // I choose a General error template  
                    default:
                        routeData.Values.Add("action", "General");
                        break;
                }
            }

            // Pass exception details to the target error View.
            routeData.Values.Add("error", exception);

            // Clear the error on server.
            Server.ClearError();

            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;

            // Call target Controller and pass the routeData.
            IController errorController = new ErrorController();
            errorController.Execute(new RequestContext(
                 new HttpContextWrapper(Context), routeData));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            ChangeThreadLanguage();
            ChangeLayout();
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
            if (!UIGlobalSettings.LayoutSwitchEnabled)
            {
                UIStatics.UIStaticVariables.IsDarkLayout = false;
                return;
            }

            //получаю куку о фоне
            HttpCookie cookie = HttpContext.Current.Request.Cookies["IsDarkLayout"];

            if (cookie == null)
            {
                HttpCookie newCookie = new HttpCookie("IsDarkLayout");
                newCookie.Expires = DateTime.Now.AddYears(1);

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
