

using HtmlExtensions.HtmlEntities;
using Logic.Extensions.HtmlEmail;
using Runtasker.LocaleBuilders.Email.CallToAction;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Logic.Entities;
using Runtasker.Logic.HtmlConstantEmails.Models;
using Runtasker.Resources.Email.OrderCustomer;

namespace Runtasker.Logic.HtmlConstantEmails
{
    public class CustomerEmailConstants
    {
        #region Constructors
        public CustomerEmailConstants()
        {
            Construct();
        }

        void Construct()
        {
            ModelBuilder = new CustomerEmailCallToActionModelBuilder();
        }
        #endregion

        #region Constants
        public string Host
        {
            get { return "https://runtasker.ru"; }
        }

        #endregion

        #region Properties
        CustomerEmailCallToActionModelBuilder ModelBuilder { get; set; }
        #endregion

        public EmailModel GetForCustomerAddedNewOrder(string userName, Order order)
        {
            ForEmailCallToAction model = ModelBuilder.AddedOrder(userName, order.Id);

            string text = new HtmlEmailBase
                (
                    callToAction: new BigEmailCallToActionBase
                    (
                        header: model.Header,
                        littleHeader: null,
                        bigText: model.BigText,
                        link: null
                    )
                ).ToString();

            return new EmailModel
            {
                Subject = $"{CustEmailNotRes.AddedSubject1}{order.Id}",
                Body = text
            };
        }

        public EmailModel GetForCustomerPaidFirstHalfOfAnOrder(string userName, Order order)
        {
            ForEmailCallToAction model = ModelBuilder.PaidFirstHalf(userName, order.Id, order.FinishDate);

            string text =  new HtmlEmailBase
                (
                    callToAction: new BigEmailCallToActionBase
                    (
                        header: model.Header,
                        littleHeader: null,
                        bigText: model.BigText,
                        link: null
                    )
                ).ToString();

            return new EmailModel
            {
                Body = text,
                Subject = $"{CustEmailNotRes.FirstPaidSubject1}{order.Id}"
            };
        }

        public EmailModel GetForCustomerDownloadedAnOrderSolution(string userName, Order order)
        {
            ForEmailCallToAction model = ModelBuilder.DownloadedSolution(userName, order.Id);

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
                Body = text,
                Subject = CustEmailNotRes.DownloadedSubject1
            };
        }

        //Not done not implemented
        public string GetForInvitedCustomerFinishedAnOrder(string userName, string invitedEmail)
        {
            return new HtmlEmailBase
                (
                    callToAction: new BigEmailCallToActionBase
                    (
                        header: $"{CustEmailNotRes.Hello} {userName}!",
                        littleHeader: null,
                        bigText:
                        $"{CustEmailNotRes.Dear} {userName}! {CustEmailNotRes.User} {invitedEmail} {CustEmailNotRes.InvFinishedText1} "
                        + $"{CustEmailNotRes.InvFinishedText2} {CustEmailNotRes.RuntaskerWish}",
                        link: new HtmlLink
                        (
                            hrefParam: $"{Host}/Orders/Create",
                            textParam: CustEmailNotRes.InvFinishedBtnText
                        )
                    )
                ).ToString();
        }
    }
}
