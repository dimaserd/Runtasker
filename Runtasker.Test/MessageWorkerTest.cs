﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Runtasker.Hubs;

namespace Runtasker.Test
{
    [TestClass]
    public class MessageWorkerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            AboutOrderChatHub hub = new AboutOrderChatHub();
            OrderHubMessage message = new OrderHubMessage
            {
                Attachments = "attachments",
                
                ReceiverName = "receiver",
                SenderName = "sender",
                Text = "text",
                ToGuid = "toguid",
                UserGuid = "userGuid"
            };

            object o = message;
            OrderHubMessage mes = o as OrderHubMessage;

            Assert.AreEqual(message, mes);
        }

        
    }
}
