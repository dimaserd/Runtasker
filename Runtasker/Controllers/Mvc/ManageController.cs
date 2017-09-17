using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Runtasker.Models;
using Runtasker.Logic.Workers;
using Runtasker.Logic.Workers.Invitations;
using Runtasker.Logic.Models;
using Logic.Extensions.Models;
using Runtasker.LocaleBuilders.Views.Manage;
using Runtasker.Settings.Enumerations;
using Runtasker.Logic.Models.ManageModels;
using Runtasker.Logic.Entities;
using System.Data.Entity;
using Runtasker.Resources.Views.Manage.Index;
using Runtasker.Controllers.Base;

namespace Runtasker.Controllers
{
    [Authorize]
    public class ManageController : BaseMvcController
    {
        #region Поля

        #region Мной созданные
        
        InvitationWorker _inviter;
        AvatarWorker _avatar;
        ManageLocaleViewModelBuilder _viewModelBuilder;

        #endregion

        #region Стандартные
        
        #endregion

        #endregion

        #region Конструкторы
        public ManageController()
        {
        }
        
        #endregion

        #region Свойства

        #region Мной созданные
        

        AvatarWorker Avatarer
        {
            get
            {
                if (_avatar == null)
                {
                    _avatar = new AvatarWorker(UserGuid);
                }
                return _avatar;
            }
        }

        InvitationWorker Inviter
        {
            get
            {
                if(_inviter == null)
                {
                    _inviter = new InvitationWorker(UserGuid, new Logic.MyDbContext());
                }
                return _inviter;
            } 
        }

        ManageLocaleViewModelBuilder ViewModelBuilder
        {
            get
            {
                if(_viewModelBuilder == null)
                {
                    _viewModelBuilder = new ManageLocaleViewModelBuilder();
                }
                return _viewModelBuilder;
            }
        }
        #endregion

        

        #endregion

        #region Http обработчики

        #region Созданные мной методы

        #region Добавление ВкИнфо

        [HttpGet]
        public async Task<ActionResult> AddVkInfo()
        {
            if(Settings.Settings.AppSetting == ApplicationSettingType.Production)
            {
                return RedirectToAction("Index", "Home");
            }

            ApplicationUser customer = await Db.Users.FirstOrDefaultAsync(x => x.Id == UserGuid);

            return View(new AddVkInfoModel
            {
                VkDomain = customer.VkDomain,
                VkId = customer.VkId
            });
        }

        [HttpPost]
        public async Task<ActionResult> AddVkInfo(AddVkInfoModel model)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser customer = await Db.Users.FirstOrDefaultAsync(x => x.Id == UserGuid);

                customer.VkDomain = model.VkDomain;
                customer.VkId = model.VkId;

                Db.Entry(customer).State = EntityState.Modified;
                await Db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }
        #endregion

        #region Приглашение пользователя

        [HttpGet]
        public ActionResult InviteUser()
        {
            SendInvitationModel model = Inviter.GetSendInvitationModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult InviteUser(SendInvitationModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            WorkerResult result = Inviter.SendInvitation(model);
            if(!result.Succeeded)
            {
                foreach(string error in  result.ErrorsList)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion

        [HttpGet, ActionName("Profile")]
        public ActionResult ViewProfile()
        {
            ViewBag.avatarPath = Avatarer.GetAvatarPath();
            return View();
        }

        #region Обновление данных
        [HttpGet]
        public async Task<ActionResult> Update()
        {
            string userId = UserGuid;
            ApplicationUser currentUser = await Db.Users.FirstOrDefaultAsync(x => x.Id == userId);

            return View(currentUser);
        }

        [HttpPost]
        public ActionResult Update(string d)
        {
            return View();
        }

        #endregion

        [HttpGet]
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? IndexRes.ChangePasswordSuccess
                : message == ManageMessageId.SetPasswordSuccess ? IndexRes.SetPasswordSuccess
                : message == ManageMessageId.SetTwoFactorSuccess ? "Настроен поставщик двухфакторной проверки подлинности."
                : message == ManageMessageId.Error ? IndexRes.Error
                : message == ManageMessageId.AddPhoneSuccess ? "Ваш номер телефона добавлен."
                : message == ManageMessageId.RemovePhoneSuccess ? "Ваш номер телефона удален."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        

        #region AddPhoneNumber Methods

        [HttpGet]
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Создание и отправка маркера
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        #endregion

        #region TwoFactor Auth Methods

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        #endregion

        #region PhoneNumber Methods

        [HttpGet]
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Отправка SMS через поставщик SMS для проверки номера телефона
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // Это сообщение означает наличие ошибки; повторное отображение формы
            ModelState.AddModelError("", "Не удалось проверить телефон");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        #endregion

        #region Password Methods

        [HttpGet]
        public ActionResult ChangePassword()
        {
            ViewData["localeModel"] = ViewModelBuilder.ChangePasswordView();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            ViewData["localeModel"] = ViewModelBuilder.ChangePasswordView();

            if (!ModelState.IsValid)
            {
                
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }

            
            AddErrors(result);
            return View(model);
        }

        [HttpGet]
        public ActionResult SetPassword()
        {
            ViewData["localeModel"] = ViewModelBuilder.SetPasswordView();

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // Это сообщение означает наличие ошибки; повторное отображение формы
            return View(model);
        }

        #endregion

        #region Login Methods
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        [HttpGet]
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "Your external local login removed."
                : message == ManageMessageId.Error ? "An error occured."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Запрос перенаправления к внешнему поставщику входа для связывания имени входа текущего пользователя
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        [HttpGet]
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        #endregion

        #endregion

        #endregion

        protected override void Dispose(bool disposing)
        {
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

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion

    }
}