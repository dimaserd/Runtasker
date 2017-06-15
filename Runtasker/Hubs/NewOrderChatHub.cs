using Microsoft.AspNet.SignalR.Hubs;
using Runtasker.Logic;
using Runtasker.Logic.Models.Messages;

namespace Runtasker.Hubs
{
    [HubName("newOrderChatHub")]
    public class NewOrderChatHub : ChatHubBase
    {

        #region Конструкторы
        public NewOrderChatHub(MyDbContext context) : base(context)
        {

        }
        #endregion

        #region Основные методы

        #region Методы пришедшие с клиента

        /// <summary>
        /// Метод отправки сообщения
        /// </summary>
        /// <param name="message"></param>
        public void SendMessageAboutOrder(OrderChatMessage message)
        {
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
    }
}