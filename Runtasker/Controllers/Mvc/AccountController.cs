﻿using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic;
using Microsoft.AspNet.Identity;
using Runtasker.Logic.Workers.Notifications;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Invitations;
using Runtasker.Logic.Workers;
using Runtasker.Resources.Controllers.Account;
using Runtasker.LocaleBuilders.Views.Account;
using System;
using Runtasker.Logic.Workers.Orders;
using Runtasker.Logic.Workers.Email;
using Runtasker.Logic.Entities;
using Runtasker.Settings;
using HtmlExtensions.StaticRenderers;
using Runtasker.Logic.Enumerations.InfoModels;

namespace Runtasker.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Поля
        
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private RoleManager<IdentityRole> _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new MyDbContext()));

        #region Мои поля
        MyDbContext _context;
        InvitationWorker _invitater;
        AccountWorker _accountWorker;
        KnowPriceWorker _knowPriceWorker;
        
        private AccountNotificationMethods _notificater;

        #endregion

        #endregion

        #region Свойства

        #region Стандартные свойства

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

        public ApplicationUserManager UserManager
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

        

        public AccountNotificationMethods Notificater
        {
            get
            {
                if (_notificater == null)
                {
                    _notificater = new AccountNotificationMethods(Context);
                }
                return _notificater;
            }

        }

        #endregion

        #region Мои свойства
        MyDbContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new MyDbContext();
                }
                return _context;
            }
        }

        string FilesDir
        {
            get { return System.Web.Hosting.HostingEnvironment.MapPath("~/Files"); }
        }

        InvitationWorker Invitator
        {
            get
            {
                if (_invitater == null)
                {
                    _invitater = new InvitationWorker(UserGuid, Context);
                }
                return _invitater;
            }
        }

        AccountWorker AccountWorker
        {
            get
            {
                if (_accountWorker == null)
                {
                    _accountWorker = new AccountWorker(UserManager, Context, UserGuid);
                }

                return _accountWorker;
            }
        }
        
        string UserGuid
        {
            get
            {
                if (Request.IsAuthenticated)
                {
                    return User.Identity.GetUserId();
                }
                else
                {
                    return "";
                }
            }
        }

        

        KnowPriceWorker KnowPricer
        {
            get
            {
                if (_knowPriceWorker == null)
                {
                    _knowPriceWorker = new KnowPriceWorker(UserManager, Context);
                }
                return _knowPriceWorker;
            }
        }

        #endregion

        #endregion


        #region Конструкторы
        public AccountController()
        {
            Construct();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Construct();
        }

        void Construct()
        {
            
        }
        #endregion

        

        #region HttpController methods


        #region Temporary Dev Methods
        [AllowAnonymous]
        public string Roles()
        {
            string[] roles = DevSettings.RolesInApp;

            foreach (string role in roles)
            {
                if (!_roleManager.RoleExists(role))
                {
                    _roleManager.Create(new IdentityRole(role) { Name = role });
                }
            }

            return "Готово";
        }

        [AllowAnonymous]
        public string Dev()
        {
            //Добавление ролей в систему
            Roles();

            string customerEmail = DevSettings.TestCustomerEmail;
            string performerEmail = DevSettings.AdminEmail;

            var maybeCustomer = UserManager.FindByEmail(customerEmail);
            if (maybeCustomer == null)
            {
                var customer = new ApplicationUser
                {
                    Email = customerEmail,
                    EmailConfirmed = true,
                    Balance = UISettings.RegistrationBonus,
                    Language = "ru-RU",
                    Name = "Dmitry",
                    UserName = customerEmail
                };

                UserManager.Create(customer, DevSettings.TestPassword);
                UserManager.AddToRole(customer.Id, "Customer");
            }
            else
            {
                UserManager.AddToRole(maybeCustomer.Id, "Customer");
            }

            var maybePerformer = UserManager.FindByEmail(performerEmail);
            if (maybePerformer == null)
            {
                var performer = new ApplicationUser
                {
                    Email = performerEmail,
                    EmailConfirmed = true,
                    Balance = UISettings.RegistrationBonus,
                    Language = "ru-RU",
                    Name = "Dmitry",
                    UserName = performerEmail,
                    VkDomain = DevSettings.AdminVkDomain,
                    Specialization = "0,1,2,3,4,5,6,7,8,9"
                };

                UserManager.Create(performer, DevSettings.TestPassword);
                UserManager.AddToRole(performer.Id, "Admin");
                UserManager.AddToRole(performer.Id, "Performer");

                
                
                Context.SaveChanges();
            }
            else
            {
                UserManager.AddToRole(maybePerformer.Id, "Admin");
                UserManager.AddToRole(maybePerformer.Id, "Performer");
            }    

            return "готово";
        }
        #endregion
        
        /// <summary>
        /// Обновляет куки текущего пользователя. Используется для смены баланса после его пополнения или снятия.
        /// </summary>
        public void ReSignIn()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
            }
        }

        #region Login Methods
        /// <summary>
        /// Частичное представление страницы с логином. Представление бывает двух видов. Для анонимного пользователя представление
        /// представляет форму для входа. Для авторизованного пользователя представление показывает некоторые сведения,
        /// такие как: адрес электронной почты, баланс, а также кнопки действий.
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Signin()
        {
            
            return PartialView();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            ViewData["viewModel"] = AccountViewModelBuilder.LoginView();

            
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            ViewData["viewModel"] = AccountViewModelBuilder.LoginView();
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {   
                return View(model);
            }

            // Require the user to have a confirmed email before they can log on.
            var user = await UserManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", AccountRes.WrongInput);
                return View(model);
            }

            // Сбои при входе не приводят к блокированию учетной записи
            // Чтобы ошибки при вводе пароля инициировали блокирование учетной записи, замените на shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", AccountRes.WrongInput);
                    return View(model);
            }
        }

        #endregion

        #region VerifyCode Methods

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Требовать предварительный вход пользователя с помощью имени пользователя и пароля или внешнего имени входа
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Приведенный ниже код защищает от атак методом подбора, направленных на двухфакторные коды. 
            // Если пользователь введет неправильные коды за указанное время, его учетная запись 
            // будет заблокирована на заданный период. 
            // Параметры блокирования учетных записей можно настроить в IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Неправильный код.");
                    return View(model);
            }
        }

        #endregion

        #region Register Methods
        [AllowAnonymous]
        public ActionResult Register()
        {
            if(Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["viewModel"] = AccountViewModelBuilder.RegisterView();
            return View();
        }

       
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterModel model)
        {

            ViewData["viewModel"] = AccountViewModelBuilder.RegisterView();
            if (ModelState.IsValid)
            {
                ApplicationUser user = AccountWorker.RegisterCustomer(model, out IdentityResult result);
                
                if(!result.Succeeded)
                {
                    AddErrors(result);
                    return View(model);
                }

                //авторизуем пользователя
                await SignInManager.SignInAsync(user, true, true);

                //отправляем подтверждающее письмо
                SendConfirmationEmail(user);

                return RedirectToAction("Info", "Notification", routeValues: new { infoType = InfoModelType.ToConfirmEmail });   
            }
            return View(model : model);
        }

        #region Invitations
        [HttpGet]
        [AllowAnonymous]
        public ActionResult RegisterByInvitation(string id)
        {
            if(Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            
            RegisterByInvitationModel model = AccountWorker.TryGetRegisterByInvitationModel(id, out InfoModel infoModel);

            if(model == null)
            {
                return RedirectToAction("Info", "Notifications", routeValues: infoModel);
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegisterByInvitation(RegisterByInvitationModel model)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            
            ApplicationUser user = AccountWorker.RegisterInvitedUser(model, out IdentityResult result);
            if(!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            SendConfirmationEmail(user);

            return RedirectToAction("Info", "Notification", routeValues: new { infoType = InfoModelType.ToConfirmEmail });
        }
        #endregion

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await AccountWorker.ConfirmEmail(userId, code);

            if(!result.Succeeded)
            {
                return View("Error");
            }

            ////Убрал асинхронные методы
            //ApplicationUser user = UserManager.FindById(userId);


            //if(user != null)
            //{
            //    SignInManager.SignIn(user, true, true);
            //}
            

            ViewData["localeModel"] = AccountViewModelBuilder.ConfirmEmailView(GISigns.Login.ToString());
            return View();
        }

        #endregion

        #region ForgotPassword Methods

        [AllowAnonymous]
        public ActionResult ForgotPassword(bool? emailSent)
        {
            ViewData["localeModel"] = AccountViewModelBuilder.ForgotPasswordView();
            ViewData["emailSent"] = emailSent;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            ViewData["localeModel"] = AccountViewModelBuilder.ForgotPasswordView();
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("error", $"User with email : {model.Email} not found!");
                    
                    return View(model);
                }

                //Дополнительные сведения о том, как включить подтверждение учетной записи и сброс пароля, см. по адресу: http://go.microsoft.com/fwlink/?LinkID=320771
                // Отправка сообщения электронной почты с этой ссылкой
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                string callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                Notificater.OnUserForgottenPassword(user, callbackUrl);
                
                return RedirectToAction("ForgotPassword", "Account", routeValues: new { emailSent = true });
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            
            return View(model);
        }


        
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region ResetPassword Methods
        
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if(code == null)
            {
                return View("Error");
            }
            else
            {
                ViewData["localeModel"] = AccountViewModelBuilder.ResetPasswordView();
                return View();
            }
            
        }

        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["localeModel"] = AccountViewModelBuilder.ResetPasswordView();
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Не показывать, что пользователь не существует
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            AddErrors(result);
            ViewData["localeModel"] = AccountViewModelBuilder.ResetPasswordView();
            return View();
        }

        
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            ViewData["localeModel"] = AccountViewModelBuilder.ResetPasswordConfirmationView();
            return View();
        }
        #endregion

        #region SendCode methods

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Создание и отправка маркера
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }
        #endregion

        #region External Login Methods
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Запрос перенаправления к внешнему поставщику входа
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Выполнение входа пользователя посредством данного внешнего поставщика входа, если у пользователя уже есть имя входа
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // Если у пользователя нет учетной записи, то ему предлагается создать ее
                    ViewBag.ReturnUrl = returnUrl;

                    ViewData["localeModel"] = AccountViewModelBuilder.ExternalLoginConfirmationView(loginInfo.Login.LoginProvider);
                    return View("ExternalLoginConfirmation", 
                        new ExternalLoginConfirmationModel
                        {
                            Email = loginInfo.Email,
                            ProviderName = loginInfo.Login.LoginProvider
                        });
            }
        }

        
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationModel model, string returnUrl)
        {
            ViewData["localeModel"] = AccountViewModelBuilder.ExternalLoginConfirmationView(model.ProviderName);
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Получение сведений о пользователе от внешнего поставщика входа
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                
                ApplicationUser user = AccountWorker.RegisterCustomerFromSocialProvider(model, info, out IdentityResult result);

                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: true);

                    SendConfirmationEmail(user, providerName: model.ProviderName);

                    return RedirectToAction("Info", "Notification", routeValues: new { infoType = InfoModelType.ToConfirmEmail});
                }

                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        #endregion

        [HttpPost]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            
            return RedirectToAction("Index", "Home");
        }



        #endregion

        #region Help Methods
        void SendConfirmationEmail(ApplicationUser user, string providerName = null)
        {
            string callBackUrl = GetCallBackUrl(user);

            if (providerName == null)
            {
                AccountWorker.OnUserRegistered(user, callBackUrl);
                return;
            }
            AccountWorker.OnUserRegisteredFromSocialProvider(user, callBackUrl, providerName);
        }

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

        
        protected override void Dispose(bool disposing)
        {
            IDisposable[] toDisposes = new IDisposable[]
            {
                _accountWorker, _roleManager, _userManager, _signInManager
            };

            if (disposing)
            {
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

        #region Вспомогательные приложения
        // Используется для защиты от XSRF-атак при добавлении внешних имен входа
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}