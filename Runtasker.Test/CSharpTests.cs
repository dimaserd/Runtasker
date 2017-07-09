using Microsoft.VisualStudio.TestTools.UnitTesting;

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
