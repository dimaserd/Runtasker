﻿using HtmlExtensions.Renderers;
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
            FAIcons = new FontAwesomeRenderer();
            GIcons = new GlyphiconRenderer();
        }
        #endregion

        #region Properties
        FontAwesomeRenderer FAIcons { get; set; }

        GlyphiconRenderer GIcons { get; set; }

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
                    buttonText: $"{FAIcons.Download} Скачать решение",
                    buttonClass: GetButtonClass()
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
                    buttonText: $"Чат по заказу {GIcons.Count(unreadCount)}",
                    //for javascript toggler with modal
                    htmlAttributes: new { id = Order.Id, @class = $"{GetButtonClass()} orderChat"  }
                ).ToString();
        }

        string GetButtonsForExecutingOrder()
        {
            StringBuilder buttons = new StringBuilder();
            buttons.Append(new HtmlActionButtonLink
                (
                    buttonLink: $"Performer/Solve/{Order.Id}",
                    buttonText: $"{GIcons.Briefcase} Загрузить решение",
                    buttonClass: GetButtonClass()
                ).ToString());

            return buttons.ToString();
        }

        string GetButtonForFilesDownloading()
        {
            if(!string.IsNullOrEmpty(Order.Attachments))
            {
                return new HtmlActionButtonLink
                         (
                             buttonLink: Order.Attachments,
                             buttonText: "<span class='glyphicon glyphicon-download-alt'> </span> Скачать файлы",
                             buttonClass: GetButtonClass()
                         ).ToString();
            }

            return new HtmlActionButtonLink
                         (
                             buttonLink: Order.Attachments,
                             buttonText: "Нет прикрепленных файлов",
                             buttonClass: GetButtonClass(),
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
                             buttonClass: GetButtonClass()
                         ).ToString())

            .Append(new HtmlActionButtonLink
                     (
                        buttonLink : $"/Performer/ValueOrder/{Order.Id}",
                        buttonText : "Оценить заказ",
                        buttonClass : GetButtonClass()
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
                            buttonClass: GetButtonClass(),
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
                            buttonClass: GetButtonClass()
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
                    buttonClass: GetButtonClass(),
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

                case OrderStatus.Valued:
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
