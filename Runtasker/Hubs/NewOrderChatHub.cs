using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Runtasker.Logic;
using Runtasker.Logic.Models.Messages;
using System.Linq;

namespace Runtasker.Hubs
{
    [HubName("newOrderChatHub")]
    public class NewOrderChatHub : Hub
    {

        #region Конструкторы
        /// <summary>
        /// Конструктор без параметров (Без него все идет в полную жопу)
        /// </summary>
        public NewOrderChatHub()
        {
            _db = new MyDbContext();
        }

        public NewOrderChatHub(MyDbContext db)
        {
            _db = Db;
        }
        #endregion

        #region Поля

        MyDbContext _db;
        #endregion

        #region Свойства

        /// <summary>
        /// Свойство контекста (dbo для подключения к базе данных)
        /// </summary>
        public MyDbContext Db { get { return _db; } }
        #endregion

        #region Основные методы

        #region Методы пришедшие с клиента

        public void AddToGroup(string senderId, string receiverId)
        {
            string groupName = GetGroup(receiverId, senderId);

            Clients.Group(groupName).onAddedToGroup(groupName);
        }

        /// <summary>
        /// Метод отправки сообщения, Метод вызывается с клиента
        /// </summary>
        /// <param name="message"></param>
        public void SendMessageAboutOrder(OrderChatMessage message)
        {

            
            //Сохраняю сообщение в базе
            SaveMessageInDb(message);

            //вызов метода на клиенте 
            OnMessageSend(message);
        }

        /// <summary>
        /// Метод который помечает сообщение как прочитанное в базе данных
        /// (для небольшой защиты от подделки запросов принимается receiverId и senderId
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="receiverId"></param>
        public void ReadMessageAboutOrder(int messageId, string receiverId)
        {
            Logic.Entities.Message message = Db.Messages
                .FirstOrDefault(x => x.Id == messageId && x.ReceiverId == receiverId);

            if(message == null)
            {
                return;
            }

            message.Status = Logic.Entities.MessageStatus.Read;
            // Указать, что запись изменилась
            Db.Messages.Attach(message);
            Db.Entry(message).Property(x => x.Status).IsModified = true;

            Db.SaveChanges();

            OnMessageRead(message);
        }

        #endregion

        #region Методы вызываемые на клиенте

        private void OnMessageSend(OrderChatMessage message)
        {
            //получаем группу пользователя
            //(в дальнейшем усложни метод получения группы но вылижи наследование между объектами
            //описывающими сообщения)
            string groupName = GetGroup(message.ReceiverId, message.SenderId);

            //Вызов на клиентах данный группы этого методы
            //!!!!Важно на клиенте нельзя принимать объект
            //у этой функции есть джава скрипт двойник
            Clients.Group(groupName).onNewMessage(message);
        }

        private void OnMessageRead(Logic.Entities.Message message)
        {
            //получаем группу пользователя
            //(в дальнейшем усложни метод получения группы но вылижи наследование между объектами
            //описывающими сообщения)
            string groupName = GetGroup(message.ReceiverId, message.SenderId);

            //Вызов на обратившемся клиенте javascript функции
            Clients.Caller.onMessageRead(message.Id);
        }

        #endregion

        #endregion

        #region Вспомогательные методы
        public void SaveMessageInDb(OrderChatMessage message)
        {
            Logic.Entities.Message mes = message.ToMessage();

            Db.Messages.Add(mes);
            Db.SaveChanges();

            message.FormattedDate = mes.Date.ToString("G");
            message.MessageId = mes.Id;
        }
        #endregion

        

        #region Защищенные методы

        /// <summary>
        /// Получает группу для текущего пользователя чата и добавляет его соединение
        /// к имени группы
        /// </summary>
        /// <param name="toGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        protected string GetGroup(string toGuid, string userGuid)
        {
            string groupName;

            groupName = (string.Compare(toGuid, userGuid) > 0) ? $"{toGuid}{userGuid}" : $"{userGuid}{toGuid}";

            //собрали группу и добавили пользователя туда
            Groups.Add(Context.ConnectionId, groupName);
            return groupName;
        }

        
        #endregion
    }
}