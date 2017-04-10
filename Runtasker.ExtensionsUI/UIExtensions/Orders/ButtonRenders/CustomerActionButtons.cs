using HtmlExtensions.StaticRenderers;
using Runtasker.Logic.Entities;
using System.Text;
using Runtasker.Resources.UIExtensions.Orders;
using Extensions.Decimal;
using System.Linq;
using System;
using Runtasker.ExtensionsUI.Statics;

namespace Runtasker.ExtensionsUI.UIExtensions.Orders
{
    //this class produces action link or links for current OrderHtmlEntity
    public class CustomerOrderActionButtons : OrderHtmlButtonsBase
    {
        //TODO add spans with glyphicon
        #region Constructors
        public CustomerOrderActionButtons(CustomerOrderHtmlEntity orderEntity) : base(orderEntity)
        {
            Construct();
        }

        void Construct()
        {
            
        }
        #endregion

        

        #region Help Methods

        #region Buttons parameters

        /// <summary>
        /// Возвращает кнопку для чата о заказе. 
        /// После нажатия кнопка откроется в модале
        /// </summary>
        /// <returns></returns>
        string GetButtonForChatAboutOrder()
        {
            //подсчитываем непрочитанные пользователем сообщения
            int unreadCount = Order.Messages
                .Count(m => m.ReceiverGuid == Order.UserGuid
                    && m.Status == MessageStatus.New);

            return ActionBtns.ChatBtn(Order.Id, unreadCount, GetButtonClass()).ToString();
        }

        /// <summary>
        /// Получает кнопки для только что созданного заказа
        /// </summary>
        /// <returns></returns>
        string GetButtonsForNewOrder()
        {
            StringBuilder buttons = new StringBuilder();

            buttons.Append(ActionBtns.EstimationBtn(GetButtonClass()));

            return buttons.ToString();
        }

        /// <summary>
        /// Кнопки для заказа который был оценен администраторами, оплата должна отличаться
        /// для онлайн помощи и других видов работы
        /// </summary>
        /// <returns></returns>
        string GetButtonsForEstimatedOrder()
        {
            StringBuilder buttons = new StringBuilder();

            //выбираем сумму
            string sumToPayString = ( Order.WorkType != OrderWorkType.OnlineHelp)? (Order.Sum / 2).ToMoney() : (Order.Sum).ToMoney();
            

            if(Order.WorkType != OrderWorkType.OnlineHelp)
            {
                buttons.Append(ActionBtns.PayFirstHalfBtn(Order.Id, sumToPayString, GetButtonClass()));

            }
            else
            {
                buttons.Append(ActionBtns.PayOnlineHelpBtn(Order.Id, sumToPayString, GetButtonClass()));
            }

            return buttons.ToString();
        }

        string GetButtonsForHalfPaidOrder()
        {
            StringBuilder buttons = new StringBuilder();

            buttons.Append(ActionBtns.ExecutingBtn(GetButtonClass()));

            return buttons.ToString();
        }

        /// <summary>
        /// Абсолютно идентична с методом получающим кнопки для 
        /// заказа оплаченного наполовину
        /// </summary>
        /// <returns></returns>
        string GetButtonsForExecutingOrder()
        {
            return GetButtonsForHalfPaidOrder();
        }

        string GetButtonsForFinishedOrder()
        {
            StringBuilder buttons = new StringBuilder();

            string sumToPayString = Order.PaidSum.ToMoney();

            buttons.Append(ActionBtns.PaySecondHalfBtn(Order.Id, sumToPayString, GetButtonClass());

            return buttons.ToString();
        }
        
        /// <summary>
        /// Получает кнопки для полностью оплаченного заказа
        /// </summary>
        /// <returns></returns>
        string GetButtonsForPaidOrder()
        {
            StringBuilder buttons = new StringBuilder();


            if(Order.WorkType != OrderWorkType.OnlineHelp)
            {
                buttons.Append(ActionBtns.DownloadSolutionBtn(Order.Id, GetButtonClass()));
            }
            else
            {
                //если событие онлайн помощи уже завершено
                if((DateTime.Now - Order.FinishDate).TotalDays >= 1)
                {
                    buttons.Append(GetButtonsForDownloadedOrder());
                }
                else
                {
                    buttons.Append(ActionBtns.WaitingForHelpEventBtn(Order.FinishDate, GetButtonClass()));
                } 
            }
            
            return buttons.ToString();
        }


        /// <summary>
        /// Генерирует кнопки для скачаннго заказа либо завершенной онлайн помощи
        /// </summary>
        /// <returns></returns>
        string GetButtonsForDownloadedOrder()
        {
            StringBuilder buttons = new StringBuilder();
            buttons.Append(GetButtonsForPaidOrder());

            buttons.Append(ActionBtns.RatingBtn(orderId: Order.Id, btnClass: GetButtonClass()));
            return buttons.ToString();
        }

        string GetButtonsForAppreciatedOrder()
        {
            StringBuilder buttons = new StringBuilder();
            buttons.Append(ActionBtns.FinishedBtn(GetButtonClass()));

            buttons.Append(GetButtonsForPaidOrder());

            return buttons.ToString();
        }

        string GetButtonsForHasErrorOrder()
        {
            switch(Order.ErrorType)
            {
                case OrderErrorType.NeedDescription:
                    return ActionBtns.AddDescriptionBtn(Order.Id, GetButtonClass()).ToString();


                case OrderErrorType.NeedFiles:
                    return ActionBtns.AddFilesBtn(Order.Id, GetButtonClass()).ToString();

                default:
                    return null;
            }
        }
        #endregion
        
        #endregion

        #region Overriden Methods

        public override string ToString()
        {
            string buttons = "";
            switch(Order.Status)
            {
                case OrderStatus.New:
                    buttons = GetButtonsForNewOrder();
                    break;

                case OrderStatus.Estimated:
                    buttons = GetButtonsForEstimatedOrder();
                    break;

                case OrderStatus.HalfPaid:
                    buttons = GetButtonsForHalfPaidOrder();
                    break;

                case OrderStatus.Executing:
                    buttons = GetButtonsForExecutingOrder();
                    break;

                case OrderStatus.Finished:
                    buttons = GetButtonsForFinishedOrder();
                    break;

                case OrderStatus.FullPaid:
                    buttons = GetButtonsForPaidOrder();
                    break;

                case OrderStatus.Downloaded:
                    buttons = GetButtonsForDownloadedOrder();
                    break;

                case OrderStatus.Appreciated:
                    buttons = GetButtonsForAppreciatedOrder();
                    break;

                case OrderStatus.HasError:
                    buttons = GetButtonsForHasErrorOrder();
                    break;
            }
            return buttons + GetButtonForChatAboutOrder();
        }

        #endregion

    }
}
