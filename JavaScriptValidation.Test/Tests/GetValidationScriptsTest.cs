using Common.JavascriptValidation.Statics;
using JavaScriptValidation.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace JavaScriptValidation.Test.Tests
{
    [TestClass]
    public class GetValidationScriptsTest
    {
        [TestMethod]
        public void TestRequiredGetErrorText()
        {
            List<JsProperty> jsObect = JSValidationMaker.GetValidationObject(typeof(TestModel));

            //Assert.IsTrue(string.IsNullOrEmpty(jsObect.First().))
        }
    }
}
