using HtmlExtensions.Renderers;
using Runtasker.Logic.Entities;
using System.Text;
using Runtasker.Resources.UIExtensions.Orders;
using Extensions.Decimal;
using System.Linq;

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
            FASignsCollection = new FontAwesomeRenderer();
            GISignsCollection = new GlyphiconRenderer();
        }
        #endregion

        #region Properties
        FontAwesomeRenderer FASignsCollection { get; set; }

        GlyphiconRenderer GISignsCollection { get; set; }
        #endregion

        #region Help Methods

        #region Buttons parameters

        //Is called by javascript
        string GetButtonForChatAboutOrder()
        {
            //подсчитываем непрочитанные пользователем сообщения
            int unreadCount = Order.Messages
                .Count(m => m.ReceiverGuid == Order.UserGuid
                    && m.Status == MessageStatus.New);
            return new HtmlActionButtonLink
                (
                    buttonLink: "#",
                    buttonText: $"{GISignsCollection.Envelope} {OrderEntityRes.ChatAboutOrder} {GISignsCollection.Count(unreadCount)}",
                    //for javascript toggler with modal
                    htmlAttributes: new { id = Order.Id, @class = $"{GetButtonClass()} orderChat" }
                ).ToString();
        }

        string GetButtonsForNewOrder()
        {
            StringBuilder buttons = new StringBuilder();

            buttons.Append(new HtmlActionButtonLink
                (
                    buttonLink: "#",
                    buttonText: OrderEntityRes.Estimation,
                    buttonClass: GetButtonClass(),
                    disabled: true
                ).ToString());

            return buttons.ToString();
        }

        string GetButtonsForValuedOrder()
        {
            StringBuilder buttons = new StringBuilder();

            buttons.Append(new HtmlActionButtonLink
                (
                    buttonLink: $"/Orders/PayHalf/{Order.Id}",
                    buttonText: OrderEntityRes.PayHalf,
                    buttonClass: GetButtonClass()
                ).ToString());

            return buttons.ToString();
        }

        string GetButtonsForHalfPaidOrder()
        {
            StringBuilder buttons = new StringBuilder();

            buttons.Append(new HtmlActionButtonLink
                (
                    buttonLink : "#",
                    buttonText: OrderEntityRes.Executing,
                    buttonClass: GetButtonClass(),
                    disabled: true
                ).ToString());

            return buttons.ToString();
        }

        //They are same
        string GetButtonsForExecutingOrder()
        {
            return GetButtonsForHalfPaidOrder();
        }

        string GetButtonsForFinishedOrder()
        {
            StringBuilder buttons = new StringBuilder();

            buttons.Append(new HtmlActionButtonLink
                    (
                       buttonLink: $"/Orders/PayAnotherHalf/{Order.Id}",
                       buttonText: $"{OrderEntityRes.Pay} {Order.PaidSum.ToMoney()}{FASignsCollection.Rouble}",
                       buttonClass: GetButtonClass()
                    ).ToString());

            return buttons.ToString();
        }
        
        string GetButtonsForPaidOrder()
        {
            StringBuilder buttons = new StringBuilder();

            buttons.Append(new HtmlActionButtonLink
                (
                    buttonLink: $"File/DownloadSolution/{Order.Id}",
                    buttonText: $"{FASignsCollection.Download} {OrderEntityRes.DownloadSolution}",
                    buttonClass: GetButtonClass()
                ).ToString());
            return buttons.ToString();
        }

        //rateLink class added for opening link in modal
        string GetButtonsForDownloadedOrder()
        {
            StringBuilder buttons = new StringBuilder();
            buttons.Append(GetButtonsForPaidOrder());

            buttons.Append(new HtmlActionButtonLink
                (
                    buttonText: $"{GISignsCollection.Star} {OrderEntityRes.RatingWork}!",
                    buttonLink: $"Orders/Rating/{Order.Id}",
                    htmlAttributes: new { id = $"{Order.Id}", @class = $"{GetButtonClass()} rateLink" }
                ).ToString());
            return buttons.ToString();
        }

        string GetButtonsForAppreciatedOrder()
        {
            StringBuilder buttons = new StringBuilder();
            buttons.Append(new HtmlActionButtonLink
                (
                    buttonLink: "#",
                    buttonText: OrderEntityRes.Finished,
                    buttonClass: GetButtonClass(),
                    disabled: true
                ).ToString());

            buttons.Append(GetButtonsForPaidOrder());

            return buttons.ToString();
        }

        string GetButtonsForHasErrorOrder()
        {
            switch(Order.ErrorType)
            {
                case OrderErrorType.NeedDescription:
                    return new HtmlActionButtonLink
                        (
                            buttonLink: $"/Orders/AddDescription/{Order.Id}",
                            buttonText: OrderEntityRes.AddDescription,
                            buttonClass: GetButtonClass()
                        ).ToString();


                case OrderErrorType.NeedFiles:
                    return new HtmlActionButtonLink
                        (
                            buttonLink: $"/Orders/AddFiles/{Order.Id}",
                            buttonText: OrderEntityRes.AddFiles,
                            buttonClass: GetButtonClass()
                        ).ToString();

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

                case OrderStatus.Valued:
                    buttons = GetButtonsForValuedOrder();
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
