using Logic.Extensions.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using VkParser.Enumerations;
using VkParser.Models.MessageSenderModels;
using VkParser.Workers.Api;

namespace VkParser.MessageSenders
{
    public class VkMessageSender : VkApiWorkerBase
    {
        public VkMessageSender()
        {

        }

        #region Properties

        #endregion

        public WorkerResult SendMessageToVkUser(VkMessage message)
        {
            string method = "messages.send";
            string paramsString = string.Empty;
            if (message.UserDomain != null)
            {
                paramsString = $"domain={message.UserDomain}&message={message.Text}";
            }
            else
            {
                paramsString = $"user_id={message.UserId}&message={message.Text}";
            }
            JObject response = Request(method, paramsString, VkTokenType.RuntaskerGroup);

            return new WorkerResult
            {
                Succeeded = true
            };

        }

        public WorkerResult SendMessagesToVkUsers(IEnumerable<VkMessage> messages)
        {
            foreach(VkMessage message in messages)
            {
                Thread.Sleep(250);
                WorkerResult sendingResult = SendMessageToVkUser(message);
                if(!sendingResult.Succeeded)
                {
                    return sendingResult;
                }
            }

            return new WorkerResult
            {
                Succeeded = true
            };
        }
    }
}
