using Runtasker.Logic.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Runtasker.Logic.Workers.Developer
{
    public class DataBaseMethods
    {
        #region Constants
        string Password { get { return "56676332"; } }
        #endregion

        #region Constructors
        public DataBaseMethods(MyDbContext context)
        {
            Context = context;
        }
        #endregion

        #region Properties
        MyDbContext Context { get; set; }
        #endregion

        #region Public Methods
        public IEnumerable<ApplicationUser> GetUserTable()
        {
            return Context.Users;
        }

        public string GetCurrentOrderTableStateInJson()
        {
            StringBuilder sb = new StringBuilder();
          


            List<Order> orders = Context.Orders.ToList();
            
            sb.Append(Newtonsoft.Json.JsonConvert.SerializeObject(orders).ToString());


            return sb.ToString();
        }

        public string ResetTestUser()
        {
            ApplicationUser testUser = Context.Users.FirstOrDefault(u => u.Email == "dimaserd96@yandex.ru");
            if(testUser == null)
            {
                return "Пользователя не существует!";
            }
            Context.PaymentTransactions.RemoveRange(Context.PaymentTransactions.Where(pt => pt.UserGuid == testUser.Id));
            Context.SaveChanges();

            Context.Payments.RemoveRange(Context.Payments.Where(p => p.UserGuid == testUser.Id));
            Context.SaveChanges();

            Context.Messages.RemoveRange(Context.Messages.Where(m => m.ReceiverId == testUser.Id || m.SenderId == testUser.Id));
            Context.SaveChanges();

            Context.Notifications.RemoveRange(Context.Notifications.Where(n => n.UserGuid == testUser.Id));
            Context.SaveChanges();

            List<Order> orders = Context.Orders.Include(x => x.Attachments).Where(o => o.UserGuid == testUser.Id).ToList();

            orders.ForEach(x =>
            {
                Context.Attachments.RemoveRange(x.Attachments);
                
                Context.Orders.Remove(x);
            });
            
            Context.SaveChanges();

            Context.Users.Remove(testUser);
            Context.SaveChanges();
            return $"пользователь {testUser.Email} удален!";
        }
        #endregion
    }
}
