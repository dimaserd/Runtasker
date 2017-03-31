using Microsoft.VisualStudio.TestTools.UnitTesting;
using Runtasker.Logic;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers.Orders;
using System.Linq;

namespace Runtasker.Test
{
    [TestClass]
    public class CSharpTests
    {
        [TestMethod]
        public void Test()
        {
            int id = 5;

            string s1 = $"id:{id}";
            string s2 = string.Format("id:{0}", id);
            Assert.AreEqual(s1, s2);
        }
    }
}
