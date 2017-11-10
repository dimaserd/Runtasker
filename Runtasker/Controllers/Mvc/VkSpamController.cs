using Runtasker.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VkParser.Entities;
using System.Data.Entity;
using VkParser.Workers.Api;
using Newtonsoft.Json.Linq;
using VkParser.Entities.Spam;

namespace Runtasker.Controllers.Mvc
{
    public class VkSpamController : BaseMvcController
    {
        List<VkMan> _vkMen = null;

        public List<VkMan> VkMen
        {
            get
            {
                if(_vkMen == null)
                {
                    _vkMen = Db.VkMen.ToList();
                }
                return _vkMen;
            }
        }
        // GET: VkSpam
        public ActionResult Index()
        {
            List<VkGroup> model = Db.VkGroups.ToList();

            return View(model);
        }

        public ActionResult GroupMembers(int id)
        {
            VkGroup group = Db.VkGroups.Include(x => x.VkGroupMembers).FirstOrDefault(x => x.Id == id);

            return View(group);
        }

        public ActionResult GetAllVkMenFromGroup(int id)
        {
            VkGroup group = Db.VkGroups.FirstOrDefault(x => x.Id == id);

            VkApiWorkerBase vkApi = new VkApiWorkerBase();

            int offset = 0;

            JObject response = vkApi.Request(method: "groups.getMembers", paramsString: $"group_id={group.GroupId}&sort=id_asc&fields=domain&offset={offset}");

            JToken resp = response["response"];

            int count = (int)response["response"]["count"];
            JToken users = response["response"]["users"];

            List<VkMan> vkMen = users.Select(x => x.ToVkMan()).ToList();

            return Redirect($"/VkSpam/GroupMembers/{id}");
        }
    }
}