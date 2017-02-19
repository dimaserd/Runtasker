using Extensions.Decimal;
using HtmlExtensions.HtmlEntities;
using HtmlExtensions.Renderers;
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
            FASigns = new FontAwesomeRenderer();
            GISigns = new GlyphiconRenderer();
        }
        #endregion

        #region Fields
        ApplicationUser _user;
        #endregion

        #region Properties
        MyDbContext Context { get; set; }

        FontAwesomeRenderer FASigns {get;set;}
        GlyphiconRenderer GISigns { get; set; }
        #endregion

        #region Methods like Events
        //We need to set customerculture
        public void OnUserPaid(Payment p)
        {
            SetCustomerCulture(p);

            Order activeOrder = Context.Orders.FirstOrDefault(
                o => o.UserGuid == p.UserGuid &&
            (o.Status == OrderStatus.Finished || o.Status == OrderStatus.Valued));




            Notification N = new Notification
            {
                AboutType = NotificationAboutType.Balance,
                Status = NotificationStatus.Unseen,
                Type = NotificationType.Info,
                UserGuid = p.UserGuid,
                Title = $"{PaymentNotRes.PaymentReceived} {p.Amount.ToMoney()}{FASigns.Rouble} {PaymentNotRes.Via} "
                +( (p.ViaType == PaymentViaType.Robokassa)? PaymentNotRes.RoboKassa : PaymentNotRes.YandexMoney)
                + $" {PaymentNotRes.Service}!",
                Text = $"{PaymentNotRes.YourBalance} {GetUserBalance(p)}{FASigns.Rouble}!",
                Link = (activeOrder == null) ? 
                new HtmlLink
                (
                    hrefParam: "/Orders/Create",
                    textParam: $"{FASigns.PlusCircle.Lg()} {PaymentNotRes.CreateOrder}",
                    buttonSizeParam: HtmlButtonSize.Large,
                    buttonTypeParam: HtmlButtonType.Success
                ).ToString()

                : new HtmlLink
                (
                    hrefParam: (activeOrder.Status == OrderStatus.Valued) ?
                    $"/Orders/PayHalf/{activeOrder.Id}"
                    : $"/Orders/PayAnotherHalf/{activeOrder.Id}",
                    textParam: $"{FASigns.PlusCircle.Lg()} {PaymentNotRes.PayOrder} №{activeOrder.Id}",
                    buttonSizeParam: HtmlButtonSize.Large,
                    buttonTypeParam: HtmlButtonType.Success
                ).ToString()

            };
            Context.Notifications.Add(N);
            Context.SaveChanges();
        }

        #endregion

        #region Help Methods
        string GetUserBalance(Payment payment)
        {
            if (_user == null)
            {
                SetUserByPayment(payment);
            }
            return _user.Balance.ToMoney();
        }

        void SetCustomerCulture(Payment payment)
        {
            if(_user == null)
            {
                SetUserByPayment(payment);
            }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(_user.Language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(_user.Language);

        }

        void SetUserByPayment(Payment payment)
        {
            _user = Context.Users.FirstOrDefault(u => u.Id == payment.UserGuid);
        }
        #endregion
    }
}
