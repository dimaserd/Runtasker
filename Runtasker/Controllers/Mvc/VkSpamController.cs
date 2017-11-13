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
using System.Threading;

namespace Runtasker.Controllers.Mvc
{
    public class VkSpamController : BaseMvcController
    {
        public VkSpamController()
        {

        }

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

        public ActionResult GroupMembers(int id, bool searchMore = false, int offsetParam = 0)
        {
            VkGroup group = Db.VkGroups.Include(x => x.VkGroupMembers).FirstOrDefault(x => x.Id == id);

            ViewData["searchMore"] = searchMore;
            ViewData["offsetParam"] = offsetParam;

            return View(group);
        }

        public ActionResult VkMenList()
        {
            List<VkMan> vkMen = Db.VkMen.ToList();

            return View(vkMen);
        }

        public ActionResult GetAllVkMenFromGroup(int id, int offset = 0)
        {
            VkGroup group = Db.VkGroups.FirstOrDefault(x => x.Id == id);

            VkApiWorkerBase vkApi = new VkApiWorkerBase();
            
            JObject response = vkApi.Request(method: "groups.getMembers", paramsString: $"group_id={group.GroupId}&sort=id_asc&fields=domain&offset={offset}");

            int count = (int)response["response"]["count"];
            JToken users = response["response"]["users"];

            List<VkMan> vkMenFromApi = users.Select(x => x.ToVkMan()).ToList();

            
            foreach(VkMan vkMan in vkMenFromApi)
            {
                if (!VkMen.Any(x => x.VkId == vkMan.VkId))
                {
                    Db.VkMen.Add(vkMan);
                }
            }
            Db.SaveChanges();

            return Redirect($"/VkSpam/GroupMembers?id={id}&searchMore={(count > offset).ToString().ToLower()}&offsetParam={offset + 1000}");
        }
    }
}