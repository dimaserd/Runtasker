using Runtasker.Logic;
using Runtasker.Logic.Entities;
using System.Collections.Generic;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNet.Identity;
using Runtasker.Logic.Models.Orders;

namespace Runtasker.Controllers.Api
{
    [RoutePrefix("api/menu")]
    public class ApiMenuController : ApiController
    {
        string UserGuid
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        [Route("GetOrders")]
        public async Task<UserOrdersInfo> GetOrders()
        {
            using (MyDbContext db = new MyDbContext())
            {
                List<Order> orders = await db.Orders
                    .Where(o => o.UserGuid == UserGuid)
                    .Include(x => x.Messages).ToListAsync();

                List<Order> unApreciated = orders.Where(o => o.Status != OrderStatus.Appreciated).ToList();

                List<OrderMessagesInfo> infos = unApreciated
                    .Select(x => new OrderMessagesInfo
                    {
                        Id = x.Id,
                        UnreadCount = x.Messages.Count(m => m.ReceiverGuid == UserGuid && m.Status == MessageStatus.New)
                    }).ToList();

                return new UserOrdersInfo
                {
                    UnreadCount = unApreciated.SelectMany(x => x.Messages)
                        .Count(m => m.ReceiverGuid == UserGuid && m.Status == MessageStatus.New),
                    HasOrders = orders.Count > 0,
                    OrderMessageInfos = infos
                };
            }
            
        }
    }
}
