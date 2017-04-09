using HtmlExtensions.HtmlEntities;
using HtmlExtensions.Renderers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Runtasker.Logic.Entities;

namespace Runtasker.Test
{
    [TestClass]
    public class HtmlExtensionsTest
    {
        [TestMethod]
        public void HtmlLinkTest()
        {
            Order order = new Order
            {
                Id = 1,
                Sum = 500.00m
            };
            string button = new HtmlLink
            (
                hrefParam: $"/Orders/PayHalf/{order.Id}",
                textParam: $"Pay {order.Sum / 2}",
                buttonSizeParam: HtmlButtonSize.Large,
                buttonTypeParam: HtmlButtonType.Success
            ).ToString();

            string expectedButton = $"<a class='btn btn-success btn-lg' href='/Orders/PayHalf/{order.Id}'>" +
                "Pay 250,00</a>";
            Assert.AreEqual(expectedButton, button);
        }

        
        [TestMethod]
        public void FAToAnimatedTest()
        {
            
            //"<i class='fa fa-rub'></i>"
            
        }
    }
}
