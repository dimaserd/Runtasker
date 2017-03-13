using Logic.Extensions.Models;
using Newtonsoft.Json.Linq;
using VkNet;
using VkNet.Model.RequestParams;
using VkParser.Constants;
using VkParser.Workers.Api;

namespace VkParser.Workers.Post
{
    public class WallPostWorker : VkApiWorkerBase
    {
        #region Public methods
        public WorkerResult WriteTextOnGroupWall(string text, int groupId)
        {
            string method = "wall.post";

            string paramsString = $"message={text}&owner_id=-{groupId}";

            JObject response = Request(method, paramsString);

            return new WorkerResult
            {
                Succeeded = true
            };
        }

        public WorkerResult WriteTextOnGroupWallTest()
        {
            int appId = VkConstants.ClientId; // указываем id приложения
            string login = VkConstants.VkFakeLogin; // email для авторизации
            string password = VkConstants.VkFakePassword; // пароль
            VkNet.Enums.Filters.Settings settings = VkNet.Enums.Filters.Settings.All; // уровень доступа к данным

            var api = new VkApi();
            api.Authorize(new ApiAuthParams
            {
                ApplicationId = appID,
                Login = login,
                Password = password,
                Settings = settings
            });
            

            var post = api.Wall.Post(new WallPostParams
            {
                OwnerId = VkConstants.RuntaskerGroupId,
                FromGroup = false,
                Message = "text text",

            });

            return new WorkerResult
            {
                Succeeded = true
            };
        }
        #endregion
    }
}
