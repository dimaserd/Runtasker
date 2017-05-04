using Extensions.Decimal;
using HtmlExtensions.HtmlEntities;
using HtmlExtensions.StaticRenderers;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Enumerations;
using Runtasker.Resources.Notifications.CustomerOrderErrorMethods;
using System;

namespace Runtasker.Logic.Workers.Notifications
{
    public class CustomerOrderNotificationErrorMethods
    {
        #region Конструкторы
        public CustomerOrderNotificationErrorMethods(string userGuid, MyDbContext context)
        {
            UserGuid = userGuid;
            Context = context;
            
        }

        void Construct()
        {
            
        }
        #endregion

        #region Свойства
        string UserGuid { get; set; }

        MyDbContext Context { get; set; }
        #endregion

        #region Методы по событиям

        public void OnCustomerTriedToAddAnOrderWithUnconfirmedAccount(SaveChangesType saveType = SaveChangesType.Now)
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
            Context.Notifications.Add(N);

            if(saveType == SaveChangesType.Now)
            {
                Context.SaveChanges();
            } 
        }

        public void OnCustomerTriedToPayWithoutMoney(decimal balance, decimal sumThatUserNeedToPay, int orderId, SaveChangesType saveType = SaveChangesType.Now)
        {
            int sumToPay = int.Parse(Math.Round(((sumThatUserNeedToPay) - balance), MidpointRounding.AwayFromZero).ToString());
            if(sumToPay % 10 != 0)
            {
                sumToPay += (10 - sumToPay % 10);
            }


            string balanceString = balance.ToMoney();
            string orderHalfPriceString = (sumThatUserNeedToPay).ToMoney();

            Notification N = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                UserGuid = UserGuid,
                Type = NotificationType.Danger,
                Title = CustOrderErrorRes.PayWithoutMoneyTitle,

                Text = string.Format(CustOrderErrorRes.PayWithoutMoneyTextFormat, balanceString, HtmlSigns.Rouble, orderId, orderHalfPriceString, sumToPay),
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
            Context.Notifications.Add(N);

            if(saveType == SaveChangesType.Now)
            {
                Context.SaveChanges();
            }
            

        }
        #endregion
    }
}
