using Microsoft.VisualStudio.TestTools.UnitTesting;
using Runtasker.Logic.Workers.Payments.PaymentGetters;

namespace Runtasker.Test.Payment
{
    [TestClass]
    public class EncryptingTestClass
    {
        YandexKassaPaymentGetter getter = new YandexKassaPaymentGetter(new F);

        [TestMethod]
        public void PreTest()
        {
            string yandexVal = "paymentAviso;10.00;10643;1003;130768;2000001134313;dimaserd96@yandex.ru;runtasker37and0qwZ";

            string yandexMD5 = "9DFF95E2622F7B46649F5889053EBEA6";

            string countedMd5 = getter.CreateMD5(yandexVal);

            Assert.AreEqual(yandexMD5, countedMd5.ToUpper());
        }

    }
}
