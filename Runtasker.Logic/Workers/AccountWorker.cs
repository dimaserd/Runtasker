using Extensions.String;
using Logic.Extensions.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Enumerations;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Notifications;
using Runtasker.Logic.Workers.Payments;
using Runtasker.Settings;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers
{

    public enum RegistrationType
    {
        WithPassword, WithoutPassword
    }
    //For now it contains help methods to AccountController
    public class AccountWorker : IDisposable
    {
        #region Constructors
        
        public AccountWorker(UserManager<ApplicationUser> userManager, MyDbContext context, string userGuid)
        {
            Construct(userManager, context, userGuid);
        }

        void Construct(UserManager<ApplicationUser> userManager, MyDbContext context, string userGuid)
        {
            UserManager = userManager;
            Context = context;
            UserGuid = userGuid;

            Paymenter = new AccountPaymentMethods(context);
            Notificater = new AccountNotificationMethods(Context);
        }
        #endregion

        #region Поля
        public DisposingInternalObjectsSetting DisposeInternalObjects = DisposingInternalObjectsSetting.Yes;
        #endregion

        #region Свойства

        #region Intenal Properties
        UserManager<ApplicationUser> UserManager { get; set; }
        MyDbContext Context { get; set; }
        string UserGuid { get; set; }

        AccountNotificationMethods Notificater { get; set; }
        AccountPaymentMethods Paymenter { get; set; }
        #endregion

        #endregion

        #region Public Methods

        public ApplicationUser RegisterCustomer(RegisterModel model, out IdentityResult iResult)
        {
            //создание объекта пользователя
            var user = model.ToCustomer();

            //создание пользователя с введенным паролем из модели
            IdentityResult result = UserManager.Create(user, model.Password);
            if (result.Succeeded)
            {
                //добавляем к роли
                UserManager.AddToRole(user.Id, "Customer");

                
                //делаем лог в таблицу Payments
                Paymenter.OnUserRegistered(user);
            }
            iResult = result;
            return user;
        }

        //public async Task<ApplicationUser>RegisterCustomerAsync()

        public ApplicationUser RegisterCustomerFromSocialProvider(ExternalLoginConfirmationModel model, 
            ExternalLoginInfo info, out IdentityResult iResult)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Balance = UISettings.RegistrationBonus,
                EmailConfirmed = false,
                RegistrationDate = DateTime.Now,
                Name = info.DefaultUserName.GetNameAndSurname(),
                Language = GetLanguage()
            };

            iResult = UserManager.Create(user);
            if (iResult.Succeeded)
            {
                //добавляем роль
                UserManager.AddToRole(user.Id, "Customer");
                //добавляем внешнюю учетную запись для входа
                iResult = UserManager.AddLogin(user.Id, info.Login); 
            }

            return user;

        }

        public async Task<IdentityResult> ConfirmEmail(string userId, string code)
        {
            return await UserManager.ConfirmEmailAsync(userId, code);
        }

        public bool UserWithEmailExists(string email)
        {
            return Context.Users.Any(u => u.Email == email);
        }

        #region Methods like events

        public void OnUserRegistered(ApplicationUser user, string callBackUrl)
        {
            Notificater.OnUserRegistered(user, callBackUrl);
            return;
        }

        public void OnUserRegisteredFromSocialProvider(ApplicationUser user, string callBackUrl, string providerName)
        {
            Notificater.OnUserRegisteredFromSocialProvider(user, callBackUrl, providerName);
            return;
        }

        public void OnUserConfirmedEmail(string userGuid)
        {
            Notificater.OnUserConfirmedEmail(userGuid);
        }

        #endregion

        #region With Invitations

        public ApplicationUser RegisterInvitedUser(RegisterByInvitationModel model, out IdentityResult Iresult)
        {
            Invitation I = Context.Invitations.FirstOrDefault(i => i.ReceiverEmail == model.Email);
            if(I == null)
            {
                Iresult = new IdentityResult("An invitation for this email not found!");
                return null;
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                EmailConfirmed = false,
                Balance = UISettings.RegistrationBonus
            };
            var result = UserManager.Create(user, model.Password);
            if (result.Succeeded)
            {
                Paymenter.OnUserRegistered(user);
                UserManager.AddToRole(user.Id, "Customer");
            }

            I.Status = InvitationStatus.UserRegistered;
            I.ReceiverGuid = user.Id;
            Context.SaveChanges();


            Notificater.OnUserRegisteredFromInvitation(user, I);

            Iresult = result;
            return user;
        }
    
        public RegisterByInvitationModel TryGetRegisterByInvitationModel(string id, out InfoModel infoModel)
        {
            Invitation I = Context.Invitations.FirstOrDefault(i => i.Id == id);
            if (I == null)
            {
                infoModel = new InfoModel
                {
                    Title = $"An invitation with given id does not exist!",
                    Text = $"Check out the given link in email! If everything is right contact us, " +
                    "we will solve your problem! " +
                    "<a href='/Home/Contact'>Contact link</a>"
                };
                return null;
            }

            if (I.Status != InvitationStatus.Sent)
            {
                infoModel = new InfoModel
                {
                    Title = $"User has already registered by this invitation!",
                    Text = "Maybe this invitation was for you! If you forgot your password "
                    + "<a href='/Account/ForgotPassword'>Click here!</a>"
                };
                return null;
            }

            infoModel = null;
            return new RegisterByInvitationModel
            {
                Email = I.ReceiverEmail,
                InvitationId = I.Id
            };
        }
        
        #endregion

        #endregion

        #region Help Methods
        string GetLanguage()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }

        public ApplicationUser RegisterCustomer(RegistrationType regType, RegisterModel model)
        {
            //создание объекта пользователя
            ApplicationUser user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                EmailConfirmed = false,
                Balance = UISettings.RegistrationBonus,
                RegistrationDate = DateTime.Now,
                Language = GetLanguage()
            };

            if (regType == RegistrationType.WithPassword)
            {             
                //создание пользователя с введенным паролем из модели
                IdentityResult result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    //добавляем к роли
                    UserManager.AddToRole(user.Id, "Customer");
                    //делаем лог в таблицу Payments
                    Paymenter.OnUserRegistered(user);
                }

                return user;
            }
            else if(regType == RegistrationType.WithoutPassword)
            {
                //создание пользователя с введенным паролем из модели
                IdentityResult result = UserManager.Create(user);
                if (result.Succeeded)
                {
                    //добавляем к роли
                    UserManager.AddToRole(user.Id, "Customer");

                    //создаем другое инфо по ползователю
                    //await CreateOtherUserInfo(user);

                    //делаем лог в таблицу Payments
                    Paymenter.OnUserRegistered(user);
                }

                return user;
            }

            //невозможный исход
            return null;
            
        }

        public async Task<ApplicationUser> RegisterCustomerAsync(RegistrationType regType, RegisterModel model)
        {
            //создание объекта пользователя
            ApplicationUser user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                EmailConfirmed = false,
                Balance = UISettings.RegistrationBonus,
                RegistrationDate = DateTime.Now,
                Language = GetLanguage()
            };

            if (regType == RegistrationType.WithPassword)
            {
                //создание пользователя с введенным паролем из модели
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //добавляем к роли
                    await UserManager.AddToRoleAsync(user.Id, "Customer");
                    //делаем лог в таблицу Payments
                    Paymenter.OnUserRegistered(user);
                }

                return user;
            }
            else if (regType == RegistrationType.WithoutPassword)
            {
                //создание пользователя с введенным паролем из модели
                IdentityResult result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    //добавляем к роли
                    UserManager.AddToRole(user.Id, "Customer");

                    //создаем другое инфо по ползователю
                    await CreateOtherUserInfo(user);

                    //делаем лог в таблицу Payments
                    Paymenter.OnUserRegistered(user);
                }

                return user;
            }

            //невозможный исход
            return null;

        }

        async Task<WorkerResult> CreateOtherUserInfo(ApplicationUser user)
        {
            OtherUserInfo otherInfo = new OtherUserInfo
            {
                Id = user.Id
            };

            Context.OtherUserInfos.Add(otherInfo);
            await Context.SaveChangesAsync();

            return new WorkerResult
            {
                Succeeded = true
            };
        }
        
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue && DisposeInternalObjects == DisposingInternalObjectsSetting.Yes)
            {
                //то что нужно учничтожить
                IDisposable[] toDisposes = new IDisposable[]
                {
                    UserManager, Context
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

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
