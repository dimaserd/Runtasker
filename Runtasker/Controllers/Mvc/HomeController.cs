using Logic.Extensions.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using Runtasker.LocaleBuilders.Views.Home;
using Runtasker.Logic;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Enumerations.Notifications.Anonymous;
using Runtasker.Logic.Models;
using Runtasker.Logic.Models.Orders;
using Runtasker.Logic.Workers;
using Runtasker.Logic.Workers.Email;
using Runtasker.Logic.Workers.Orders;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Runtasker.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        #region Поля
        HomeViewModelBuilder _modelBuilder;

        ContactWorker _contacter;

        KnowPriceWorker _knowPriceWorker;

        #region Стандартные
        ApplicationSignInManager _signInManager;

        UserManager<ApplicationUser> _userManager;

        MyDbContext _db;
        #endregion

        #endregion

        #region Свойства
        ContactWorker Contacter
        {
            get
            {
                if(_contacter == null)
                {
                    _contacter = new ContactWorker(UserGuid);
                }
                return _contacter;
            }
        }

        KnowPriceWorker KnowPricer
        {
            get
            {
                if(_knowPriceWorker == null)
                {
                    _knowPriceWorker = new KnowPriceWorker(UserManager, Db);
                }
                return _knowPriceWorker;
            }
        }

        HomeViewModelBuilder ModelBuilder
        {
            get
            {
                if(_modelBuilder == null)
                {
                    _modelBuilder = new HomeViewModelBuilder();
                }
                return _modelBuilder;
            }
        }

        #region Standard Properties
        string UserGuid
        {
            get
            {
                if(Request.IsAuthenticated)
                {
                    return User.Identity.GetUserId();
                }
                return null;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public UserManager<ApplicationUser> UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        MyDbContext Db
        {
            get
            {
                if(_db == null)
                {
                    _db = new MyDbContext();
                }
                return _db;
            }
        }
        #endregion
        
        #endregion

        #region HttpController methods

        //Get Home/Index
        public ActionResult Index(AnonymousNotificationType? notType = null)
        { 
            if(!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Landing");
            }
            //передаю вид уведомления
            ViewData["notType"] = notType;

            //передаю локализованную модель
            ViewData["localeModel"] = ModelBuilder.HomeView();

            return View();
        }

        public ActionResult News()
        {
            return View();
        }

        public ActionResult Menu()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Comments()
        {
            ViewData["localeModel"] = ModelBuilder.CommentsView();
            return View();
        }

        #region Landing
        public ActionResult Landing()
        {
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }

            return View();
        }
        #endregion

        #region KnowPrice methods
        [HttpGet]
        [Route("Home/KnowPrice")]
        public ActionResult KnowPrice(int? workTypeId = null)
        {
            if (User.IsInRole("Performer") || User.IsInRole("Admin") || User.IsInRole("VkPerformer"))
            {
                return RedirectToAction("Index", "Performer");
            }

            if(User.IsInRole("Customer"))
            {
                return RedirectToAction("Create", "Orders");
            }

            //отдаем локализованную модель в представление
            ViewData["localeModel"] = ModelBuilder.KnowPriceView();

            AnonymousKnowThePrice model = new AnonymousKnowThePrice()
            {
                CompletionDate = DateTime.Now.AddDays(3)
            };
            if (workTypeId.HasValue && workTypeId.Value >= 0 && workTypeId.Value <= 2)
            {
                OrderWorkType workTypeFromInt = (OrderWorkType)workTypeId.Value;
                model.WorkType = workTypeFromInt;
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> KnowPrice(AnonymousKnowThePrice model)
        {
            //отдаем локализованную модель в представление
            ViewData["localeModel"] = ModelBuilder.KnowPriceView();

            if (ModelState.IsValid)
            {
                //для каждого задания свои сроки выполнения
                switch (model.WorkType)
                {
                    case OrderWorkType.Ordinary:
                        if ((model.CompletionDate - DateTime.Now).TotalDays < 2)
                        {
                            ModelState.AddModelError("", Resources.Views.Orders.Create.Create.FinishDateErrorOrdinary);
                            return View(model);
                        }
                        break;

                    case OrderWorkType.Essay:
                        if ((model.CompletionDate - DateTime.Now).TotalDays < 7)
                        {
                            ModelState.AddModelError("", Resources.Views.Orders.Create.Create.FinishDateErrorEssay);
                            return View(model);
                        }
                        break;

                    case OrderWorkType.CourseWork:
                        if ((model.CompletionDate - DateTime.Now).TotalDays < 30)
                        {
                            ModelState.AddModelError("", Resources.Views.Orders.Create.Create.FinishDateErrorCourseWork);
                            return View(model);
                        }

                        break;

                    default:
                        break;
                }

                WorkerResult result = await KnowPricer.CreateUserAndOrderAsync(model);

                //если все прошло успешно пользователь входит на сайт
                //и переходит в меню своих заказов
                if (result.Succeeded)
                {
                    //получаем данные для авторизации пользователя в системе
                    ApplicationUser userToLogin = KnowPricer.RegisteredUser;
                    string userPass = KnowPricer.RegisteredPass;

                    SendRegistrationEmail(userToLogin, userPass);

                    //авторизуем пользователя
                    SignInManager.PasswordSignIn(userToLogin.UserName, userPass, true, shouldLockout: false);

                    return RedirectToAction("Index", "Orders");
                }

                if (!result.Succeeded && result.ErrorsList[0] == "User with given email is already exist")
                {
                    //так же нужно создать локальное уведомление 
                    //в котором нужно объяснить всю ситуацию
                    return RedirectToAction("Login", "Account");
                }

                return View();
            }
            else
            {
                return View(model);
            }

        }

        #endregion

        #region Contact Methods
        [HttpGet]
        public ActionResult Contact(bool? messageSent = null)
        {
            ViewData["messageSent"] = messageSent;

            //отдаем локализованную модель в представление
            ViewData["localeModel"] = ModelBuilder.ContactView();
            

            return View(new ContactViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Contact(ContactViewModel model)
        {
            if(ModelState.IsValid && await CheckCaptcha())
            {
                Contacter.OnContactMessageReceived(model);
                
                //message should be generated here or in ContactWorker
                //but request might be not authenticated
                
                return RedirectToAction("Contact", "Home", routeValues: new { messageSent = true });
            }

            //отдаем локализованную модель в представление
            ViewData["localeModel"] = ModelBuilder.ContactView();
            return View(model);
        }
        #endregion

        #endregion

        #region Help Methods
        void SendRegistrationEmail(ApplicationUser user, string pass)
        {
            using (AccountEmailMethods emailer = new AccountEmailMethods())
            {
                string callBackUrl = GetCallBackUrl(user);
                emailer.OnUserRegisteredWithCreatedOrder(user, pass, callBackUrl);
            }
        }

        string GetCallBackUrl(ApplicationUser user)
        {
            string code = UserManager.GenerateEmailConfirmationToken(user.Id);

            string callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            return callbackUrl;
        }
        #endregion

        #region Captcha methods
        private async Task<bool> CheckCaptcha()
        {
            string captchaResponse = Request.Form["g-recaptcha-response"].ToString();
            string remoteIP = Request.UserHostAddress;

            JObject result = await JsonCaptchaRequest(captchaResponse, remoteIP);
            
            return (result["success"].Type == JTokenType.Boolean && result["success"].ToObject<bool>() == true);
        }

        async Task<JObject> JsonCaptchaRequest(string captchaResponse, string remoteIP)
        {
            string secret = "6LfGJQsUAAAAANyg9zLgMZ5BORfI0hLj3d8trLJX";
            string uri = @"https://www.google.com/recaptcha/api/siteverify";

            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "secret", secret },
                    { "response", captchaResponse },
                    { "remoteip", remoteIP }
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(uri, content);

                var responseString = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(responseString);
                return json;
            }

            
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                IDisposable[] toDisposes = new IDisposable[]
                {
                    _userManager, _knowPriceWorker, _contacter,
                    _signInManager, _db, _modelBuilder
                };
                
                for(int i = 0; i < toDisposes.Length; i++)
                {
                    if(toDisposes[i] != null)
                    {
                        toDisposes[i].Dispose();
                        toDisposes[i] = null;
                    }
                }
                
            }
            
            base.Dispose(disposing);
        }
        #endregion
    }
}