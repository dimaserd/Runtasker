
using Microsoft.AspNet.Identity;
using Runtasker.Logic.Entities;
using Runtasker.Logic.HtmlConstantEmails;
using Runtasker.Logic.HtmlConstantEmails.Models;
using Runtasker.Resources.Email.OrderPerformer;

namespace Runtasker.Logic.Workers.Email
{
    //Class that sends emails when performer do something
    //that someone else should know about
    public class PerformerEmailMethods : EmailWorkerBase
    {
        #region Constructors

        public PerformerEmailMethods() : base ()
        {
            Construct();
        }

        void Construct()
        {
            HtmlConstants = new PerformerEmailConstants();
        }
        #endregion

        #region Properties
        PerformerEmailConstants HtmlConstants { get; set; }
        #endregion

        #region Methods like Events

        public void OnPerformerValuedAnOrder(Order order, ApplicationUser customer)
        {
            EmailModel model = HtmlConstants.GetForPerformerValuedAnOrder(customer.Name, order);

            IdentityMessage m = new IdentityMessage
            {
                Subject = model.Subject,
                Body = model.Body,              
                Destination = customer.Email
            };
            SendEmail(m);
            
        }

        public void OnPerformerAddedErrorToOrder(Order order, ApplicationUser customer)
        {
            EmailModel model = HtmlConstants.GetForPerformerFoundAnErrorInOrder(customer.Name, order);

            IdentityMessage m = new IdentityMessage
            {
                Subject = model.Subject,
                Body = model.Body,
                Destination = customer.Email
                
            };
            SendEmail(m);
        }

        public void OnPerformerExecutedAnOrder(Order order, ApplicationUser customer, string performerEmail)
        {
            decimal leftSum = order.Sum - order.PaidSum;

            EmailModel model = HtmlConstants.GetForPerformerExecutedAnOrder(customer.Name, order);

            IdentityMessage customerM = new IdentityMessage
            {
                Subject = model.Subject,
                Body = model.Body,
                Destination = customer.Email
            };

            IdentityMessage performerM = new IdentityMessage
            {
                Subject = $"Завершена работа по заказу №{order.Id}",
                Body = $"<p>Вы загрузили решение заказа №{order.Id}</p>"
                + "<p>Мы уведомим вас когда пользователь оплатит заказ и скачает его!</p>",
                Destination = performerEmail
            };

            SendEmail(customerM);
            SendEmail(performerM);


        }
        #endregion

    }
}
