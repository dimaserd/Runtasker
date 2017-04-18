using System.Collections.Generic;
using System.Linq;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models.VkNotificater;
using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Models.ManageModels;

namespace Runtasker.Logic.Workers.Notifications.Orders
{
    /// <summary>
    /// Класс который создает локальные уведомления для исполнителей 
    /// о событиях происходящих на сайте
    /// </summary>
    public class PerformerNotificationCreator
    {
        #region Constructors
        public PerformerNotificationCreator(IMyDbContext db)
        {
            Db = db;
            PerformersAndAdmins = GetPerformersAndAdminsWithInfo();
        }
        #endregion

        #region Properties
        IMyDbContext Db { get; set; }

        List<ApplicationUser> PerformersAndAdmins { get; set; }

        /// <summary>
        /// Свойство создано для того чтобы не проходить заново
        /// по пользователям в поиске тех кто должен знать о заказе
        /// так как это делается уже в самом начале при возвращении уведомлений
        /// </summary>
        List<ApplicationUser> WhoShouldKnowUsers { get; set; }

        
        #endregion

        #region Public Methods
        public List<string> GetEmailsWhoShouldKnowAboutThisOrder(Order order)
        {
            return WhoShouldKnowUsers.Select(x => x.Email).ToList();
        }

        /// <summary>
        /// Получает список
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VkUserInfo> GetVkUserInfos()
        {
            return WhoShouldKnowUsers
                .ToVkUserInfoList
                (
                PerformersAndAdmins
                .Select(x => x.GetOtherInfo())
                );
        }


        #region Методы возвращающие уведомления
        /// <summary>
        /// Возвращает список уведомлений для пользователей 
        /// которым нужно узнать о созданном заказе
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<Notification> GetNotificationsListForJustCreatedOrder(Order order)
        {
            WhoShouldKnowUsers = GetUsersWhoShouldKnowAboutThisOrder(order);
            

            return WhoShouldKnowUsers.Select(x =>
            {
                return new Notification
                {
             
                    Type = NotificationType.Success,
                    Title = $"Пользователь добавил заказ №{order.Id}!",
                    Text = "Приступайте немедленно или сообщите заказчику об ошибках! "
                    + $"Заказ нужно завершить к {order.FinishDate.ToString("dd MMM yyyy")}",
                    UserGuid = x.Id,
                    AboutType = NotificationAboutType.Ordinary,
                    Link = null
            
                };
            }).ToList();
        }

        public List<Notification> GetNotificationsListForOrderDescriptionChanged(Order order)
        {
            WhoShouldKnowUsers = GetUsersWhoShouldKnowAboutThisOrder(order);


            return WhoShouldKnowUsers.Select(x =>
            {
                return new Notification
                {
                    AboutType = NotificationAboutType.Ordinary,
                    Text = $"В заказе №{order.Id} было изменено описание, проверьте",
                    Title = $"Пользователь изменил описание заказа по вашей просьбе! "
                + $"Проверьте и помните, что его нужно выполнить {order.FinishDate.ToString("d MMM yyyy")}",
                    Type = NotificationType.Info,
                    UserGuid = x.Id,
                    Link = null
                };
            }).ToList();
        }

        public List<Notification> GetNotificationsListForFilesAddedToOrder(Order order)
        {
            WhoShouldKnowUsers = GetUsersWhoShouldKnowAboutThisOrder(order);


            return WhoShouldKnowUsers.Select(x =>
            {
                return new Notification
                {
                    AboutType = NotificationAboutType.Ordinary,
                    Title = $"Заказчик добавил файлы к заказу №{order.Id}",
                    Text = $"Проверьте работу и приступайте к выполнению работы," +
                $"помните, что работа должна быть выполнена к сроку {order.FinishDate.ToString("d MMM yyyy")}",
                    Type = NotificationType.Info,
                    UserGuid = x.Id,
                    Link = null
                };
            }).ToList();
        }

        public List<Notification> GetNotificationsForHalfPaidOrder(Order order)
        {
            WhoShouldKnowUsers = GetUsersWhoShouldKnowAboutThisOrder(order);
            

            return WhoShouldKnowUsers.Select(x =>
            {
                return new Notification
                {
                    AboutType = NotificationAboutType.Ordinary,
                    Title = "Пользователь оплатил половину заказа",
                    Text = $"Заказ №{order.Id} оплачен наполовину, приступайте немедленно и успейте к сроку {order.FinishDate}",
                    Type = NotificationType.Success,
                    UserGuid = x.Id,
                    Link = null
                };
            }).ToList();
        }

        public List<Notification> GetNotificationsForOnlineHelpCreated(Order order)
        {
            WhoShouldKnowUsers = GetUsersWhoShouldKnowAboutThisOrder(order);


            return WhoShouldKnowUsers.Select(x =>
            {
                return new Notification
                {
                    AboutType = NotificationAboutType.Ordinary,
                    Title = $"Пользователь создал онлайн заявку по предмету {order.GetSubjectName()}",
                    Text = $"Заказ №{order.Id} является заявкой на онлайн помощь, проверьте заявку и сообщите пользователю возьмемся ли мы за нее или нет.",
                    Type = NotificationType.Success,
                    UserGuid = x.Id,
                    Link = null
                };
            }).ToList();
        }
        #endregion

        #endregion

        #region Help Methods
        List<ApplicationUser> GetPerformersAndAdminsWithInfo()
        {
            
            List<string> selectedUserIds = (from role in Db.Roles
                                  where role.Name == "Admin" || role.Name == "Performer"
                                  from user in role.Users
                                  select user.UserId).ToList();

            List<ApplicationUser> allUsers = Db.Users.ToList();

            return allUsers.Where(applicationUser => selectedUserIds.Contains(applicationUser.Id)).ToList();
            
        }

        /// <summary>
        /// Получает список пользователей которых нужно оповестить о заказе
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        List<ApplicationUser> GetUsersWhoShouldKnowAboutThisOrder(Order order)
        {
            string orderSubjectEnum = ((int)order.Subject).ToString();

            //если заказ не свободен то о нем должен знать только его исполнитель
            if(!order.IsFree())
            {
                return PerformersAndAdmins
                    .Where(x => x.Id == order.PerformerGuid).ToList();
            }

            return PerformersAndAdmins.Select(x =>
            {
                OtherUserInfo info = x.GetOtherInfo();
                if( (info != null) && (info.Specialization != null) && (info.Specialization.Contains(orderSubjectEnum)) )
                {
                    return x;
                }
                else
                {
                    return null;
                }
            }).Where(x => x != null).ToList();
        }


        #endregion
    }
}
