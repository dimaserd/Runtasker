using Logic.Extensions.Models;
using Runtasker.Logic.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers.Admin
{
    public class AdminDeleteMethods
    {
        #region Constructors
        public AdminDeleteMethods()
        {
            Construct();
        }

        void Construct()
        {
            
        }
        #endregion

        #region Properties
        #endregion

        #region Public Methods

        #region DeleteUser methods
        public ApplicationUser GetDeleteUserModel(string id)
        {
            using (MyDbContext context = new MyDbContext())
            {
                return context.Users.FirstOrDefault(u => u.Id == id);
            }
        }

        public void DeleteUserConfirmed(ApplicationUser model)
        {
            DeleteUser(model.Id);
        }
        #endregion

        #region Delete user orders

        #region Delete One Order
        public async Task<Order> GetDeleteOrderModelAsync(int id)
        {
            using (MyDbContext db = new MyDbContext())
            {
                return await db.Orders.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public WorkerResult DeleteOrderConfirmed(Order model)
        {
            List<Order> manyModelFromOne = new List<Order>();
            manyModelFromOne.Add(model);

            DeleteOrders(manyModelFromOne);

            return new WorkerResult
            {
                Succeeded = true
            };
        }
        #endregion

        #region DeleteMany Get Models
        public IEnumerable<Order> GetDeleteAllUserOrdersModel(string id)
        {
            using (MyDbContext context = new MyDbContext())
            {
                return context.Orders.Where(o => o.UserGuid == id).ToList();
            }
        }

        public IEnumerable<Order> GetDeleteFinishedUserOrdersModel(string id)
        {
            using (MyDbContext context = new MyDbContext())
            {
                return context.Orders.Where(o => o.UserGuid == id && o.Status >= OrderStatus.Downloaded).ToList();
            }
        }
        #endregion


        public void DeleteOrders(IEnumerable<Order> model)
        {
            using (MyDbContext context = new MyDbContext())
            {
                foreach (Order order in model)
                {
                    IEnumerable<Message> messages = order.Messages;
                    foreach(Message mes in messages)
                    {
                        context.Messages.Remove(mes);
                    }
                }
                context.SaveChanges();

                foreach (Order order in model)
                {
                    context.Orders.Attach(order);
                    context.Orders.Remove(order);
                }
                context.SaveChanges();
            }
        }
        #endregion

        #endregion

        #region Help Methods
        void DeleteUser(string userGuid)
        {
            using (MyDbContext Context = new MyDbContext())
            {
                Context.PaymentTransactions.RemoveRange(Context.PaymentTransactions.Where(pt => pt.UserGuid == userGuid));
                Context.SaveChanges();

                Context.Payments.RemoveRange(Context.Payments.Where(p => p.UserGuid == userGuid));
                Context.SaveChanges();

                Context.Messages.RemoveRange(Context.Messages.Where(m => m.ReceiverGuid == userGuid || m.SenderGuid == userGuid));
                Context.SaveChanges();

                Context.Notifications.RemoveRange(Context.Notifications.Where(n => n.UserGuid == userGuid));
                Context.SaveChanges();

                Context.Orders.RemoveRange(Context.Orders.Where(o => o.UserGuid == userGuid));
                Context.SaveChanges();

                ApplicationUser user = Context.Users.FirstOrDefault(u => u.Id == userGuid);
                Context.Users.Remove(user);
                Context.SaveChanges();
            }
        }
        #endregion
    }
}
