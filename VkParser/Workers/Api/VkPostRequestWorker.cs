using Extensions.Date;
using Extensions.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VkParser.Entities;
using VkParser.Extensions;
using VkParser.Workers.Parse;

namespace VkParser.Workers.Api
{
    //никаких записей в базу данных 
    //только запросы к апи и их обработка
    //не более не менее
    public class VkPostRequestWorker : IDisposable
    {
        #region Constructor
        public VkPostRequestWorker()
        {
            Construct();
        }

        void Construct()
        {
            VkApi = new VkApiWorkerBase();
            JsonParser = new JsonPostParser();
        }
        #endregion

        #region Properties

        VkApiWorkerBase VkApi { get; set; }

        JsonPostParser JsonParser { get; set; }
        #endregion

        #region Public Methods
        public IEnumerable<VkFoundPost> GetHundredPostsFromGroup(VkGroup vkGroup, int offSet)
        {
            Thread.Sleep(300);

            List<VkFoundPost> result = new List<VkFoundPost>();

            string method = "wall.get";
            string paramsString = $"owner_id={-vkGroup.GroupId}&count={100}&offset{0}&extended=1";

            JObject resp = VkApi.Request(method, paramsString);

            //обработай ошибку по правилам вк
            if (!resp["error"].IsNullOrEmpty())
            {
                if (resp["error"]["error_msg"].ToString().ToLower().Contains("access denied"))
                {
                    return result;
                }
                Thread.Sleep(700);
                GetHundredPostsFromGroup(vkGroup, offSet);
            }

            JToken posts = resp["response"]["wall"];

            foreach (JToken post in posts)
            {
                VkFoundPost vkPost = JsonParser.GetPostFromJSON(post).ToVkFoundPost();
                if (vkPost != null)
                {
                    result.Add(vkPost);
                }

            }

            return result;
        }

        public async Task<IEnumerable<VkFoundPost>> GetHundredPostsFromGroupAsync(VkGroup vkGroup, int offSet)
        {
            Thread.Sleep(300);

            List<VkFoundPost> result = new List<VkFoundPost>();

            string method = "wall.get";
            string paramsString = $"owner_id={-vkGroup.GroupId}&count={100}&offset{0}&extended=1";

            JObject resp = await VkApi.RequestAsync(method, paramsString);

            //обработай ошибку по правилам вк
            if (!resp["error"].IsNullOrEmpty())
            {
                if (resp["error"]["error_msg"].ToString().ToLower().Contains("access denied"))
                {
                    return result;
                }
                Thread.Sleep(700);
                await GetHundredPostsFromGroupAsync(vkGroup, offSet);
            }

            JToken posts = resp["response"]["wall"];

            foreach (JToken post in posts)
            {
                VkFoundPost vkPost = JsonParser.GetPostFromJSON(post).ToVkFoundPost();
                if (vkPost != null)
                {
                    result.Add(vkPost);
                }
            }

            return result;
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    VkApi.Dispose(disposing);
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.
                VkApi = null;

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~VkPostRequestWorker() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.

        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
