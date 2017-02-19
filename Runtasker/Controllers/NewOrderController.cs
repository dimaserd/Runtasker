using Microsoft.AspNet.Identity;
using Runtasker.Logic;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Runtasker.Controllers
{
    public class NewOrderController : Controller
    {
        #region Fields
        NewCustomerOrderWorker _newOrderWorker;

        MyDbContext _db = new MyDbContext();
        #endregion

        #region Properties
        //свойство по которому получаем айди текущего пользователя
        string UserGuid
        {
            get { return User.Identity.GetUserId(); }
        }

        NewCustomerOrderWorker NewOrderWorker
        {
            get
            {
                if (_newOrderWorker == null)
                {
                    _newOrderWorker = new NewCustomerOrderWorker(UserGuid, _db);
                }
                return _newOrderWorker;
            }
        }
        #endregion

        #region Public Methods
        //тестирование методов добавления описания новым 
        //более совершенным классом
        #region AddDescription Methods

        [HttpGet]
        public ActionResult AddDescription(int id)
        {
            AddDescriptionModel model = NewOrderWorker.GetAddDescriptionModel(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddDescription(AddDescriptionModel model)
        {
            if (ModelState.IsValid)
            {
                
                Order order = await NewOrderWorker.AddDescriptionToOrder(model);
                if (order == null)
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }
        #endregion

        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if(_db != null)
            {
                _db.Dispose();
            }
            if (_newOrderWorker != null)
            {
                _newOrderWorker.Dispose();
            }

            _db = null;
            _newOrderWorker = null;

            base.Dispose(disposing);
        }
        #endregion
    }
}