using Microsoft.VisualStudio.TestTools.UnitTesting;
using Runtasker.Logic;
using Runtasker.Logic.Contexts.Fake;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Notifications;
using System.Linq;

namespace Runtasker.Test.Logic.Workers.Notifications
{
    [TestClass]
    public class NotificationsTestClass
    {
        TestDbContext db = new TestDbContext();


        

        [TestMethod]
        public void TestNotification()
        {
            

            ApplicationUser user = new ApplicationUser
            {
                UserName = "testUserName",
                Balance = 300.00m,
                EmailConfirmed = true,
                Email = "test@mail.com"
            };

            db.Users.Add(user);
            db.SaveChanges();

            WebUINotificater notificater = new WebUINotificater(user.Id, db);
            Notification model = notificater.GetNotification();

            Assert.AreEqual(db.Users.Count(), 1);
            Assert.AreEqual(model, null);
        }
    }
}
