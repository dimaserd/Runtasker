using Runtasker.Logic.Contexts.Interfaces;
using Runtasker.Logic.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Runtasker.Logic.Contexts.Fake
{
    public class LocalDbContext : MyDbContext
    {
        #region Constructor
        public LocalDbContext() : base("LocalTestConection")
        {
            //Удаляем все данные из таблиц
            RemoveAllData();
        }

        void RemoveAllData()
        {
            PaymentTransactions.RemoveRange(PaymentTransactions);
            SaveChanges();

            Messages.RemoveRange(Messages);
            SaveChanges();

            Notifications.RemoveRange(Notifications);
            SaveChanges();

            Payments.RemoveRange(Payments);
            SaveChanges();

            Orders.RemoveRange(Orders);
            SaveChanges();

            List<ApplicationUser> users = Users.ToList();
            foreach (ApplicationUser user in users)
            {
                Users.Remove(user);
            }
            List<OtherUserInfo> userInfos = OtherUserInfos.ToList();
            foreach (OtherUserInfo userInfo in userInfos)
            {
                OtherUserInfos.Remove(userInfo);
            }
            SaveChanges();
            
            
        }


        #endregion

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public new async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
