using HtmlExtensions.StaticRenderers;
using Logic.Extensions.Models;
using Microsoft.AspNet.Identity;
using Runtasker.LocaleBuilders.Views.Payment;
using Runtasker.Logic;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Models.Payments.YandexKassa;
using Runtasker.Logic.Workers;
using Runtasker.Logic.Workers.Logging;
using Runtasker.Logic.Workers.Payments;
using Runtasker.Logic.Workers.Payments.PaymentGetters;
using Runtasker.Logic.Workers.PaymentTransactions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace Runtasker.Controllers
{
    [EnableCors("*", "*", "*")]
    [Authorize(Roles = "Customer")]
    public class PaymentController : Controller
    {
        #region Поля

        MyDbContext _context;

        YandexPaymentWorker _paymentWorker;

        RobokassaPaymentMethods robokassaWorker;

        CustomerPaymentTransactionsWorker _PTWorker;

        PaymentViewModelBuilder _viewsHelper;
        #endregion

        #region Свойства
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

        PaymentViewModelBuilder ViewsHelper
        {
            get
            {
                if(_viewsHelper == null)
                {
                    _viewsHelper = new PaymentViewModelBuilder();
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


        #region Http Методы

        //Здесь пользователь может выбрать каким способом оплачивать
        [HttpGet]
        [Authorize]
        public ActionResult Index(decimal? sumToPay)
        {
            ViewData["localeModel"] = ViewsHelper.Index();
            return View(model: sumToPay);
        }

        #region ЯндексКасса
        [HttpGet]
        [Authorize]
        public ActionResult YandexKassa(decimal? sumToPay)
        {
            if(Settings.Settings.AppSetting == Settings.Enumerations.ApplicationSettingType.Production)
            {
                return RedirectToAction("Index");
            }


            if (sumToPay.HasValue && sumToPay.Value <= 0)
            {
                RedirectToAction("Index");
            }
            ViewData["localeModel"] = ViewsHelper.YandexKassa();

            ViewData["sumToPay"] = sumToPay;
            ViewData["userGuid"] = UserGuid;
            ViewData["userEmail"] = User.Identity.GetEmail();

            return View();
        }

        /// <summary>
        ///  Получение уведомления от кассы пока сделано уебищно
        ///  потом поставь через contrib библиотеку
        /// </summary>
        /// <param name="action"></param>
        /// <param name="orderSumAmount"></param>
        /// <param name="orderSumCurrencyPaycash"></param>
        /// <param name="orderSumBankPaycash"></param>
        /// <param name="shopId"></param>
        /// <param name="invoiceId"></param>
        /// <param name="customerNumber"></param>
        /// <param name="MD5"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<string> YandexKassa(string action = null, string orderSumAmount = null,
            string orderSumCurrencyPaycash = null, string orderSumBankPaycash = null,
            string shopId = null, string invoiceId = null, string customerNumber = null,
            string MD5 = null)
        {
            using (LoggingWorker logger = new LoggingWorker())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"action={action}\n")
                .Append($"orderSumAmount={orderSumAmount}\n")
                .Append($"orderSumCurrencyPaycash={orderSumCurrencyPaycash}\n")
                .Append($"orderSumBankPaycash={orderSumBankPaycash}\n")
                .Append($"shopId={shopId}\n")
                .Append($"invoiceId={invoiceId}\n")
                .Append($"customerNumber={customerNumber}\n")
                .Append($"MD5={MD5}\n");
                

                string fileName = "yandexKassaPostTest.txt";

                logger.LogTextToFile(fileName, sb.ToString());
            }


            if (Settings.Settings.AppSetting == Settings.Enumerations.ApplicationSettingType.Production)
            {
                return "999";
            }
            //code = "0"    такой заказ есть в магазине, 
            //можно продолжать оплату магазин принимает оплаченный заказ
            //code = "1"    полученная MD5-сумма не совпадает с MD5-суммой 
            //на стороне магазина тоже самое
            //code = "100"  такого заказа нет в магазине в авизо такого ответа нет
            //code = "200"  не удается выполнить разбор полученных параметров   тоже самое

            YandexKassaPaymentGetter getter = new YandexKassaPaymentGetter(Context);

            WorkerResult result = await getter.YandexKassaPaymentStartedAsync(action: action,
                orderSumAmount: orderSumAmount,
                orderSumBankPaycash: orderSumBankPaycash,
                MD5: MD5,
                customerNumber: customerNumber,
                invoiceId: invoiceId,
                orderSumCurrencyPaycash: orderSumCurrencyPaycash,
                shopId: shopId);

            int responseCode = 0;

            if(!result.Succeeded)
            {
                responseCode = 1;
            }

            Response.ContentType = "application/xml";
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                $"<checkOrderResponse performedDatetime=\"{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture)}\" " +
                $"code=\"{responseCode}\" invoiceId=\"{invoiceId}\" shopId=\"{shopId}\"/>";
            
        }

        #endregion

        #region Старые методы оплаты
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

            ViewData["localeModel"] = ViewsHelper.Robokassa(sumToPay, sumToPay * 1.07m, HtmlSigns.Rouble);

            return View(model);
        }
        #endregion


        [HttpGet]
        public ActionResult History()
        {
            IEnumerable<PaymentTransaction> model = PTWorker.GetTransactions();
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Succeeded(YandexKassaPaymentResponse response)
        {
            if(response == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["localeModel"] = ViewsHelper.Succeeded(decimal.Parse(response.orderSumAmount, CultureInfo.InvariantCulture), HtmlSigns.Rouble);

            return View(response);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Failed()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Paid()
        {
            return View();
        }

        #region Обработчики оплаты

        #region Бомжовские обработчики
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<string> YandexKassaPaid(string action = null, string orderSumAmount = null,
            string orderSumCurrencyPaycash = null, string orderSumBankPaycash = null,
            string shopId = null, string invoiceId = null, string customerNumber = null, 
            string MD5 = null)
        {
            YandexKassaPaymentGetter getter = new YandexKassaPaymentGetter(Context);

            WorkerResult result = await getter.YandexKassaNotificatedAsync(action: action, orderSumAmount: orderSumAmount,
                orderSumCurrencyPaycash: orderSumCurrencyPaycash, orderSumBankPaycash: orderSumBankPaycash,
                shopId: shopId, invoiceId: invoiceId, customerNumber: customerNumber,
                MD5: MD5);

            int succeededCode = 0;
            if (result.Succeeded)
            {
                succeededCode = 0;
            }
            else
            {
                succeededCode = 100;
            }

            Response.ContentType = "application/xml";
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                $"<paymentAvisoResponse performedDatetime=\"{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture)}\" " +
                $"code=\"{succeededCode}\" invoiceId=\"{invoiceId}\" shopId=\"{shopId}\"/>";

        }
        #endregion

        #endregion
    }
}