
using Microsoft.AspNet.Identity;
using Runtasker.Logic.Entities;
using Runtasker.Logic.HtmlConstantEmails;
using Runtasker.Logic.HtmlConstantEmails.Models;
using Runtasker.Resources.Email.OrderCustomer;
using System.Collections.Generic;

namespace Runtasker.Logic.Workers.Email
{
    public class CustomerEmailMethods : EmailWorkerBase
    {
        #region Constructors
        public CustomerEmailMethods() : base()
        {
            Construct();
        }

        void Construct()
        {
            HtmlConstants = new CustomerEmailConstants();
        }
        #endregion

        #region Properties
        CustomerEmailConstants HtmlConstants { get; set; }
        #endregion

        #region Methods like events

        public void OnCustomerAddedOrder(Order order, ApplicationUser customer, string performerEmail)
        {
            EmailModel model = HtmlConstants.GetForCustomerAddedNewOrder(customer.Name, order);
            IdentityMessage customerM = new IdentityMessage
            {
                Body = model.Body,
                Destination = customer.Email,
                Subject = model.Subject
            };

            IdentityMessage performerM = new IdentityMessage
            {
                Subject = $"У нас новый заказ №{order.Id}",
                Body = $"<p>Пользователь {customer.Email} добавил заказ по предмету {order.Subject.ToDescriptionString()}</p>" +
                "<p>Проверьте заказ и установите цену, за которую мы его выполним!</p>",
                Destination = performerEmail
            };


            SendEmail(customerM);
            SendEmail(performerM);
        }

        public void OnCustomerAddedOrder(Order order, ApplicationUser customer, List<string> adminEmails)
        {
            EmailModel model = HtmlConstants.GetForCustomerAddedNewOrder(customer.Name, order);

            List<IdentityMessage> emailMessages = new List<IdentityMessage>();

            IdentityMessage customerM = new IdentityMessage
            {
                Body = model.Body,
                Destination = customer.Email,
                Subject = model.Subject
            };

            emailMessages.Add(customerM);

            //формируем сообщения для администраторов сервиса
            //и добавляем их в общий список
            foreach(string adminEmail in adminEmails)
            {
                emailMessages.Add(new IdentityMessage
                {
                    Subject = $"У нас новый заказ №{order.Id}",
                    Body = $"<p>Пользователь {customer.Email} добавил заказ по предмету {order.Subject.ToDescriptionString()}</p>" +
                "<p>Проверьте заказ и установите цену, за которую мы его выполним!</p>",
                    Destination = adminEmail
                });
            }
            
            //посылаем все сообщения
            SendEmails(emailMessages);
            
        }


        public void OnCustomerAddedNewFilesToOrder(Order order, string performerEmail)
        {
            IdentityMessage m = new IdentityMessage
            {
                Subject = $"Пользователь добавил файлы в заказ №{order.Id}",
                Body = $"<p>В заказе №{order.Id} появились новые файлы</p>" +
                "<p>Проверьте, достаточно ли у нас информации, чтобы выполнить заказ.</p>" +
                $"<p>Оцените его и помните, что заказ нужно сдать к {order.FinishDate.ToString("d MMM yyyyy")}</p>",
                Destination = performerEmail
                
            };
            SendEmail(m);
        }

        public void OnCustomerChangedDescription(Order order, string performerEmail)
        {
            IdentityMessage m = new IdentityMessage
            {
                Body = "<p>Проверьте изменения и немедленно уведомите пользователя о результате!</p>",
                Destination = performerEmail,
                Subject = "Заказчик изменил описание"
            };
            SendEmail(m);
        }

        public void OnCustomerPaidAHalfOfAnOrder(Order order, ApplicationUser customer, string performerEmail)
        {
            EmailModel model = HtmlConstants.GetForCustomerPaidFirstHalfOfAnOrder(customer.Name, order);

            IdentityMessage customerM = new IdentityMessage
            {
                Subject = model.Subject,
                Body = model.Body,
                Destination = customer.Email
            };

            IdentityMessage performerM = new IdentityMessage
            {
                Subject = $"Заказчик оплатил половину заказа №{order.Id}",
                Body = $"Приступайте немедленно и успейте к сроку {order.FinishDate.ToString("d MMM yyyy")}",
                Destination = performerEmail
            };

            SendEmail(customerM);
            SendEmail(performerM);
        }

        public void OnCustomerPaidAnotherHalfOfAnOrder(Order order)
        {

        }

        public void OnCustomerDownloadedAnOrderSolution(ApplicationUser customer, Order order)
        {
            EmailModel model = HtmlConstants.GetForCustomerDownloadedAnOrderSolution(customer.Name, order);

            IdentityMessage m = new IdentityMessage
            {
                Subject = model.Subject,
                Body = model.Body,
                Destination = customer.Email
            };
            SendEmail(m);
        }

        public void OnCustomerRatedAnOrderSolution(Order order, string performerEmail)
        {
            IdentityMessage m = new IdentityMessage
            {
                Subject = "Пользователь оценил решение!",
                Body = $"<p>Решение по заказу №{order.Id} было оценено на ({order.Rating} из 5)</p>"
                + "<p>Оцените его комментарии, если они жалобные, " +
                       "то предложите пользователю какой-либо бонус!</p>",
                Destination = performerEmail
            };
            SendEmail(m);
        }

        public void OnInvitedCustomerRatedAnOrderSolution(ApplicationUser inviter, string invitedEmail)
        {
            IdentityMessage m = new IdentityMessage
            {
                Subject = CustEmailNotRes.InvFinishedSubject1,
                Body = HtmlConstants.GetForInvitedCustomerFinishedAnOrder(inviter.Name, invitedEmail),
                Destination = inviter.Email
            };
            SendEmail(m);
        }
        #endregion
    }
}
