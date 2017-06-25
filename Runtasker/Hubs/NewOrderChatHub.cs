using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Runtasker.Logic;
using Runtasker.Logic.Models.Messages;

namespace Runtasker.Hubs
{
    [HubName("newOrderChatHub")]
    public class NewOrderChatHub : Hub
    {

        #region Конструкторы
        public NewOrderChatHub()
        {
            _context = new MyDbContext();
        }

        public NewOrderChatHub(MyDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Основные методы

        #region Методы пришедшие с клиента


        public void SendMessage(object text)
        {

        }
        /// <summary>
        /// Метод отправки сообщения, Метод вызывается с клиента
        /// </summary>
        /// <param name="message"></param>
        public void SendMessageAboutOrder(object mes)
        {

            OrderChatMessage message = mes as OrderChatMessage;
            //Сохраняю сообщение в базе
            SaveMessageInDb(message);

            //вызов метода на клиенте (у этой функции есть javascript двойник - приемник)
            OnMessageSend(message);
        }

        #endregion

        #region Методы вызываемые на клиенте

        public void OnMessageSend(OrderChatMessage message)
        {
            //получаем группу пользователя
            //(в дальнейшем усложни метод получения группы но вылижи наследование между объектами
            //описывающими сообщения)
            string groupName = GetGroup(message.ReceiverId, message.SenderId);

            //Вызов на клиентах данный группы этого методы
            //!!!!Важно на клиенте нельзя принимать объект
            Clients.Group(groupName).onNewMessage(message);
        }

        public void OnMessageRead(int messageId)
        {
            
        }

        #endregion

        #endregion

        #region Вспомогательные методы
        public void SaveMessageInDb(OrderChatMessage message)
        {
            Logic.Entities.Message mes = message.ToMessage();

            context.Messages.Add(mes);
            context.SaveChanges();
        }
        #endregion

        #region Поля

        MyDbContext _context;
        #endregion

        #region Свойства

        /// <summary>
        /// Свойство контекста (записано с маленькой буквы, чтобы избежать конфликтов(
        /// </summary>
        public MyDbContext context { get { return _context; } }
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

        /// <summary>
        /// Метод достающий имя пользователя из базы (Его нужно переделать!)
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        protected string GetSenderNickName(string guid)
        {
            return context.Users.Find(guid).Name;
        }

        #endregion
    }
}