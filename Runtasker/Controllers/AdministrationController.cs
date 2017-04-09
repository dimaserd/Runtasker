using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Runtasker.Logic.Models;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Admin;
using Logic.Extensions.Models;
using Runtasker.Logic.Workers.Admin.Users;
using System.Data.Entity.Validation;
using Runtasker.Logic.Handlers;
using System.Data.Entity.Migrations;

namespace Runtasker.Controllers
{
    //в этом контроле можно будет наделять пользователей
    //различными правами присваивать им роли
    //в дальнейшем скорее всего будет использоваться
    //для отслеживания деятельности исполнителей
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        #region Fields
        static MyDbContext _db;
        UserManager<ApplicationUser> _userManager;
        AdminCustomerMethods _adminWorker;
        AdminUserCreationWorker _userCreator;
        #endregion

        #region Properties
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

        UserManager<ApplicationUser> UserManager
        {
            get
            {
                if(_userManager == null)
                {
                    _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Db));
                }
                return _userManager;
            } 
        }

        AdminCustomerMethods AdminWorker
        {
            get
            {
                if (_adminWorker == null)
                {
                    _adminWorker = new AdminCustomerMethods(Db);
                }

                return _adminWorker;
            }
        }

        AdminUserCreationWorker UserCreator
        {
            get
            {
                if(_userCreator == null)
                {
                    _userCreator = new AdminUserCreationWorker(Db, UserManager);
                }
                return _userCreator;
            }
        }
        #endregion

        #region HttpController Methods
        // GET: Administration
        public ActionResult Index()
        {
            return View();
        }
        #region OtherInfo Methods

        #region UpdatePerformerInfo methods
        [HttpGet]
        public async Task<ActionResult> UpdatePerformerInfo(string id)
        {
            
            OtherUserInfo model = await Db.OtherUserInfos
                   .Where(x => x.Id == id).FirstOrDefaultAsync();

            if(model == null)
            {
                throw new Exception("Не существует информации об исполнителе!");
            }
            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> UpdatePerformerInfo(OtherUserInfo model)
        {
            OtherUserInfo infoToUpdate = await Db.OtherUserInfos.FirstOrDefaultAsync(x => x.Id == model.Id);

            infoToUpdate.Specialization = model.Specialization;
            infoToUpdate.VkDomain = model.VkDomain;
            infoToUpdate.VkId = model.VkId;

            
            await Db.SaveChangesAsync();
            
            

            return RedirectToAction("Performers");
        }
        
        #endregion

        #endregion
        #region CreateUser Methods
        [HttpGet]
        public async Task<ActionResult> CreateUser()
        {
            var model = await UserCreator.GetCreateUserModelAsync();
           
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(CreateUserModel model)
        {
            #region Полчуение обратно
            List<IdentityRole> roles = await Db.Roles.ToListAsync();

            roles.Remove(roles.FirstOrDefault(x => x.Name == "Admin"));

            MultiSelectList rolesList = new MultiSelectList(roles,
                "Name", "Name");


            model.UserRolesList = rolesList;
            #endregion

            if (ModelState.IsValid)
            {
                WorkerResult result = await UserCreator.CreateUserAsync(model);
                if(!result.Succeeded)
                {
                    AddErrors(result);
                    return View(model);
                }

                return RedirectToAction("Index");
            }


            
            return View(model);
        }
        #endregion

        #region AddUserToRole Methods
        [HttpGet]
        public async Task<ActionResult> AddUserToRole(string userId)
        {
            
            ViewBag.Roles = new SelectList(await Db.Roles.ToListAsync(), "Name", "Name");
            AddUserToRoleModel model = new AddUserToRoleModel
            {
                User = await Db.Users.FirstOrDefaultAsync(x => x.Id == userId)
            };

            return View();
        }

        [HttpPost]
        public ActionResult AddUserToRole(AddUserToRoleModel model)
        {
            if(ModelState.IsValid)
            {

            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Admin Methods
        [HttpGet]
        public ActionResult Users()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Performers()
        {
            IEnumerable<ApplicationUser> model = await AdminWorker.GetPerformersAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Customers()
        {
            IEnumerable<ApplicationUser> model = await AdminWorker.GetCustomersAsync();

            return View(model);
        }
        #region Statistics methods
        [HttpGet]
        public async Task<ActionResult> CustomerStatistics(string id)
        {
            
            CustomerInfo model = await AdminWorker.GetCustomerInfoAsync(id);

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> PerformerStatistics(string id)
        {
            PerformerInfo model = await AdminWorker.GetPerformerInfoAsync(id);
            return View(model);
        }
        #endregion

        #region Delete Methods

        #region Delete Order
        [HttpGet]
        public ActionResult DeleteAllOrders(string id)
        {
            IEnumerable<Order> model = AdminWorker.Delete.GetDeleteAllUserOrdersModel(id);
            return View(viewName: "DeleteOrders", model: model);
        }
        #region Delete One Order methods
        [HttpGet]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            Order model = await AdminWorker.Delete.GetDeleteOrderModelAsync(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteOrder(Order model)
        {
            WorkerResult result = AdminWorker.Delete.DeleteOrderConfirmed(model);
            if(result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            AddErrors(result);
            return View(model);
        }
        #endregion
        [HttpGet]
        public ActionResult DeleteFinishedOrders(string id)
        {
            IEnumerable<Order> model = AdminWorker.Delete.GetDeleteFinishedUserOrdersModel(id);
            return View(viewName: "DeleteOrders", model: model);
        }

        [HttpPost]
        public ActionResult DeleteOrders(IEnumerable<Order> model)
        {
            AdminWorker.Delete.DeleteOrders(model);

            return RedirectToAction("Index");
        }

        #endregion

        #region Delete User
        [HttpGet]
        public ActionResult DeleteUser(string id)
        {
            ApplicationUser model = AdminWorker.Delete.GetDeleteUserModel(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteUser(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                AdminWorker.Delete.DeleteUserConfirmed(user);
            }
            return RedirectToAction("Customers");
        }
        #endregion

        //TODO
        #region Delete Message Methods
        public ActionResult DeleteMessage(int id)
        {

            return View();
        }
        #endregion
        #endregion


        #region WriteEmail methods
        [HttpGet]
        public ActionResult WriteEmail(string id)
        {
            AdminWorker.SetCustomerField(id);

            ViewData["user"] = AdminWorker.GetCustomer();
            ActionEmailToCustomer model = AdminWorker.GetWriteEmailModel();

            return View(model);
        }

        //TODO
        [HttpPost]
        public ActionResult WriteEmail(ActionEmailToCustomer model)
        {
            AdminWorker.WriteEmailToCustomer(model);
            return RedirectToAction("Users");
        }
        #endregion


        #endregion

        #endregion

        #region Help Methods
        void AddErrors(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        void AddErrors(WorkerResult result)
        {
            foreach (string error in result.ErrorsList)
            {
                ModelState.AddModelError("", error);
            }
        }

        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if(_db != null)
            {
                _db.Dispose();
                _db = null;
            }

            if(_userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}