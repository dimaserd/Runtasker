using Runtasker.Logic;
using Runtasker.Logic.Entities;
using System.Collections.Generic;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNet.Identity;
using Runtasker.Logic.Models.Orders;
using Runtasker.Logic.Statics.Actions;
using Runtasker.Logic.Workers.Menus;

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
            using (MenuWorker worker = new MenuWorker(db, UserGuid))
            {
                if (User.IsInRole("Customer"))
                {
                    return await worker.GetUserOrdersInfoForCustomerAsync();
                }
                else
                {
                    return await worker.GetUserOrdersInfoForAdminAsync();
                }
            }
        }
    }
}
