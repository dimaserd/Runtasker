using Logic.Extensions.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Models.ManageModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Runtasker.Logic.Workers.Admin.Users
{
    /// <summary>
    /// Создает пользователей в системе и управляет настройками
    /// </summary>
    public class AdminUserCreationWorker
    {
        #region Constructor
        public AdminUserCreationWorker(MyDbContext db, UserManager<ApplicationUser> userManager)
        {
            Db = db;
            UserManager = userManager;

            Construct();
        }

        void Construct()
        {

        }
        #endregion

        #region Properties
        MyDbContext Db { get; set; }

        UserManager<ApplicationUser> UserManager { get; set; }
        #endregion

        #region Public methods
        public async Task<CreateUserModel> GetCreateUserModelAsync()
        {
            List<IdentityRole> roles = await Db.Roles.ToListAsync();

            //исключаем роль админа чтобы случайно никого не создать
            roles.Remove(roles.FirstOrDefault(x => x.Name == "Admin"));

            //создаем список со множественным выбором в котором в контроллер
            //придет список свойств имен а при выборе покажутся имена 
            //также можно выбрать например чтобы приходили айдишники
            MultiSelectList rolesList = new MultiSelectList(roles,
                "Name", "Name");


            return new CreateUserModel
            {
                UserRolesList = rolesList,
            };
        }
           
        public async Task<WorkerResult> CreateUserAsync(CreateUserModel model)
        {
            if (await Db.Users.FirstOrDefaultAsync(x => x.Email == model.Email) != null)
            {
                return new WorkerResult("User with given email already exists!");
                
            }

            ApplicationUser user = new ApplicationUser
            {
                Name = model.Name,
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                RegistrationDate = DateTime.Now,
                Language = "ru-RU",
                Specialization = GetSpecializationString(model.Subjects),
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);
            //если создание пользователя прошло успешно
            if (result.Succeeded)
            {
                //добавляем пользователя к выбранным в контроллере моделям
                foreach (string role in model.UserRoles)
                {
                    IdentityResult roleRes = await UserManager.AddToRoleAsync(user.Id, role);
                }

                
                await Db.SaveChangesAsync();

                return new WorkerResult
                {
                    Succeeded = true
                };
            }
            else
            {
                return new WorkerResult(result.Errors);
            }
            

        }
        #endregion

        #region Help methods
        string GetSpecializationString(IEnumerable<Subject> subjects)
        {
            string result = string.Empty;

            foreach(Subject subject in subjects)
            {
                result += $"{(int)subject},";
            }

            return result;
        }
        #endregion
    }
}
