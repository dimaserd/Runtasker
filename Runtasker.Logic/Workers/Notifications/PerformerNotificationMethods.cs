using HtmlExtensions.HtmlEntities;
using HtmlExtensions.StaticRenderers;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Email;
using System.Linq;
using Runtasker.Resources.Notifications.PerformerOrderMethods;
using System.Globalization;
using System.Threading;
using Runtasker.LocaleBuilders.Notification;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Logic.Enumerations;
using Runtasker.Settings;
using Extensions.Enumerations;

namespace Runtasker.Logic.Workers.Notifications
{
    public class PerformerOrderNotificationMethods
    {
        #region Constructors
        public PerformerOrderNotificationMethods(MyDbContext context, string userGuid)
        {
            Construct(Context, null, userGuid);
        }

        void Construct(MyDbContext context = null, PerformerEmailMethods emailer = null, string userGuid = null)
        {
            Context = context ?? new MyDbContext();
            Emailer = emailer ?? new PerformerEmailMethods();
            PerformerGuid = userGuid;

            ModelBuilder = new PerformerOrderNotificationBuilder();
        }
        #endregion

        #region Properties
        MyDbContext Context { get; set; }
        string PerformerGuid { get; set; }
        PerformerEmailMethods Emailer { get; set; }

        PerformerOrderNotificationBuilder ModelBuilder { get; set; }
        #endregion

        #region Public Methods

        //Started not completed
        public void OnPerformerAddedErrorToOrder(Order order)
        {
            SetCustomerCulture(order);

            ForNotification model = ModelBuilder.AddedError(order.Id, order.ErrorType.ToDisplayName());
            Notification customerN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                UserGuid = order.UserGuid,
                Type = NotificationType.Danger,
                Title = model.Title,
                Text = model.Text,
                Link = new HtmlLink
                (
                    hrefParam: (order.ErrorType == OrderErrorType.NeedFiles) ? 
                    $"/Orders/AddFiles/{order.Id}" : $"/Orders/AddDescription/{order.Id}",
                    textParam: (order.ErrorType == OrderErrorType.NeedFiles) ? 
                    PerformerOrderNotRes.AddFilesBtnText : PerformerOrderNotRes.AddDescBtnText,
                    buttonSizeParam: HtmlButtonSize.Large,
                    buttonTypeParam: HtmlButtonType.Warning
                ).ToString()
            };

            Notification performerN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                UserGuid = PerformerGuid,
                Type = NotificationType.Info,
                Link = null,
                Title = $"Вы обнаружили ошибку в заказе №{order.Id}",
                Text = $"Ожидайте действий пользователя - ошибка : {order.ErrorType.ToDisplayName()}!"
            };

            Context.Notifications.Add(customerN);
            Context.Notifications.Add(performerN);
            Context.SaveChanges();

