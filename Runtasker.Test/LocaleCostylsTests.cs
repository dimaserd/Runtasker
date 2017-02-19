
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Runtasker.Test
{
    [TestClass]
    public class LocaleCostylsTests
    {
        [TestMethod]
        public void ParseDecimal()
        {
            string maybeDecimal = "1.00";

            decimal dec = 0;

            bool result = decimal.TryParse(maybeDecimal, out dec);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void ParseDecimal1()
        {
            string maybeDecimal = "1,00";

            decimal dec = 0;

            bool result = decimal.TryParse(maybeDecimal, out dec);
            Assert.AreEqual(true, result);
        }
    }
}
