using Microsoft.AspNet.Identity;
using Runtasker.Logic;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models.Messages;
using Runtasker.Logic.Workers.Message;
using Runtasker.Logic.Workers.MessageWorker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Runtasker.Controllers.Api
{
    [RoutePrefix("api/message")]
    public class ApiMessageController : ApiController
    {
        #region Поля
        AdminMessageWorker _adminMessager;

        MessageWorker _messageWorker;

        MessageOrderWorker _messageOrderWorker;

        MyDbContext _context;
        #endregion

        #region Свойства

        string UserGuid
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        MessageWorker Messager
        {
            get
            {
                if (_messageWorker == null)
                {
                    _messageWorker = new MessageWorker(Context, UserGuid);
                }
                return _messageWorker;
            }
        }


        MessageOrderWorker OrderMessager
        {
            get
            {
                if (_messageOrderWorker == null)
                {
                    _messageOrderWorker = new MessageOrderWorker(UserGuid, Context);
                }
                return _messageOrderWorker;
            }
        }

        MyDbContext Context
        {
            get
            {
                if(_context == null)
                {
                    _context = new MyDbContext();
                }
                return _context;
            }
        }

        AdminMessageWorker AdminMessager
        {
            get
            {
                if(_adminMessager == null)
                {
                    _adminMessager = new AdminMessageWorker(Context, UserGuid);
                }
                return _adminMessager;
            }

        }
        #endregion



        #region Http Api методы

        [Route("GetOrderChatInfos")]
        public async Task<IEnumerable<OrderChatInfo>> GetOrderChatInfos()
        {
            return await AdminMessager.GetOrderChatInfosAsync();
        }

        [Route("GetChatAboutOrder")]
        public async Task<IEnumerable<OrderChatMessage>> GetChatAboutOrder(int orderId)
        {
            return await AdminMessager.GetChatAboutOrderAsync(orderId, User.Identity.GetName());
        }
        #endregion


        #region Dispose
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            IDisposable[] toDisposes = new IDisposable[]
            {
                _adminMessager,

                _context,
             };

            for(int i = 0; i < toDisposes.Length; i++)
            {
                if(toDisposes[i] != null)
                {
                    toDisposes[i].Dispose();
                    toDisposes = null;
                }
            }
        }

        #endregion
    }
}
