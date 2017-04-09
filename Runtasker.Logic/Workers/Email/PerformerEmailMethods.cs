
using Microsoft.AspNet.Identity;
using Runtasker.Logic.Entities;
using Runtasker.Logic.HtmlConstantEmails;
using Runtasker.Logic.HtmlConstantEmails.Models;

namespace Runtasker.Logic.Workers.Email
{

    /// <summary>
    /// Класс который посылает электронные письма уведомляя пользователей
    /// о каких либо событиях на сервисе
    /// </summary>
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

        #region Методы как события

        #region Методы оценки
        public void OnPerformerEstimatedAnOrder(Order order, ApplicationUser customer)
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

        public void OnAdminEstimatedOnlineHelp(Order order, ApplicationUser customer)
        {
            EmailModel model = HtmlConstants.GetForPerformerEstimatedOnlineHelp(customer.Name, order);

            IdentityMessage m = new IdentityMessage
            {
                Subject = model.Subject,
                Body = model.Body,
                Destination = customer.Email
            };
            SendEmail(m);

        }
        #endregion

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
