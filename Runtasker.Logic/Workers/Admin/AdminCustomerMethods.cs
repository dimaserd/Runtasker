using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Admin.Email;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System;

namespace Runtasker.Logic.Workers.Admin
{
    public class AdminCustomerMethods
    {
        #region Constants

        #endregion

        #region Constructors
        public AdminCustomerMethods()
        {
            Construct();
        }

        void Construct()
        {
            Emailer = new AdminEmailSender();
            Delete = new AdminDeleteMethods();
        }
        #endregion

        #region Fields
        ApplicationUser _customer;
        #endregion

        #region Properties
        AdminEmailSender Emailer { get; set; }

        public AdminDeleteMethods Delete { get; set; }
        #endregion

        #region Public methods

        #region WriteEmail methods
        public ActionEmailToCustomer GetWriteEmailModel()
        {
            return new ActionEmailToCustomer
            {
                Email = _customer.Email,
                Name = _customer.Name
            };
        }

        public void WriteEmailToCustomer(ActionEmailToCustomer model)
        {
            Emailer.SendContactEmail(model);
        }
        #endregion

        #region DeleteOrder methods
        public Order GetDeleteOrderModel(int orderId)
        {
            using (MyDbContext context = new MyDbContext())
            {
                return context.Orders.FirstOrDefault(o => o.Id == orderId);
            }
        }

        public void DeleteOrder()
        {

        }
        #endregion

        #region Users

        #region Performers
        public async Task<IEnumerable<ApplicationUser>> GetPerformersAsync()
        {
            using (MyDbContext db = new MyDbContext())
            {
                string[] performerRolesIds = await (from r in db.Roles
                                             where (r.Name == "Performer") || (r.Name == "VkPerformer")
                                             select r.Id).ToArrayAsync();

                string role0 = performerRolesIds[0];
                string role1 = performerRolesIds[1];

                List<ApplicationUser> result = await (from user in db.Users
                              where user.Roles.Any(r => r.RoleId == role0 || r.RoleId == role1)
                              orderby user.RegistrationDate
                              select user).ToListAsync();

                ApplicationUser userAdmin = result.FirstOrDefault(u => u.Email == "dimaserd84@gmail.com");
                if(userAdmin != null)
                {
                    result.Remove(userAdmin);
                }

                return result;
            }  
        }
        
        public async Task<PerformerInfo> GetPerformerInfoAsync(string id)
        {
            using (MyDbContext db = new MyDbContext())
            {
                ApplicationUser user = await db.Users
                    .FirstOrDefaultAsync(u => u.Id == id);

                List<Message> messages = await (from m in db.Messages
                                          where m.SenderGuid == id || m.ReceiverGuid == id
                                          select m)
                                          .Include(x => x.Receiver)
                                          .Include(x => x.Sender)
                                          .ToListAsync();

                List<Order> orders = await (from o in db.Orders
                                      where o.PerformerGuid == id
                                      select o).Include(x => x.Customer)
                                      .ToListAsync();

                List<PaymentTransaction> pts = await (from pt in db.PaymentTransactions
                                                where pt.UserGuid == id
                                                select pt).ToListAsync();

                OtherUserInfo info = await db.OtherUserInfos
                    .FirstOrDefaultAsync(x => x.Id == id);

                return new PerformerInfo(user, messages, orders, pts, info);
                
            }
        }
        #endregion


        #region Customers
        public IEnumerable<ApplicationUser> GetCustomers()
        {
            using (MyDbContext context = new MyDbContext())
            {
                string customeRoleId = context.Roles.FirstOrDefault(r => r.Name == "Customer").Id;

                return (from user in context.Users
                        where user.Roles.Any(r => r.RoleId == customeRoleId)
                        orderby user.RegistrationDate
                        select user
                       ).ToList();
            }
                
        }

        public async Task<IEnumerable<ApplicationUser>> GetCustomersAsync()
        {
            using (MyDbContext context = new MyDbContext())
            {
                string customeRoleId = context.Roles.FirstOrDefault(r => r.Name == "Customer").Id;

                return await (from user in context.Users
                        where user.Roles.Any(r => r.RoleId == customeRoleId)
                        orderby user.RegistrationDate
                        select user
                       ).ToListAsync();
            }

        }

        public async Task<CustomerInfo> GetCustomerInfoAsync(string id)
        {          
            using (MyDbContext db = new MyDbContext())
            {
               
                return new CustomerInfo
                {
                    User = await db.Users.FirstOrDefaultAsync(u => u.Id == id),
                                

                    Orders = await (from o in db.Orders
                                    where o.UserGuid == id
                                    select o).Include(x => x.Customer).ToListAsync(),

                    Messages = await (from m in db.Messages
                                where m.SenderGuid == id || m.ReceiverGuid == id
                                select m).Include(x => x.Sender).Include(x => x.Receiver).ToListAsync(),

                    PaymentTransactions = await (from p in db.PaymentTransactions
                                                 where p.UserGuid == id
                                                 select p).ToListAsync(),
               };

                
            }
        }

        

        public ApplicationUser GetCustomer(string id)
        {
            using (MyDbContext context = new MyDbContext())
            {
                return context.Users.FirstOrDefault(u => u.Id == id);
            }                
        }

        public ApplicationUser GetCustomer()
        {
            return _customer;
        }

        public void SetCustomerField(string id)
        {
            using (MyDbContext context = new MyDbContext())
            {
                _customer = context.Users.FirstOrDefault(u => u.Id == id);
            }
        }
        #endregion

        #endregion

        #endregion
    }
}
