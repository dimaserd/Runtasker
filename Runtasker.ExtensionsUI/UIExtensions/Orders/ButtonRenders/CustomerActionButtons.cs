using Runtasker.Logic.Entities;
using System.Text;
using Extensions.Decimal;
using System;
using Runtasker.ExtensionsUI.Statics;

namespace Runtasker.ExtensionsUI.UIExtensions.Orders
{
    /// <summary>
    /// Этот класс выдает кнопки действий для сущности заказ
    /// </summary>
    public class CustomerOrderActionButtons : OrderHtmlButtonsBase
    {
        
        #region Constructors
        public CustomerOrderActionButtons(CustomerOrderHtmlEntity orderEntity, int unreadMesCount) : base(orderEntity, unreadMesCount)
        {
            Construct();
        }

        void Construct()
        {
            
        }
        #endregion

        

        #region Вспомогательные методы

        #region Генераторы кнопок

        string GetButtonForDeleting()
        {
            return CustomerBtns.DeleteBtn(Order.Id, BtnClass).ToString();
        }

        /// <summary>
        /// Возвращает кнопку для чата о заказе. 
        /// После нажатия кнопка откроется в модале
        /// </summary>
        /// <returns></returns>
        string GetButtonForChatAboutOrder()
        {
            return CustomerBtns.ChatBtn(Order.Id, UnreadMessagesCount, BtnClass).ToString();
        }

        /// <summary>
        /// Получает кнопки для только что созданного заказа
        /// </summary>
        /// <returns></returns>
        string GetButtonsForNewOrder()
        {
            StringBuilder buttons = new StringBuilder();

            buttons.Append(CustomerBtns.EstimationBtn(BtnClass));

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
                buttons.Append(CustomerBtns.PayFirstHalfBtn(Order.Id, sumToPayString, BtnClass));

            }
            else
            {
                buttons.Append(CustomerBtns.PayOnlineHelpBtn(Order.Id, sumToPayString, BtnClass));
            }

            return buttons.ToString();
        }

        string GetButtonsForHalfPaidOrder()
        {
            StringBuilder buttons = new StringBuilder();

            buttons.Append(CustomerBtns.ExecutingBtn(BtnClass));

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

            buttons.Append(CustomerBtns.PaySecondHalfBtn(Order.Id, sumToPayString, BtnClass));

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
                buttons.Append(CustomerBtns.DownloadSolutionBtn(Order.Id, BtnClass));
            }
            else
            {
                //если событие онлайн помощи уже завершено и заказ еще не оценен
                if((DateTime.Now - Order.FinishDate).TotalDays >= 1 && Order.Status != OrderStatus.Appreciated)
                {
                    buttons.Append(CustomerBtns.RatingBtn(orderId: Order.Id, btnClass: BtnClass));
                }
                else
                {
                    buttons.Append(CustomerBtns.WaitingForHelpEventBtn(Order.FinishDate, BtnClass));
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
            buttons.Append(CustomerBtns.DownloadSolutionBtn(Order.Id, BtnClass));

            buttons.Append(CustomerBtns.RatingBtn(orderId: Order.Id, btnClass: BtnClass));
            return buttons.ToString();
        }

        string GetButtonsForAppreciatedOrder()
        {
            StringBuilder buttons = new StringBuilder();
            buttons.Append(CustomerBtns.FinishedBtn(BtnClass));

            //у онлайн заказов нет решения
            if(Order.WorkType != OrderWorkType.OnlineHelp)
            {
                buttons.Append(CustomerBtns.DownloadSolutionBtn(Order.Id, BtnClass));
            }
            

            return buttons.ToString();
        }

        string GetButtonsForHasErrorOrder()
        {
            switch(Order.ErrorType)
            {
                case OrderErrorType.NeedDescription:
                    return CustomerBtns.AddDescriptionBtn(Order.Id, BtnClass).ToString();


                case OrderErrorType.NeedFiles:
                    return CustomerBtns.AddFilesBtn(Order.Id, BtnClass).ToString();

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
            return buttons + GetButtonForChatAboutOrder() + GetButtonForDeleting();
        }

        #endregion

    }
}
