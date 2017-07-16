using Runtasker.Logic.Contexts.Interfaces;
using System;
using System.Collections.Generic;

namespace Runtasker.Logic.Workers.MessageWorker
{
    #region Class For Hub Operations
    public class UIHubMessage
    {
        #region Свойства
        /*For JavaScript message building*/
        public int Id { get; set; }

        public string Text { get; set; }

        public string Attachments { get; set; }

        public string NickName { get; set; }

        public DateTime Date { get; set; }
        /*/For JavaScript message building*/

        /// <summary>
        /// Id отправителя
        /// </summary>
        public string SenderGuid { get; set; }

        /// <summary>
        /// Id получателя
        /// </summary>
        public string ReceiverGuid { get; set; }
        
        #endregion

        
    }

    public static class UIHubMessageExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="senderName"></param>
        /// <returns></returns>
        public static UIHubMessage ToUIHubMessage(this Entities.Message mes, string senderName)
        {
            //Второй раз имя не обязательно посылать
            return new UIHubMessage
            {
                /*For group building*/
                SenderGuid = mes.SenderId,
                ReceiverGuid = mes.ReceiverId,
                /*/For group building*/

                /*For building message in Chat via JavaScript*/
                Id = mes.Id,
                Attachments = mes.AttachmentId,
                Date = mes.Date,
                NickName = senderName,
                Text = mes.Text
                /*/For building message in Chat via JavaScript*/
            };
        }
    }
    #endregion

    #region Class For JavaScript ChatBuilder


    


    #endregion

    public class MessageWorkerBase
    {
        #region Конструкторы
        public MessageWorkerBase()
        {
            _context = new MyDbContext();
        }

        public MessageWorkerBase(IMyDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Private Fields

        private IMyDbContext _context;

        #endregion

        #region Public Properties
        public IMyDbContext Context { get { return _context; } }
        #endregion

        #region Virtual Methods

        public virtual Entities.Message SendMessage(Entities.Message message)
        {
            return message;
        }

        public virtual IEnumerable<Entities.Message> GetChat()
        {
            return new List<Entities.Message>();
        }

        #endregion
    }
}
