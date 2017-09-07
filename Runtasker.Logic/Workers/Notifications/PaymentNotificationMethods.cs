using Extensions.Decimal;
using HtmlExtensions.HtmlEntities;
using HtmlExtensions.Renderers;
using HtmlExtensions.StaticRenderers;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Enumerations;
using Runtasker.Resources.Notifications.PaymentMethods;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Runtasker.Logic.Workers.Notifications
{
    public class PaymentNotificationMethods : IDisposable
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

        #region Методы

        #region Внешние методы
        /// <summary>
        /// метод передающий пользователя из внешней среды в этот класс
        /// </summary>
        /// <param name="customer"></param>
        public void SetCustomer(ApplicationUser customer)
        {
            _user = customer;
        }
        #endregion

        #region Методы по событиям

        public void OnUserPaid(Payment p, SaveChangesType saveType = SaveChangesType.Now)
        {
            //устанавливаем локаль пользователя чтобы все уведомления записались
            //на выбранном им языке
            SetCustomerCulture(p);

            //получаю заказ в системе
            Order activeOrder = Context.Orders.FirstOrDefault(
                o => o.UserGuid == p.UserGuid &&
            (o.Status == OrderStatus.Finished || o.Status == OrderStatus.Estimated));


            //выбираем сервис оплаты
            string ViaService = string.Empty;

            switch(p.ViaType)
            {
                case PaymentViaType.Robokassa:
                    ViaService = PaymentNotRes.ViaRoboKassa;
                    break;

                case PaymentViaType.YandexMoney:
                    ViaService = PaymentNotRes.ViaYandexMoney;
                    break;

                case PaymentViaType.YandexKassa:
                    ViaService = PaymentNotRes.ViaYandexKassa;
                    break;

                case PaymentViaType.Administration:
                    ViaService = PaymentNotRes.ViaRoboKassa;
                    break;
            }
            

            //создаем уведомление
            Notification N = new Notification
            {
                AboutType = NotificationAboutType.Balance,
                Status = NotificationStatus.Unseen,
                Type = NotificationType.Info,
                UserGuid = p.UserGuid,
                Title = string.Format(PaymentNotRes.PaymentReceivedTitleFormat, p.Amount.ToMoney(), HtmlSigns.Rouble, ViaService),
                Text = string.Format(PaymentNotRes.YourBalanceFormat, GetUserBalanceString(p), HtmlSigns.Rouble),
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
                    hrefParam: (activeOrder.Status == OrderStatus.Estimated) ?
                    $"/Orders/PayHalf/{activeOrder.Id}"
                    : $"/Orders/PayAnotherHalf/{activeOrder.Id}",
                    textParam: string.Format(PaymentNotRes.PayOrderFormat, FASigns.PlusCircle.Lg(), activeOrder.Id),
                    buttonSizeParam: HtmlButtonSize.Large,
                    buttonTypeParam: HtmlButtonType.Success
                ).ToString()

            };

            //добавляю уведомление в базу
            Context.Notifications.Add(N);


            if(saveType == SaveChangesType.Now)
            {
                //сохраняю изменения
                Context.SaveChanges();
            }
        }

        #endregion

        #region Вспомогательные методы
        
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

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PaymentNotificationMethods() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
