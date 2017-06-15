using Microsoft.AspNet.SignalR;
using Runtasker.Logic;
using Runtasker.Logic.Contexts.Interfaces;

namespace Runtasker.Hubs
{ 
    /// <summary>
    /// Базовый класс хаба для чата
    /// </summary>
    public class ChatHubBase : Hub
    {
        #region Конструкторы

        public ChatHubBase()
        {
            _context = new MyDbContext();
        }

        public ChatHubBase(MyDbContext contextParam)
        {
            _context = contextParam;
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
            
            groupName = (string.Compare(toGuid, userGuid) > 0)? $"{toGuid}{userGuid}" : $"{userGuid}{toGuid}";
            
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