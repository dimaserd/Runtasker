using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using VkParser.Entities;
using VkParser.Models;
using VkParser.PostFinders;

namespace Runtasker.Test.VkParse
{
    [TestClass]
    public class VkPostFinderTests
    {
        

        [TestMethod]
        public void PostFinderTestMethod()
        {
            List<VkGroup> vkGroups = new List<VkGroup>();
            vkGroups.Add(new VkGroup { GroupId = 58897819 });
            vkGroups.Add(new VkGroup { GroupId = 61055670 });

            VkPostFinder finder = new VkPostFinder();

            List<VkGroupPostsFromAPI> result = finder.FindMethod(vkGroups);

            Assert.AreEqual(2, result.Count);
        }
    }
}
