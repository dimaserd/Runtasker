using Extensions.Json;
using Logic.Extensions.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using VkParser.Constants;
using VkParser.Contexts;
using VkParser.Entities;
using VkParser.Models;
using VkParser.Workers.Api;

namespace VkParser.Workers
{
    public class VkGroupWorker : VkApiWorkerBase
    {
        #region Constructors
        public VkGroupWorker(VkParseContext context)
        {
            Construct(context);
        }

        void Construct(VkParseContext context)
        {
            Context = context;
            Preparations();
        }
        #endregion

        #region Properties
        VkParseContext Context { get; set; }
        #endregion

        #region Public methods
        public IEnumerable<VkGroup> GetGroups()
        {
            return Context.VkGroups;
        }



        #region Create Group Methods
        public WorkerResult AddNewGroup(VkAddGroupModel model)
        {
            VkGroup group = Context.VkGroups.FirstOrDefault(g => g.ScreenName == model.GroupName);
            if (group != null)
            {
                return new WorkerResult($"Cообщество {group.Name} уже добавлено!");
            }

            JObject response;

            //получаем информацию о сообществе
            VkGroupInfo info = GetGroupInfo(model.GroupName, out response);

            //если пользователь принадлежит сообществу то
            if (info != null)
            {                
                //добавляем информацию в базу
                Context.VkGroups.Add(info.ToVkGroup());
                Context.SaveChanges();

                //проверяем сообщество на новые посты

                return new WorkerResult
                {
                    Succeeded = true
                };
            }
            else
            {    
                return new WorkerResult("Произошла ошибка! Возможно ошибка в названии группы!");          
            }
            
        }
        #endregion

        #region Recovery methods
        public async Task<WorkerResult> DeleteAllGroupsAndGetFromVk()
        {
            //удаляем группы и все найденные посты
            Context.VkFoundPosts.RemoveRange(Context.VkFoundPosts);
            await Context.SaveChangesAsync();
            Context.VkGroups.RemoveRange(Context.VkGroups);
            await Context.SaveChangesAsync();

            //получили список из групп по токену
            JObject response;
            List<VkGroupInfo> vkGroupInfos = GetVkGroupsInfosFromVkPage(out response)
                .ToList();

            foreach(VkGroupInfo info in vkGroupInfos)
            {
                Context.VkGroups.Add(info.ToVkGroup());
            }

            await Context.SaveChangesAsync();

            return new WorkerResult
            {
                Succeeded = true
            };
        }

        #endregion
        #endregion

        #region Help Methods

        //проверяет  изменения подписки на сообщества
        void Preparations()
        {
            List<VkGroup> vkGroups = GetGroups().ToList();
            foreach(VkGroup vkGroup in vkGroups)
            {
                if(!vkGroup.IsMember)
                {
                    CheckForMembership(vkGroup);
                }
            }
        }

        void CheckForMembership(VkGroup vkGroup)
        {
            JObject response;
            VkGroupInfo info = GetGroupInfo(vkGroup.GroupId.ToString(), out response);

            if(info != null && info.IsMember)
            {
                //сохранение в базе идентиикатора поста
                vkGroup.IsMember = info.IsMember;
                var entry = Context.Entry(vkGroup);
                entry.Property(e => e.IsMember).IsModified = true;
                Context.SaveChanges();
            }

            
        }

        VkGroupInfo GetGroupInfo(string groupId, out JObject response)
        {
            string method = "groups.getById";
            string paramsString = $"version=5.60&group_id={groupId}&fields=description";

            response = Request(method: method, paramsString: paramsString);

            //Logging
            #region Log
            string filesDir = HostingEnvironment.MapPath("~/Files");
            string filePath = $"{filesDir}/json_text.txt";
            string fileContents = response.ToString();
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }  
            File.AppendAllText(filePath, fileContents);
            #endregion

            if (!response["error"].IsNullOrEmpty())
            {
                return null;
            }

            JToken resp = response["response"][0];
            //потому что групп может быть много

            if (!resp.IsNullOrEmpty())
            {
                bool isMember = resp["is_member"].IsNullOrEmpty() ? false : Convert.ToBoolean(int.Parse(resp["is_member"].ToString()));
                return new VkGroupInfo
                {
                    Id = int.Parse(resp["gid"].ToString()),
                    ScreenName = resp["screen_name"].ToString(),
                    Name = resp["name"].ToString(),
                    IsClosed = Convert.ToBoolean(int.Parse(resp["is_closed"].ToString())),
                    IsMember = isMember
                };
            }
            return null;
        }

        IEnumerable<VkGroupInfo> GetVkGroupsInfosFromVkPage(out JObject response)
        {
            string method = "groups.get";
            string paramsString = $"version=5.62&extended=true&user_id={VkConstants.UserId}";

            response = Request(method: method, paramsString: paramsString);


            if (!response["error"].IsNullOrEmpty())
            {
                return null;
            }

            List<VkGroupInfo> result = new List<VkGroupInfo>();

            JToken groups = response["response"];
            
            foreach(JToken group in groups)
            {
                if(group.Type == JTokenType.Integer)
                {
                    continue;
                }

                result.Add(new VkGroupInfo
                {
                    Id = int.Parse(group["gid"].ToString()),
                    ScreenName = group["screen_name"].ToString(),
                    Name = group["name"].ToString(),
                    IsMember = true
                });
            }

            return result;
        }
        #endregion

    }
}
