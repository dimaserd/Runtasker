using Logic.Extensions.Models;
using Newtonsoft.Json.Linq;
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
    }
}
