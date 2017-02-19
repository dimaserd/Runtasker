using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using VkParser.Entities;
using VkParser.Models;
using VkParser.Workers.Api;
using VkParser.Workers.Parse;

namespace VkParser.PostFinders
{
    /// <summary>
    /// Класс который получает посты из апи
    /// и возвращает лист из групп
    /// </summary>
    public class VkPostFinder
    {
        #region Constructor
        public VkPostFinder()
        {
            Construct();
        }

        void Construct()
        {
            JsonParser = new JsonPostParser();
        }
        #endregion

        #region Properties
        JsonPostParser JsonParser { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Метод возвращает список групп с постами которые были получены из API
        /// никакой обработки только получение нужных данных
        /// </summary>
        /// <returns></returns>
        public List<VkGroupPostsFromAPI> FindMethod(List<VkGroup> vkGroups)
        {
            //результат операции
            List<VkGroupPostsFromAPI> result = new List<VkGroupPostsFromAPI>();

            //для запросов
            VkApiWorkerBase apiBase = new VkApiWorkerBase();
            
            string code = GetQueryToAPI(vkGroups);

            string paramsString = $"code={code}";

            JObject resp = apiBase.Request(method: "execute", paramsString: paramsString);
            
            //обработка ошибок
            if(resp["error"] != null)
            {

            }

            JToken json = resp["response"];
            foreach (JToken groupInfo in json)
            { 
                result.Add(GetVkGroupPostsFromAPI(groupInfo));
            }
            

            return result;
        }
        #endregion

        #region Help Methods
        string GetQueryToAPI(List<VkGroup> vkGroups)
        {
            #region code
            //var a0 = API.wall.get({ "owner_id" : "-58897819", "count" : "100", "offset" : "0", "extended":"1"});
            string t = "var res;";
            for (int i = 0; i < vkGroups.Count; i++)
            {
                t += $"var a{i} = " +
                "API.wall.get({" + $"\"owner_id\" : \"{-vkGroups[i].GroupId}\", \"count\" : \"100\", \"offset\" : \"0\", \"extended\" : \"1\" " + "})" + "; ";

                t += $"res += a{i}; ";
            }
            t += "return res;";
            #endregion

            #region cycle
            string c = "var res = [];";
            c += "var arr = [";
            for (int i = 0; i < vkGroups.Count; i++)
            {
                if (i != vkGroups.Count - 1)
                {
                    c += $"{-vkGroups[i].GroupId}, ";
                }
                else
                {
                    c += $"{-vkGroups[i].GroupId} ];";
                }
            }
            c += $"var i = {vkGroups.Count};";
            c += "while(i > 0)";
            c += "{";
            c += "i--;";
            c += "res.push( API.wall.get( { 'owner_id' : '\" + arr[i] + \"', 'count' : '100', 'offset' : '0', 'extended' : '1' " + "}))" + "; ";
            c += "}";
            c += "return res;";

            #endregion
            string code1 = "return [";
            for (int i = 0; i < vkGroups.Count; i++)
            {
                if (i != vkGroups.Count - 1)
                {
                    code1 += "API.wall.get({" + $"\"owner_id\" : \"{-vkGroups[i].GroupId}\", \"count\" : \"100\", \"extended\" : \"1\" " + "}),";
                }
                else
                {
                    code1 += "API.wall.get({" + $"\"owner_id\" : \"{-vkGroups[i].GroupId}\", \"count\" : \"100\", \"extended\" : \"1\" " + "})" + "]; ";
                }
            }


            string code = code1.ToString();

            return code;
        }

        VkGroupPostsFromAPI GetVkGroupPostsFromAPI(JToken groupInfo)
        {
            //результат операции
            VkGroupPostsFromAPI result = new VkGroupPostsFromAPI();

            if(groupInfo.Type == JTokenType.Boolean)
            {
                return result;
            }

            

            JToken wall = groupInfo["wall"];
            JToken profiles = groupInfo["profiles"];
            JToken info = groupInfo["groups"];
            foreach(JToken post in wall)
            {
                VkAPIFoundPostModel vkPost = JsonParser.GetPostFromJSON(post);
                if (vkPost != null)
                {
                    result.Posts.Add(vkPost);
                }
            }
            

            return result;
        }

        #endregion
    }
}
