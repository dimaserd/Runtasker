using Runtasker.Logic.Entities;
using System;
using System.Collections.Generic;

namespace Runtasker.Logic.Workers.MessageWorker
{
    #region Class For Hub Operations
    public class UIHubMessage
    {
        /*For JavaScript message building*/
        public int Id { get; set; }

        public string Text { get; set; }

        public string Attachments { get; set; }

        public string NickName { get; set; }

        public DateTime Date { get; set; }
        /*/For JavaScript message building*/

        /*For group bilding*/
        public string SenderGuid { get; set; }

        public string ReceiverGuid { get; set; }
        /*/For group bilding*/

    }
    #endregion

    #region Class For JavaScript ChatBuilder
    public class ChatUIHubMessage
    {
        /*For JavaScript message building*/
        public int Id { get; set; }

        public string Text { get; set; }

        public string SenderGuid { get; set; }

        public string Attachments { get; set; }

        public string NickName { get; set; }

        public DateTime Date { get; set; }
        /*/For JavaScript message building*/
    }
    #endregion

    public class MessageWorkerBase
    {
        #region Constructors
        public MessageWorkerBase()
        {
            _context = new MyDbContext();
        }

        public MessageWorkerBase(MyDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Private Fields

        private MyDbContext _context;

        #endregion

        #region Public Properties
        public MyDbContext Context { get { return _context; } }
        #endregion

        #region Virtual Methods

        public virtual Message SendMessage(Message message)
        {
            return message;
        }

        public virtual IEnumerable<Message> GetChat()
        {
            return new List<Message>();
        }

        #endregion
    }
}
