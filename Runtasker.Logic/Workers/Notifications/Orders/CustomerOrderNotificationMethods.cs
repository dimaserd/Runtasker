using HtmlExtensions.HtmlEntities;
using HtmlExtensions.Renderers;
using Runtasker.LocaleBuilders.Models;
using Runtasker.LocaleBuilders.Notification;
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Enumerations.OrderWorker;
using Runtasker.Logic.Models.VkNotificater;
using Runtasker.Logic.Workers.Email;
using Runtasker.Logic.Workers.Notifications.Orders;
using Runtasker.Logic.Workers.Notifications.Orders.VkNotifications;
using Runtasker.Resources.Notifications.CustomerOrderMethods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Runtasker.Logic.Workers.Notifications
{
    //We can left the liasons, but we can avoid using it
    public class CustomerOrderNotificationMethods
    {
        #region Constructors
        public CustomerOrderNotificationMethods(IMyDbContext context)
        {
            Context = context;
            Construct();
            
        }

        void Construct()
        {
            Emailer = new CustomerEmailMethods();
            FASigns = new FontAwesomeRenderer();
            GISigns = new GlyphiconRenderer();

            ModelBuilder = new CustomerOrderNotificationBuilder();
            PerformerNotificationGetter = new PerformerNotificationCreator(Context);
        }
        #endregion

        #region Properties
        PerformerNotificationCreator PerformerNotificationGetter { get; set; }

        CustomerEmailMethods Emailer { get; set; }

        FontAwesomeRenderer FASigns { get; set; }
        GlyphiconRenderer GISigns { get; set; }

        CustomerOrderNotificationBuilder ModelBuilder { get; set; }

        IMyDbContext Context { get; set; }
        #endregion

        #region Public Methods

        #region Когда заказ еще не выбран исполнителем

        #region Adding Order Methods
        public void OnCustomerAddedOrder(Order order, OrderCreationType creationType, string callBackUrl = null)
        {
            ForNotification model = ModelBuilder.AddedOrder(order.Id);

            Notification customerN = new Notification
            { 
                Type = NotificationType.Info,
                Title = model.Title,
                Text = model.Text,
                UserGuid = order.UserGuid,
                Link = null,
                AboutType = NotificationAboutType.Ordinary
            };


            //получаем уведомления для исолнителей и администраторов
            //и добавляем их в базу
            List<Notification> performersNotifications = PerformerNotificationGetter.GetNotificationsListForJustCreatedOrder(order);
            Context.Notifications.AddRange(performersNotifications);

            Context.Notifications.Add(customerN);
            Context.SaveChanges();

            //получаем список тех кого нужно оповестить
            IEnumerable<VkUserInfo> vkUserInfos = PerformerNotificationGetter.GetVkUserInfos();
            //методы рассылки исполнителям в вк
            using (VkPerformerNotificater vkNotificater = new VkPerformerNotificater(vkUserInfos))
            {
                vkNotificater.OnCustomerAddedOrder(order);
            }


            //методы рассылки почты должны вызываться по разному для двух видов заказа
            if (creationType == OrderCreationType.ToKnowPrice)
            {
                //посылается письмо для подтверждения адреса электронной почты 
                using (AccountEmailMethods accountEmailer = new AccountEmailMethods())
                {
                    accountEmailer.OnUserRegistered(GetCustomer(order), callBackUrl);
                }

            }
            else if (creationType == OrderCreationType.Ordinary)
            {
                //сообщение о добавленном заказе у них будет одинаковым
                Emailer.OnCustomerAddedOrder(order, GetCustomer(order), GetAdminsEmails(order));
            }

        }

        
        #endregion

        /// <summary>
        /// Еще не реализовано
        /// </summary>
        /// <param name="order"></param>
        public void OnCustomerDeletedOrder(Order order)
        {
            Notification N = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Type = NotificationType.Warning,
                Title = $"Order №{order.Id} was deleted!",
                Text = new HtmlLink("/Orders/Create", "You can make another one here").ToString(),
                UserGuid = order.UserGuid,
                Status = NotificationStatus.Unseen,
                AboutType = NotificationAboutType.Ordinary
            };
            
            Context.Notifications.Add(N);
            Context.SaveChanges();
            
        }

        public void OnCustomerAddedNewDescription(Order order)
        {
            ForNotification model = ModelBuilder.AddedDescription(order.Id);

            Notification customerN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                Title = model.Title,
                Text = model.Text,
                Type = NotificationType.Info,
                UserGuid = order.UserGuid,
                Link = null
            };

            
            Context.Notifications.Add(customerN);
            
            List<Notification> performersNotifications = PerformerNotificationGetter
                .GetNotificationsListForOrderDescriptionChanged(order);
            Context.Notifications.AddRange(performersNotifications);


            //получаем список тех кого нужно оповестить
            IEnumerable<VkUserInfo> vkUserInfos = PerformerNotificationGetter.GetVkUserInfos();
            //методы рассылки исполнителям в вк
            using (VkPerformerNotificater vkNotificater = new VkPerformerNotificater(vkUserInfos))
            {
                vkNotificater.OnCustomerChangedOrderDescription(order);
            }

            Emailer.OnCustomerChangedDescription(order, GetAdminEmail(order));
        }

        public void OnCustomerAddedNewFilesToOrder(Order order)
        {
            ForNotification model = ModelBuilder.AddedNewFiles(order.Id);

            Notification customerN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                Title = model.Title,
                Text = model.Text,
                Type = NotificationType.Info,
                UserGuid = order.UserGuid,
                Link = null
            };
            
            
            Context.Notifications.Add(customerN);
            
            

            List<Notification> performersNotifications = PerformerNotificationGetter
                .GetNotificationsListForFilesAddedToOrder(order);
            Context.Notifications.AddRange(performersNotifications);

            Context.SaveChanges();

            //получаем список тех кого нужно оповестить
            IEnumerable<VkUserInfo> vkUserInfos = PerformerNotificationGetter.GetVkUserInfos();
            //методы рассылки исполнителям в вк
            using (VkPerformerNotificater vkNotificater = new VkPerformerNotificater(vkUserInfos))
            {
                vkNotificater.OnCustomerAddedFiles(order);
            }

            Emailer.OnCustomerAddedNewFilesToOrder(order, GetAdminEmail(order));

        }

        public void OnCustomerPaidHalfOfAnOrder(Order order)
        {
            ForNotification model = ModelBuilder.PaidFirstHalf(order.Id, order.FinishDate);

            Notification customerN = new Notification
            {
                AboutType = NotificationAboutType.Balance,
                Title = model.Title,
                Text = model.Text,
                Type = NotificationType.Info,
                UserGuid = order.UserGuid,
                Link = null
            };

            List<Notification> performersN = PerformerNotificationGetter
                .GetNotificationsForHalfPaidOrder(order);

            Context.Notifications.AddRange(performersN);
            Context.Notifications.Add(customerN);
            Context.SaveChanges();

            //получаем список тех кого нужно оповестить
            IEnumerable<VkUserInfo> vkUserInfos = PerformerNotificationGetter.GetVkUserInfos();
            //методы рассылки исполнителям в вк
            using (VkPerformerNotificater vkNotificater = new VkPerformerNotificater(vkUserInfos))
            {
                vkNotificater.OnCustomerPaidFirstHalf(order);
            }

            //Email notifications switched off
            //Emailer.OnCustomerPaidAHalfOfAnOrder(order, GetCustomer(order), GetPerformerEmail());
        }

        #endregion


        public void OnCustomerPaidAnotherHalfOfAnOrder(Order order)
        {
            ForNotification model = ModelBuilder.PaidSecondHalf(order.Id, GISigns.Save);

            Notification customerN = new Notification
            {
                AboutType = NotificationAboutType.Balance,
                Title = model.Title,
                Text = model.Text,
                Type = NotificationType.Success,
                UserGuid = order.UserGuid,
                Link = new HtmlLink
                (
                    hrefParam: $"/Orders/DownloadSolution/{order.Id}",
                    textParam: model.ActionBtnText,
                    buttonSizeParam: HtmlButtonSize.Large,
                    buttonTypeParam: HtmlButtonType.Success
                ).ToString()
            };
            
            Context.Notifications.Add(customerN);
            Context.SaveChanges();
            
        }

        public void OnCustomerRatedAnOrderSolution(Order order)
        {
            ForNotification model = ModelBuilder.RatedSolution(order.Rating);

            Notification cutomerN = new Notification()
            {
                AboutType = NotificationAboutType.Ordinary,
                Type = NotificationType.Success,
                UserGuid = order.UserGuid,
                Title = model.Title,
                Text = model.Text,
                Link = null
            };

            Notification performerN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                Type = NotificationType.Success,
                UserGuid = order.PerformerGuid,
                Title = $"Пользователь оценил наши услуги по заказу №{order.Id}!",
                Text = $"Его оценка ({order.Rating} из 5) Оцените его комментарии, если они жалобные, " +
                       "то предложите пользователю какой-либо бонус!",
                Link = null,
            };

            Context.Notifications.Add(cutomerN);
            Context.Notifications.Add(performerN);
            Context.SaveChanges();
            
            Emailer.OnCustomerRatedAnOrderSolution(order, GetPerformerEmail());
        }

        //Not done not liasoned
        public void OnInvitedCustomerRatedAnOrderSolution(Invitation I)
        {
            string inviterEmail = GetEmailByGuid(I.SenderGuid);

            Notification inviterN = new Notification
            {
                AboutType = NotificationAboutType.Balance,
                Type = NotificationType.Success,
                UserGuid = I.SenderGuid,
                Title = $"{CustomerOrder.InviterRatingTitle1} {I.ReceiverEmail} " +
                CustomerOrder.InviterRatingTitle2,
                Text = $"{CustomerOrder.InviterRatingText1} 300{FASigns.Rouble}! "
                + CustomerOrder.InviterRatingText2,
                Link = new HtmlLink
                (
                    hrefParam: "/Manage/InviteUser",
                    textParam: $"{GISigns.User} {CustomerOrder.InviterRatingButtonText}",
                    buttonSizeParam: HtmlButtonSize.Large,
                    buttonTypeParam: HtmlButtonType.Success
                ).ToString(),
            };

            Notification invitedN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                Type = NotificationType.Info,
                UserGuid = I.ReceiverGuid,
                Title = $"{CustomerOrder.InvitedRatingTitle1} {inviterEmail} " + 
                $"{CustomerOrder.InvitedRatingTitle2} 300{FASigns.Rouble}!",
                Text = $"{CustomerOrder.InvitedRatingText1} {inviterEmail} {CustomerOrder.InvitedRatingText2} 300{FASigns.Rouble} "
                + CustomerOrder.InvitedRatingText3,
                Link = new HtmlLink
                (
                    hrefParam: "/Manage/InviteUser",
                    textParam: $"{GISigns.User} {CustomerOrder.InviterRatingButtonText}",
                    buttonSizeParam: HtmlButtonSize.Large,
                    buttonTypeParam: HtmlButtonType.Success
                ).ToString()
            };
            Context.Notifications.Add(inviterN);
            Context.Notifications.Add(invitedN);
            Context.SaveChanges();
            
            Emailer.OnInvitedCustomerRatedAnOrderSolution(GetUserByGuid(I.SenderGuid), I.ReceiverEmail);
        }


        #endregion

        #region Help Methods
        //for now there is only one performer, me
        #region Email Getters

        /// <summary>
        /// Возвращает список эмейлов тех кто должен знать о заказе
        /// тех кому будет отослана почта
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        List<string> GetAdminsEmails(Order order)
        {
            return PerformerNotificationGetter.GetEmailsWhoShouldKnowAboutThisOrder(order);
        }

        string GetEmailByGuid(string guid)
        {
            return Context.Users.FirstOrDefault(u => u.Id == guid).Email;
        }

        string GetPerformerEmail()
        {
            return "dimaserd84@gmail.com";
        }

        string GetPerformerEmail(Order order)
        {
            return Context.Users.FirstOrDefault(u => u.Id == order.PerformerGuid).Email;
        }

        string GetAdminEmail(Order order)
        {
            string guid = GetAdminGuid();

            return Context.Users.FirstOrDefault(u => u.Id == guid).Email;
        }

        string GetCustomerEmail(Order order)
        {
            return Context.Users.FirstOrDefault(u => u.Id == order.UserGuid).Email;
        }
        #endregion

        ApplicationUser GetUserByGuid(string guid)
        {
            return Context.Users.Find(guid);
        }

        ApplicationUser GetCustomer(Order order)
        {
            return Context.Users.Find(order.UserGuid);
        }

        #region Guid Getters
        string GetAdminGuid(Order order)
        {
            string email = "dimaserd84@gmail.com";
            return Context.Users.FirstOrDefault(u => u.Email == email).Id;
        }

        string GetAdminGuid()
        {
            string email = "dimaserd84@gmail.com";
            return Context.Users.FirstOrDefault(u => u.Email == email).Id;
        }

        string GetPerformerGuid(Order order)
        {
            string email = GetPerformerEmail(order);
            return Context.Users.FirstOrDefault(u => u.Email == email).Id;
            
        }

        #endregion

        #endregion
    }
}
