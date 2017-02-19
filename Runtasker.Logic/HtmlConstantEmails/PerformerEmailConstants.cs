using HtmlExtensions.HtmlEntities;
using HtmlExtensions.Renderers;
using Logic.Extensions.HtmlEmail;
using Runtasker.LocaleBuilders.Email.CallToAction;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Logic.Entities;
using Runtasker.Logic.HtmlConstantEmails.Models;
using Runtasker.Resources.Email.OrderPerformer;

namespace Runtasker.Logic.HtmlConstantEmails
{
    public class PerformerEmailConstants
    {
        #region Constructors
        public PerformerEmailConstants()
        {
            Construct();
        }

        void Construct()
        {
            ModelBuilder = new PerformerEmailCallToActionModelBuilder();

            HtmlSigns = new HtmlSignsRenderer();
        }
        #endregion

        #region Constants
        public string Host
        {
            get { return "https://runtasker.ru"; }
        }

        #endregion

        #region Properties
        PerformerEmailCallToActionModelBuilder ModelBuilder { get; set;}

        HtmlSignsRenderer HtmlSigns { get; set; }
        #endregion

        #region Public Methods
        public EmailModel GetForPerformerValuedAnOrder(string customerName, Order order)
        {
            ForEmailCallToAction model = ModelBuilder.EstimatedOrder(customerName, order.Id, order.Sum, HtmlSigns.Rouble);

            string text = new HtmlEmailBase
                (
                    callToAction: new BigEmailCallToActionBase
                    (
                        header: model.Header,
                        littleHeader: null,
                        bigText: model.BigText,                 
                        link: new HtmlLink
                        (
                            hrefParam: $"{Host}/Orders/PayHalf/{order.Id}",
                            textParam : model.ActionBtnText
                        )
                    )
                ).ToString();

            return new EmailModel
            {
                Subject = $"{PerformerOrderEmRes.EstimatedSubject1}{order.Id} {PerformerOrderEmRes.EstimatedSubject2}",
                Body = text
            };
        }

        public EmailModel GetForPerformerFoundAnErrorInOrder(string customerName, Order order)
        {
            ForEmailCallToAction model = ModelBuilder.FoundError(customerName, order.Id);

            string text = new HtmlEmailBase
                (
                    callToAction: new BigEmailCallToActionBase
                    (
                        header: model.Header,
                        littleHeader: null,
                        bigText: model.BigText,
                        link: new HtmlLink
                        (
                            hrefParam: $"{Host}/Orders",
                            textParam: model.ActionBtnText
                        )
                    )
                ).ToString();

            return new EmailModel
            {
                Subject = $"{PerformerOrderEmRes.ErrorFoundSubject1}{order.Id}",
                Body = text
            };
        }

        public EmailModel GetForPerformerExecutedAnOrder(string customerName, Order order)
        {
            decimal leftSum = order.Sum - order.PaidSum;

            ForEmailCallToAction model = ModelBuilder.ExecutedOrder(customerName, order.Id, leftSum, HtmlSigns.Rouble);

            string text =  new HtmlEmailBase
                (
                    callToAction: new BigEmailCallToActionBase
                    (
                        header: model.Header,
                        littleHeader: null,
                        bigText: model.BigText,
                        link: new HtmlLink
                        (
                            hrefParam: $"{Host}/Orders/PayAnotherHalf/{order.Id}",
                            textParam: model.ActionBtnText
                        )
                    )
                ).ToString();

            return new EmailModel
            {
                Subject = $"{PerformerOrderEmRes.ExecutedSubject1}{order.Id}",
                Body = text
            };
        }
        #endregion
    }
}
