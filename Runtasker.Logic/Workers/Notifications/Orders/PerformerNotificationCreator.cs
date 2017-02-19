using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Runtasker.Logic.Entities;

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

        
        #endregion

        #region Public Methods
        public List<string> GetEmailsWhoShouldKnowAboutThisOrder(Order order)
        {
            return WhoShouldKnowUsers.Select(x => x.Email).ToList();
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

            return PerformersAndAdmins.Select(x =>
            {
                OtherUserInfo userInfo = PerformersAndAdminsInfos
                    .FirstOrDefault(info => info.UserId == x.Id);

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