            string customerEmail = GetCustomerEmail(order);
            Emailer.OnPerformerAddedErrorToOrder(order, GetCustomer(order));
        }

        #region Оценивание заказа
        public void OnAdminEstimatedAnOrder(Order order, SaveChangesType saveType = SaveChangesType.Now)
        {
            //устанавливаем культуру пользователя чтобы уведомления создавались на его языке
            SetCustomerCulture(order);

            ForNotification model = ModelBuilder.Estimated(order.Id, order.Sum, HtmlSigns.Rouble.ToString());
            Notification customerN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                UserGuid = order.UserGuid,
                Type = NotificationType.Info,
                Text = model.Text,
                Title = model.Title,
                Link = new HtmlLink
                (
                    hrefParam: $"/Orders/PayHalf/{order.Id}",
                    textParam : model.ActionBtnText,
                    buttonSizeParam : HtmlButtonSize.Large,
                    buttonTypeParam : HtmlButtonType.Success
                ).ToString()
            };
            
            Notification performerN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                UserGuid = PerformerGuid,
                Text = $"Вы оценили заказ №{order.Id} на сумму {order.Sum} ждем оплаты половины суммы и приступаем!",
                Title = "Заказ оценен.",
                Type = NotificationType.Success,
                Link = null
            };

            //добавляю уведомления в базу
            Context.Notifications.Add(customerN);
            Context.Notifications.Add(performerN);

            if(saveType == SaveChangesType.Now)
            {
                //сохраняю изменения в базе
                Context.SaveChanges();
            }
           


            //вызываем методы рассылки электронной почты
            string customerEmail = GetCustomerEmail(order);
            Emailer.OnPerformerEstimatedAnOrder(order, GetCustomer(order));
        }
        
        /// <summary>
        /// Данный метод должен отличаться от метода который выше так как 
        /// для онлайн помощи немного другие правила
        /// </summary>
        /// <param name="order"></param>
        /// <param name="saveType"></param>
        public void OnPerformerEstimatedOnlineHelp(Order order, SaveChangesType saveType = SaveChangesType.Now)
        {
            if(order.WorkType != OrderWorkType.OnlineHelp)
            {
                throw new System.Exception("Заказ не является онлайн помощью");
            }

            //устанавливаем культуру пользователя чтобы уведомления создавались на его языке
            SetCustomerCulture(order);

            //получаю модель для уведомления для заказчика
            ForNotification model = ModelBuilder.OnlineHelpEstimated(order.Id, order.FinishDate, order.Subject.ToDisplayName(), order.Sum, HtmlSigns.Rouble);

            //создаю уведомление для заказчика
            Notification customerN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                UserGuid = order.UserGuid,
                Type = NotificationType.Info,
                Text = model.Text,
                Title = model.Title,
                Link = new HtmlLink
                (
                    hrefParam: $"/Orders/PayOnlineHelp/{order.Id}",
                    textParam: model.ActionBtnText,
                    buttonSizeParam: HtmlButtonSize.Large,
                    buttonTypeParam: HtmlButtonType.Success
                ).ToString()
            };

            //создаю уведомление для исполнителя или администратора
            Notification adminN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                UserGuid = PerformerGuid,
                Text = $"Вы оценили онлайн-помощь №{order.Id} на сумму {order.Sum} ждем полной оплаты...",
                Title = "Онлайн-помощь оценена.",
                Type = NotificationType.Success,
                Link = null
            };

            //добавляю уведомления в базу
            Context.Notifications.Add(adminN);
            Context.Notifications.Add(customerN);

            if(saveType == SaveChangesType.Now)
            {
                //сохраняю изменения в базе данных
                Context.SaveChanges();
            }


            //вызываем методы рассылки электронной почты
            string customerEmail = GetCustomerEmail(order);
            Emailer.OnAdminEstimatedOnlineHelp(order, GetCustomer(order));
        }

        #endregion
        public void OnPerformerStartedExecutingAnOrder(Order order)
        {
            SetCustomerCulture(order);

            ForNotification model = ModelBuilder.StartedExecuting(order.Id, order.FinishDate);

            Notification custormerN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                UserGuid = order.UserGuid,
                Title =  model.Title,
                Text = model.Text,
                Type = NotificationType.Info,
                Link = null

            };

            Notification performerN = new Notification
            {
                AboutType = NotificationAboutType.Ordinary,
                UserGuid = order.PerformerGuid,
                Title = $"Вы приступили к выполнению заказа №{order.Id}",
                Text = $"Выполните его к сроку {order.FinishDate.ToString("d MMM yyyy")}",
                Link = null,
                Type = NotificationType.Info
            };

            
            Context.Notifications.Add(custormerN);
            Context.Notifications.Add(performerN);
            Context.SaveChanges();

            //Email methods
        }

        public void OnPerformerExecutedAnOrder(Order order, SaveChangesType saveType = SaveChangesType.Now)
        {
            SetCustomerCulture(order);
            decimal leftSum = order.Sum - order.PaidSum;

            ForNotification model = ModelBuilder.Executed(order.Id, leftSum, HtmlSigns.Rouble);

            Notification customerN = new Notification
            {
                Title = model.Title,
                Text = model.Text,
                Type = NotificationType.Success,
                UserGuid = order.UserGuid,
                Link = new HtmlLink
                (
                    hrefParam: $"/Orders/PayAnotherHalf/{order.Id}",
                    textParam: model.ActionBtnText,
                    buttonSizeParam: HtmlButtonSize.Large,
                    buttonTypeParam: HtmlButtonType.Success
                ).ToString()
            };
            Context.Notifications.Add(customerN);

            if(saveType == SaveChangesType.Now)
            {
                Context.SaveChanges();
            }

            //Email Methods
            Emailer.OnPerformerExecutedAnOrder(order, GetCustomer(order), GetPerformerEmail());
        }

        #endregion

        #region Private Help Methods
        void SetCustomerCulture(Order order)
        {
            string lang = Context.Users.FirstOrDefault(u => u.Id == order.UserGuid).Language;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
        }

        ApplicationUser GetCustomer(Order order)
        {
            return Context.Users.Find(order.UserGuid);
        }

        string GetCustomerEmail(Order order)
        {
            return Context.Users.FirstOrDefault(u => u.Id == order.UserGuid).Email;
        }

        string GetPerformerEmail()
        {
            return AdminSettings.AdminEmail;
        }
        #endregion
    }
}
