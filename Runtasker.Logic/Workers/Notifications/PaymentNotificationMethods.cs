using Extensions.Decimal;
using HtmlExtensions.HtmlEntities;
using HtmlExtensions.Renderers;
using HtmlExtensions.StaticRenderers;
using Runtasker.Logic.Entities;
using Runtasker.Resources.Notifications.PaymentMethods;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Runtasker.Logic.Workers.Notifications
{
    public class PaymentNotificationMethods
    {
        #region Constructors
        public PaymentNotificationMethods(MyDbContext context)
        {
            Construct(context);
        }

        void Construct(MyDbContext context)
        {
            Context = context;
        }
        #endregion

        #region Fields
        ApplicationUser _user;
        #endregion

        #region Properties
        MyDbContext Context { get; set; }
        #endregion

        #region Methods like Events
        
        public void OnUserPaid(Payment p)
        {
            //устанавливаем локаль пользователя чтобы все уведомления записались
            //на выбранном им языке
            SetCustomerCulture(p);

            Order activeOrder = Context.Orders.FirstOrDefault(
                o => o.UserGuid == p.UserGuid &&
            (o.Status == OrderStatus.Finished || o.Status == OrderStatus.Valued));


            //выбираем сервис оплаты
            string ViaService = (p.ViaType == PaymentViaType.Robokassa) ? PaymentNotRes.ViaRoboKassa : PaymentNotRes.ViaYandexMoney;

            //создаем уведомление
            Notification N = new Notification
            {
                AboutType = NotificationAboutType.Balance,
                Status = NotificationStatus.Unseen,
                Type = NotificationType.Info,
                UserGuid = p.UserGuid,
                Title = string.Format(PaymentNotRes.PaymentReceivedTitleFormat, p.Amount.ToMoney(), HtmlSigns.Rouble, ViaService),
                Text = string.Format(PaymentNotRes.YourBalanceFormat, GetUserBalanceString(p),HtmlSigns.Rouble),
                Link = (activeOrder == null) ? 
                new HtmlLink
                (
                    hrefParam: "/Orders/Create",
                    textParam: string.Format(PaymentNotRes.CreateOrderFormat, FASigns.PlusCircle.Lg()),
                    buttonSizeParam: HtmlButtonSize.Large,
                    buttonTypeParam: HtmlButtonType.Success
                ).ToString()

                : new HtmlLink
                (
                    hrefParam: (activeOrder.Status == OrderStatus.Valued) ?
                    $"/Orders/PayHalf/{activeOrder.Id}"
                    : $"/Orders/PayAnotherHalf/{activeOrder.Id}",
                    textParam: string.Format(PaymentNotRes.PayOrderFormat, FASigns.PlusCircle.Lg(), activeOrder.Id),
                    buttonSizeParam: HtmlButtonSize.Large,
                    buttonTypeParam: HtmlButtonType.Success
                ).ToString()

            };

            Context.Notifications.Add(N);
            Context.SaveChanges();
        }

        #endregion

        #region Help Methods
        /// <summary>
        /// Возвращает уже преобразованную строку из текущего баланса пользователя.
        /// Например 300 если баланс = 300.00m
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        string GetUserBalanceString(Payment payment)
        {
            if (_user == null)
            {
                SetUserByPayment(payment);
            }
            return _user.Balance.ToMoney();
        }

        /// <summary>
        /// Устанавливает культуру для текущего потока
        /// </summary>
        /// <param name="payment"></param>
        void SetCustomerCulture(Payment payment)
        {
            if(_user == null)
            {
                SetUserByPayment(payment);
            }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(_user.Language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(_user.Language);
        }
        /// <summary>
        /// Устанавливает вспомогательное поле пользователя по платежу
        /// </summary>
        /// <param name="payment"></param>
        void SetUserByPayment(Payment payment)
        {
            _user = Context.Users.FirstOrDefault(u => u.Id == payment.UserGuid);
        }
        #endregion
    }
}
