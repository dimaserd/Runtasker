using Runtasker.Logic.Entities;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Admin.Email;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System;
using Runtasker.Settings;

namespace Runtasker.Logic.Workers.Admin
{
    public class AdminCustomerMethods
    {
        #region Константы

        #endregion

        #region Constructors
        public AdminCustomerMethods(MyDbContext context)
        {
            Db = context;
            Construct();
        }

        void Construct()
        {
            Emailer = new AdminEmailSender();
            Delete = new AdminDeleteMethods();
        }
        #endregion

        #region Поля
        ApplicationUser _customer;
        #endregion

        #region Свойства
        AdminEmailSender Emailer { get; set; }

        MyDbContext Db { get; set; }

        public AdminDeleteMethods Delete { get; set; }
        #endregion

        #region Публичные методы

        #region Написание почты
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

        #region Удаление заказов
        public Order GetDeleteOrderModel(int orderId)
        {
            return Db.Orders.FirstOrDefault(o => o.Id == orderId);
        }

        public void DeleteOrder()
        {

        }
        #endregion

        #region Users

        #region Performers
        public async Task<IEnumerable<ApplicationUser>> GetPerformersAsync()
        {
            
                string[] performerRolesIds = await (from r in Db.Roles
                                             where (r.Name == "Performer") || (r.Name == "VkPerformer")
                                             select r.Id).ToArrayAsync();

                string role0 = performerRolesIds[0];
                string role1 = performerRolesIds[1];

                List<ApplicationUser> result = await (from user in Db.Users
                              where user.Roles.Any(r => r.RoleId == role0 || r.RoleId == role1)
                              orderby user.RegistrationDate
                              select user).ToListAsync();

                ApplicationUser userAdmin = result.FirstOrDefault(u => u.Email == AdminSettings.AdminEmail);
                if(userAdmin != null)
                {
                    result.Remove(userAdmin);
                }

                return result;
              
        }
        
        public async Task<PerformerInfo> GetPerformerInfoAsync(string id)
        {
            
                ApplicationUser user = await Db.Users
                    .FirstOrDefaultAsync(u => u.Id == id);

                List<Message> messages = await (from m in Db.Messages
                                          where m.SenderGuid == id || m.ReceiverGuid == id
                                          select m)
                                          .Include(x => x.Receiver)
                                          .Include(x => x.Sender)
                                          .ToListAsync();

                List<Order> orders = await (from o in Db.Orders
                                      where o.PerformerGuid == id
                                      select o).Include(x => x.Customer)
                                      .ToListAsync();

                List<PaymentTransaction> pts = await (from pt in Db.PaymentTransactions
                                                where pt.UserGuid == id
                                                select pt).ToListAsync();

                OtherUserInfo info = await Db.OtherUserInfos
                    .FirstOrDefaultAsync(x => x.Id == id);

                return new PerformerInfo(user, messages, orders, pts, info);
                
            
        }
        #endregion


        #region Customers
        public IEnumerable<ApplicationUser> GetCustomers()
        {
            
                string customeRoleId = Db.Roles.FirstOrDefault(r => r.Name == "Customer").Id;

                return (from user in Db.Users
                        where user.Roles.Any(r => r.RoleId == customeRoleId)
                        orderby user.RegistrationDate
                        select user
                       ).ToList();
              
        }

        public async Task<IEnumerable<ApplicationUser>> GetCustomersAsync()
        {
            string customeRoleId = Db.Roles.FirstOrDefault(r => r.Name == "Customer").Id;

            return await (from user in Db.Users
                          where user.Roles.Any(r => r.RoleId == customeRoleId)
                          orderby user.RegistrationDate
                          select user
                   ).ToListAsync();

        }

        public async Task<CustomerInfo> GetCustomerInfoAsync(string id)
        {          
            
                return new CustomerInfo
                {
                    User = await Db.Users.FirstOrDefaultAsync(u => u.Id == id),
                                

                    Orders = await (from o in Db.Orders
                                    where o.UserGuid == id
                                    select o).Include(x => x.Customer).ToListAsync(),

                    Messages = await (from m in Db.Messages
                                where m.SenderGuid == id || m.ReceiverGuid == id
                                select m).Include(x => x.Sender).Include(x => x.Receiver).ToListAsync(),

                    PaymentTransactions = await (from p in Db.PaymentTransactions
                                                 where p.UserGuid == id
                                                 select p).ToListAsync(),
               };

                
            
        }

        

        public ApplicationUser GetCustomer(string id)
        {
            return Db.Users.FirstOrDefault(u => u.Id == id);
                         
        }

        public ApplicationUser GetCustomer()
        {
            return _customer;
        }

        public void SetCustomerField(string id)
        {
            _customer = Db.Users.FirstOrDefault(u => u.Id == id);
        }
        #endregion

        #endregion

        #endregion
    }
}
