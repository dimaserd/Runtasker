﻿using Extensions.Randoms;
using Logic.Extensions.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Enumerations.OrderWorker;
using Runtasker.Logic.Models;
using Runtasker.Logic.Models.Orders;
using Runtasker.Logic.Workers.Email;
using System;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers.Orders
{
    
    //класс который регистрирует заказы от пользователей
    //фактически происходит регистрация пользователя с заказом
    public class KnowPriceWorker : IDisposable
    {
        #region Constructor
        public KnowPriceWorker()
        {

        }

        public KnowPriceWorker(UserManager<ApplicationUser> userManager, MyDbContext db)
        {
            this.UserManager = userManager;
            _db = db;
        }
        #endregion

        #region Fields
        MyDbContext _db;
        #endregion

        #region Properties

        #region Internal Properties
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

        UserManager<ApplicationUser> UserManager { get; set; }
        
        #endregion

        #region Public Properties
        /// <summary>
        /// Устанавливается в методе при успешной регистрации ползователя
        /// </summary>
        public ApplicationUser RegisteredUser { get; set; }

        public string RegisteredPass { get; set; }
        #endregion

        #endregion

        #region Public Methods

        //проверим асинхронную версию
        public async Task<WorkerResult> CreateUserAndOrderAsync(AnonymousKnowThePrice model)
        {
            //создаем пользователя
            using (AccountWorker accounter = new AccountWorker(UserManager, _db, userGuid: null))
            {
                //говорим объект accounter не уничтожать используемые объекты
                accounter.DisposeInternalObjects = Enumerations.DisposingInternalObjectsSetting.No;

                ApplicationUser existingUser = await UserManager.FindByEmailAsync(model.Email);
                if(existingUser != null)
                {
                    return new WorkerResult("User with given email is already exist");
                }

                string pass = new RandomPassword().GetRandomPass();

                RegisterModel regModel = new RegisterModel
                {
                    Email = model.Email,
                    Password = pass,
                    ConfirmPassword = pass,
                    Name = model.Name
                };

                ApplicationUser user =
                    accounter.RegisterCustomer(RegistrationType.WithPassword, regModel);

                //отправляем электронное письмо с данными о регистрации
                //SendRegistrationEmail(user, pass);

                //записываем данные в свойства чтобы потом передать их контроллеру
                //для авторизации пользователя в системе
                RegisteredUser = user;
                RegisteredPass = pass;

                //создаем объект который создаст заказы
                using (CustomerOrderWorker orderWorker = new CustomerOrderWorker(Db, user.Id))
                {
                    OrderCreateModel createModel = new OrderCreateModel
                    {
                        WorkType = model.WorkType,
                        Description = model.Description,
                        FinishDate = model.CompletionDate,
                        FileUpload = model.Files,
                        OtherSubject = model.OtherSubject,
                        Subject = model.Subject
                    };

                    Order order = await orderWorker.CreateOrderAsync(createModel);
                    if (order != null)
                    {
                        return new WorkerResult
                        {
                            Succeeded = true
                        };
                    }
                    else
                    {
                        return new WorkerResult("Произошла ошибка при добавлении заказа!");
                    }
                }
            }
        }
        #endregion

        #region Help methods
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

            return $"https://runtasker.ru/Account/ConfirmEmail?userId={user.Id}&code={code}";
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~KnowPriceWorker() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        void IDisposable.Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}