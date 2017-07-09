using HtmlExtensions.StaticRenderers;
using Runtasker.Logic.Entities;
using System.Linq;
using System.Text;

namespace Runtasker.ExtensionsUI.UIExtensions.Orders
{
    public class PerformerActionButtons : OrderHtmlButtonsBase
    {
        #region Constructors

        public PerformerActionButtons(PerformerOrderHtmlEntity orderEntity) : base(orderEntity)
        {
            Construct();
        }

        void Construct()
        {
            
        }
        #endregion

        #region Properties
        

        #endregion

        #region Help Methods

        //NOT DONE NOT USED
        #region Additional Buttons
        string GetButtonForDownloadingSolution()
        {
            if(!((int)Order.Status >= (int)OrderStatus.Finished && Order.Status != OrderStatus.HasError))
            {
                return null;
            }

            return new HtmlActionButtonLink
                (
                    buttonLink: $"File/DownloadSolution/{Order.Id}",
                    buttonText: $"{FASigns.Download} Скачать решение",
                    buttonClass: BtnClass
                ).ToString();
        }
        #endregion

        string GetButtonForChatAboutOrder()
        {
            int unreadCount = Order.Messages
                .Count(m => m.SenderGuid == Order.UserGuid
                && m.Status == MessageStatus.New);
            return new HtmlActionButtonLink
                (
                    buttonLink: $"#",
                    buttonText: $"Чат по заказу {GISigns.Count(unreadCount)}",
                    //for javascript toggler with modal
                    htmlAttributes: new { id = Order.Id, @class = $"{BtnClass} orderChat"  }
                ).ToString();
        }

        string GetButtonsForExecutingOrder()
        {
            StringBuilder buttons = new StringBuilder();
            buttons.Append(new HtmlActionButtonLink
                (
                    buttonLink: $"Performer/Solve/{Order.Id}",
                    buttonText: $"{GISigns.Briefcase} Загрузить решение",
                    buttonClass: BtnClass
                ).ToString());

            return buttons.ToString();
        }

        string GetButtonForFilesDownloading()
        {
            if(Order.HasCustomerFiles)
            {
                return new HtmlActionButtonLink
                         (
                             buttonLink: Order.GetLinkToDownloadCustomerFiles(),
                             buttonText: "<span class='glyphicon glyphicon-download-alt'> </span> Скачать файлы",
                             buttonClass: BtnClass
                         ).ToString();
            }

            return new HtmlActionButtonLink
                         (
                             buttonLink: "#",
                             buttonText: "Нет прикрепленных файлов",
                             buttonClass: BtnClass,
                             disabled: true
                         ).ToString();

        }

        //when order is new we can find some errors and tell customer about it
        //if no errors we should value this order
        string GetButtonsForNewOrder()
        {
            StringBuilder buttons = new StringBuilder();



            buttons.Append(new HtmlActionButtonLink
                         (
                             buttonLink: $"/Performer/AddError/{Order.Id}",
                             buttonText: "Указать на ошибку",
                             buttonClass: BtnClass
                         ).ToString())

            .Append(new HtmlActionButtonLink
                     (
                        buttonLink : $"/Performer/ValueOrder/{Order.Id}",
                        buttonText : "Оценить заказ",
                        buttonClass : BtnClass
                     ).ToString());


            return buttons.ToString();
        }

        //When order is valued we can just wait for customer payment
        string GetButtonForValuedOrder()
        {
            StringBuilder buttons = new StringBuilder();

            buttons.Append(new HtmlActionButtonLink
                        (
                            buttonLink: "#",
                            buttonText: $"Ожидаем оплаты",
                            buttonClass: BtnClass,
                            disabled: true
                        ).ToString());

            return buttons.ToString();
        }

        //when order is halfpaid performers can choose it
        string GetButtonsForHalfPaidOrder()
        {
            StringBuilder buttons = new StringBuilder();

            buttons.Append(new HtmlActionButtonLink
                        (
                            buttonLink: $"/Performer/Choose/{Order.Id}",
                            buttonText: $"Выбрать для исполнения",
                            buttonClass: BtnClass
                        ).ToString());

            return buttons.ToString();
        }

        string GetButtonsForErrorOrder()
        {
            StringBuilder buttons = new StringBuilder();
            buttons.Append(new HtmlActionButtonLink
                (
                    buttonLink: "#",
                    buttonText: "Ожидаем исправлений",
                    buttonClass: BtnClass,
                    disabled: true
                ));
            return buttons.ToString();
        }

        
        #endregion

        #region Overriden Methods

        public override string ToString()
        {
            string buttons = "";
            switch(OrderEntity.Order.Status)
            {
                case OrderStatus.New:
                    buttons = GetButtonsForNewOrder();
                    break;

                case OrderStatus.Estimated:
                    buttons = GetButtonForValuedOrder();
                    break;
                

                case OrderStatus.HalfPaid:
                    buttons = GetButtonsForHalfPaidOrder();
                    break;

                

                case OrderStatus.Executing:
                    buttons = GetButtonsForExecutingOrder();
                    break;

                case OrderStatus.HasError:
                    buttons = GetButtonsForErrorOrder();
                    break; 
            }
            return buttons + GetButtonForFilesDownloading()
                    +  GetButtonForDownloadingSolution() + GetButtonForChatAboutOrder();
        }
        #endregion
    }
}
