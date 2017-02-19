using Microsoft.AspNet.Identity;
using Runtasker.Logic;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.ViewModelBuilders.Payment;
using Runtasker.Logic.Workers;
using Runtasker.Logic.Workers.Payments;
using Runtasker.Logic.Workers.PaymentTransactions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace Runtasker.Controllers
{
    [Authorize(Roles = "Customer")]
    public class PaymentController : Controller
    {
        #region Fields

        MyDbContext _context;

        YandexPaymentWorker _paymentWorker;

        RobokassaPaymentMethods robokassaWorker;

        CustomerPaymentTransactionsWorker _PTWorker;

        PaymentViewsLocaleHelper _viewsHelper;
        #endregion

        #region Properties
        MyDbContext Context
        {
            get
            {
                if(_context == null)
                {
                    _context = new MyDbContext();
                }
                return _context;
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
                return null;
            }
        }

        PaymentViewsLocaleHelper ViewsHelper
        {
            get
            {
                if(_viewsHelper == null)
                {
                    _viewsHelper = new PaymentViewsLocaleHelper();
                }
                return _viewsHelper;
            }
        }

        #region Payment Properties
        YandexPaymentWorker YandexWorker
        {
            get
            {
                if(_paymentWorker == null)
                {
                    _paymentWorker = new YandexPaymentWorker(Context);
                }
                return _paymentWorker;
            }
        }    

        RobokassaPaymentMethods RoboKassaWorker
        {
            get
            {
                if(robokassaWorker == null)
                {
                    robokassaWorker = new RobokassaPaymentMethods(Context, UserGuid);
                }
                return robokassaWorker;
            }
        }

        CustomerPaymentTransactionsWorker PTWorker
        {
            get
            {
                if(_PTWorker == null)
                {
                    _PTWorker = new CustomerPaymentTransactionsWorker(Context, UserGuid);
                }
                return _PTWorker;
            }
        }

        #endregion

        #endregion

        #region HttpController methods

        //We have index method where user  can choose his way to recharge balance
        [HttpGet]
        [Authorize]
        public ActionResult Index(decimal? sumToPay)
        {
            ViewData["localeModel"] = ViewsHelper.Index();
            return View(model: sumToPay);
        }


        [HttpGet]
        [Authorize]
        public ActionResult Yandex(decimal? sumToPay)
        {
            if(sumToPay.HasValue && sumToPay.Value <= 0)
            {
                RedirectToAction("Index");
            }
            ViewData["localeModel"] = ViewsHelper.Yandex();

            ViewData["sumToPay"] = sumToPay;
            ViewData["userGuid"] = UserGuid;

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Robokassa(decimal sumToPay)
        {
            if (sumToPay <= 0)
            {
                RedirectToAction("Index");
            }
            RoboKassaPaymentModel model = RoboKassaWorker.GetPaymentModel(sumToPay);

            ViewData["localeModel"] = ViewsHelper.Robokassa(sumToPay, sumToPay * 1.07m);

            return View(model);
        }

        [HttpGet]
        public ActionResult History()
        {
            IEnumerable<PaymentTransaction> model = PTWorker.GetTransactions();
            return View(model);
        }

        

        

        [HttpGet]
        public ActionResult Paid()
        {
            return View();
        }

        #region Paid Post Methods

        [HttpPost]
        [AllowAnonymous]
        public string Paid(string amount = null, string withdraw_amount = null, string notification_type = null, 
            string operation_id = null,
            string label = null, string datetime = null, string sender = null, string sha1_hash = null,
            string currency = null, bool codepro = false)
            //менял кодпро
        {
            string RootDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");

            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

                YandexWorker.GetPayment(amount: amount, withdraw_amount: withdraw_amount,
                        notification_type: notification_type, operation_id: operation_id,
                        label: label, datetime: datetime, sender: sender,
                        sha1_hash: sha1_hash, codepro: codepro, currency: currency);
            }
            catch (Exception ex)
            {
                string filePath = $"{RootDirectory}/yandex_exceptions.txt";
                string fileContents = $"{DateTime.Now} label : {label}, amount : {amount} \n" +
                    $"Exception {ex.Message}";

                System.IO.File.AppendAllText(filePath, fileContents);
            }
            
            return "принято";
        }


        [HttpPost]
        [AllowAnonymous]
        public string RoboKassaPaid(string OutSum, string InvId, string SignatureValue)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

            RoboKassaWorker.OnPaymentReceived(OutSum, InvId, SignatureValue);
            return $"OK{InvId}";
        }
        #endregion

        #endregion
    }
}