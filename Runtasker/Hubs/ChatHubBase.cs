using Microsoft.AspNet.SignalR;
using Runtasker.Logic;

namespace Runtasker.Hubs
{
    #region Class for UserDictionary
    public class ChatHubUser
    {
        public string Id { get; set; }

        public string Name { get; set; }
            
    }
    #endregion

    #region Message Classes
    public class HubMessage
    {
        public string Text { get; set; }

        public string UserGuid { get; set; }

        public string ToGuid { get; set; }
        //Database methods

        public string SenderName { get; set; }
        //for sending back an HtmlMessage

        public string ReceiverName { get; set; }
        //for sending back an HtmlMessage

        public string Attachments { get; set; }
        //for attaching archives with files
    }

    public class OrderHubMessage : HubMessage
    {
        public int OrderId { get; set; }
        //Use to get it to the mark
    }
    #endregion

    public class ChatHubBase : Hub
    {

        #region Constructors

        public ChatHubBase()
        {
            _context = new MyDbContext();
        }

        #endregion

        #region Future Methods

        /*private IEnumerable<ChatHubUser> GetUsersList()
        {
            return (from u in context.Users.Where(o => o.Id != "")
                    select new ChatHubUser
                    {
                        Id = u.Id,
                        Name = u.Name
                    }).ToList();
        }*/

        #endregion

        #region Private Fields

        private MyDbContext _context;

        //private static List<ChatHubUser> SenderNicknames;

        #endregion

        #region Public Properties
        //low case to avoid conflicts
        public MyDbContext context { get { return _context; } }
        #endregion

        #region Protected Fields
        //Some kind of message worker
        #endregion

        #region Protected Methods
        protected string GetGroup(string toGuid, string userGuid)
        {
            string groupName;
            if (string.Compare(toGuid, userGuid) > 0)
            {
                groupName = $"{toGuid}{userGuid}";
            }
            else
            {
                groupName = $"{userGuid}{toGuid}";
            }
            //собрали группу и добавили пользователя туда
            Groups.Add(Context.ConnectionId, groupName);
            return groupName;
        }

        //This method makes query it could be remade
        protected string GetSenderNickName(string guid)
        {
            return context.Users.Find(guid).Name;
        }

        #endregion

        #region Virtual Methods
        //for now it's better to implement interface

        #endregion
    }
}