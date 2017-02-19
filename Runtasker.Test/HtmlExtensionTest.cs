using Microsoft.VisualStudio.TestTools.UnitTesting;
using Runtasker.ExtensionsUI.UIExtensions.Orders;

namespace Runtasker.Test
{
    [TestClass]
    public class HtmlExtensionTest
    {
        [TestMethod]
        public void HtmlActionButtonTest()
        {
            HtmlActionButtonLink button = new HtmlActionButtonLink
            (
                buttonLink: "link",
                buttonText: "click me",
                htmlAttributes: new { id = "1", @class = "btn btn-lg"}
            );

            string expectedResult = "<a  href='link' id='1' class='btn btn-lg'>click me</a>";
            Assert.AreEqual(expectedResult, button.ToString());
        }
    }
}
