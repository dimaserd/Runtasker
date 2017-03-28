using Extensions.Decimal;
using HtmlExtensions.HtmlEntities;
using HtmlExtensions.Renderers;
using HtmlExtensions.StaticRenderers;
using Runtasker.Logic.Entities;
using Runtasker.Resources.Notifications.CustomerOrderErrorMethods;
using System;

namespace Runtasker.Logic.Workers.Notifications
{
    public class CustomerOrderNotificationErrorMethods
    {
        #region Constructors
        public CustomerOrderNotificationErrorMethods(string userGuid)
        {
            Construct(userGuid);
        }

        void Construct(string userGuid)
        {
            UserGuid = userGuid;

            
        }
        #endregion

        #region Properties
        string UserGuid { get; set; }

        
        #endregion

        #region Methods like Events

        public void OnCustomerTriedToAddAnOrderWithUnconfirmedAccount()
        {
            using (MyDbContext context = new MyDbContext())
            {
                Notification N = new Notification
                {
                    UserGuid = UserGuid,
                    AboutType = NotificationAboutType.Ordinary,
                    Title = CustOrderErrorRes.UnconfirmedAttemptTitle,
                    Text = CustOrderErrorRes.UnconfirmedAttemptText,
                    Type = NotificationType.Danger,
                    Link = null
                };
                context.Notifications.Add(N);
                context.SaveChanges();
            }
        }

        public void OnCustomerTriedToPayWithoutMoney(decimal balance, Order order)
        {
            int sumToPay = int.Parse(Math.Round(((order.Sum / 2) - balance), MidpointRounding.AwayFromZero).ToString());
            if(sumToPay % 10 != 0)
            {
                sumToPay += (10 - sumToPay % 10);
            }

            using (MyDbContext context = new MyDbContext())
            {
                string balanceString = balance.ToMoney();
                string orderHalfPriceString = (order.Sum / 2).ToMoney();

                Notification N = new Notification
                {
                    AboutType = NotificationAboutType.Ordinary,
                    UserGuid = UserGuid,
                    Type = NotificationType.Danger,
                    Title = CustOrderErrorRes.PayWithoutMoneyTitle,

                    Text = string.Format(CustOrderErrorRes.PayWithoutMoneyTextFormat, balanceString, HtmlSigns.Rouble, order.Id, orderHalfPriceString, sumToPay),
                                                                //format                0                 1               2           3                    4
                    Link = new HtmlLink
                    (
                        hrefParam: $"/Payment/Index?sumToPay={sumToPay}",
                        textParam: string.Format(CustOrderErrorRes.TopUpBalanceFormat, sumToPay, HtmlSigns.Rouble),
                        //                              format                            0         1
                        buttonSizeParam: HtmlButtonSize.Large,
                        buttonTypeParam: HtmlButtonType.Success
                    ).ToString(),
                };
                context.Notifications.Add(N);
                context.SaveChanges();
            }
        }
        #endregion
    }
}
