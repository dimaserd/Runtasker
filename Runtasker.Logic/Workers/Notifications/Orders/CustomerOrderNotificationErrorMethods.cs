using Extensions.Decimal;
using HtmlExtensions.HtmlEntities;
using HtmlExtensions.Renderers;
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

            FASigns = new FontAwesomeRenderer();
            GISigns = new GlyphiconRenderer();
            HtmlSigns = new HtmlSignsRenderer();
        }
        #endregion

        #region Properties
        string UserGuid { get; set; }

        FontAwesomeRenderer FASigns { get; set; }
        GlyphiconRenderer GISigns { get; set; }
        HtmlSignsRenderer HtmlSigns { get; set; }
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
                Notification N = new Notification
                {
                    AboutType = NotificationAboutType.Ordinary,
                    UserGuid = UserGuid,
                    Type = NotificationType.Danger,
                    Title = CustOrderErrorRes.PayWithoutMoneyTitle,
                    Text = $"{CustOrderErrorRes.YourBalance}: {balance.ToMoney()}{HtmlSigns.Rouble}. " +
                $"{CustOrderErrorRes.PayWithoutMoneyText1} {CustOrderErrorRes.Number}{order.Id} " +
                $"{CustOrderErrorRes.PayWithoutMoneyText2} {(order.Sum / 2).ToMoney()}{HtmlSigns.Rouble}. " +
                $"{CustOrderErrorRes.PayWithoutMoneyText3} {sumToPay}{HtmlSigns.Rouble}.",
                    Link = new HtmlLink
                    (
                        hrefParam: $"/Payment/Index?sumToPay={sumToPay}",
                        textParam: $"{CustOrderErrorRes.RechargeBalance} {sumToPay}{HtmlSigns.Rouble}",
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
