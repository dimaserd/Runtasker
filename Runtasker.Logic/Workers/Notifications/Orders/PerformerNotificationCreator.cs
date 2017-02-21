using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models.VkNotificater;
using Runtasker.Logic.Contexts.Interfaces;

namespace Runtasker.Logic.Workers.Notifications.Orders
{
    /// <summary>
    /// Класс который создает локальные уведомления для исполнителей 
    /// о событиях происходящих на сайте
    /// </summary>
    public class PerformerNotificationCreator
    {
        #region Constructors
        public PerformerNotificationCreator(MyDbContext db)
        {
            Db = db;
            PerformersAndAdmins = GetPerformersAndAdmins();
            PerformersAndAdminsInfos = Db.OtherUserInfos.ToList();
        }
        #endregion

        #region Properties
        MyDbContext Db { get; set; }

        List<ApplicationUser> PerformersAndAdmins { get; set; }

        List<OtherUserInfo> PerformersAndAdminsInfos { get; set; }
        /// <summary>
        /// Свойство создано для того чтобы не проходить заново
        /// по пользователям в поиске тех кто должен знать о заказе
        /// так как это делается уже в самом начале при возвращении уведомлений
        /// </summary>
        List<ApplicationUser> WhoShouldKnowUsers { get; set; }

        List<OtherUserInfo> WhoShoulKnowUserInfos { get; set; }
        
        #endregion

        #region Public Methods
        public List<string> GetEmailsWhoShouldKnowAboutThisOrder(Order order)
        {
            return WhoShouldKnowUsers.Select(x => x.Email).ToList();
        }

        public List<OtherUserInfo> GetOtherUsersInfo()
        {
            List<OtherUserInfo> result = new List<OtherUserInfo>();

            foreach(OtherUserInfo info in PerformersAndAdminsInfos)
            {
                if (WhoShouldKnowUsers.Any(x => x.Id == info.Id))
                {
                    result.Add(info);
                }
            }

            return result;
        }

        /// <summary>
        /// Получает список
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VkUserInfo> GetVkUserInfos()
        {
            return WhoShouldKnowUsers.ToVkUserInfoList(PerformersAndAdminsInfos);
        }
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
            var a = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                Title = "Пользователь оплатил половину заказа",
                Text = $"Заказ №{order.Id} оплачен наполовину, приступайте немедленно и успейте к сроку {order.FinishDate}",
                Type = NotificationType.Success,
                UserGuid = order.PerformerGuid,
                Link = null
            };

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
        #endregion

        #region Help Methods
        List<ApplicationUser> GetPerformersAndAdmins()
        {
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(Db));


            List<string> selectedUserIds = (from role in roleManager.Roles
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
                OtherUserInfo userInfo = PerformersAndAdminsInfos
                    .FirstOrDefault(info => info.Id == x.Id);

                if( (userInfo != null) && (userInfo.Specialization.Contains(orderSubjectEnum)) )
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
