using Microsoft.VisualStudio.TestTools.UnitTesting;
using Runtasker.Logic;
using Runtasker.Logic.Contexts.Fake;
using System.Linq;

namespace Runtasker.Test.Logic.Workers.Orders
{
    [TestClass]
    public class CustomerOrderWorkerTests
    {
        FakeMyDbContext db = FakeMyDbContext.CreateDatabase();

        [TestMethod]
        public void TestOrderCreation()
        {
            //Assert.AreEqual(2, db.Users.Count());
            //db.Attachments.Local.Where
            


        }
    }
}
