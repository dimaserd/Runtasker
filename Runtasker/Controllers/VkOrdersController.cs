using Extensions.Json;
using Newtonsoft.Json.Linq;
using Runtasker.Logic;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Data.Entity;
using VkParser.Entities;
using VkParser.Workers.Post;
using VkParser.Workers;
using VkParser.Contexts;
using System;
using VkParser.Models;
using Logic.Extensions.Models;
using VkParser.MessageSenders;
using VkParser.Models.MessageSenderModels;
using VkParser.Constants;

namespace Runtasker.Controllers
{
    [Authorize(Roles = "VkPerformer,Admin")]
    public class VkOrdersController : Controller
    {
        #region Fields
        NewVkPostParseWorker _newVkPostParseWorker;

        VkKeyWordsWorker _keyWordWorker;

        VkGroupWorker _vkGroupWorker;

        VkFoundPostWorker _vkPostWorker;

        /// <summary>
        /// Контекст должен быть одним иначе будут дупликаты в базе
        /// </summary>
        VkParseContext _vkContext = new VkParseContext();

        //MyDbContext _context = new MyDbContext();
        #endregion

        #region Properties

        #region Help Properties
        bool UserHasRightsToBeThere
        {
            get
            {
                return Request.IsAuthenticated && User.IsInRole("Admin");
            }
        }
        #endregion

        NewVkPostParseWorker NewVkPostParseWorker
        {
            get
            {
                if(_newVkPostParseWorker == null)
                {
                    _newVkPostParseWorker = new NewVkPostParseWorker(_vkContext);
                }
                return _newVkPostParseWorker;
            }
        }
        
        VkKeyWordsWorker KeyWordsWorker
        {
            get
            {
                if(_keyWordWorker == null)
                {
                    _keyWordWorker = new VkKeyWordsWorker(_vkContext);
                }

                return _keyWordWorker;
            }
        }

        VkGroupWorker VkGroupWorker
        {
            get
            {
                if(_vkGroupWorker == null)
                {
                    _vkGroupWorker = new VkGroupWorker(_vkContext);
                }
                return _vkGroupWorker;
            }
        }

        VkFoundPostWorker VkPostWorker
        {
            get
            {
                if(_vkPostWorker == null)
                {
                    _vkPostWorker = new VkFoundPostWorker(_vkContext);
                }
                return _vkPostWorker;
            }
        }
        #endregion

        #region Test methods
        public string TestVkMessage()
        {
            using (VkMessageSender sender = new VkMessageSender())
            {
                VkMessage mes = new VkMessage
                {
                    Text = "testText",
                    UserDomain = "dimaserd"
                };
                sender.SendMessageToVkUser(mes);
                return "готово";
            }
        }

        public string TestVkWallMessage()
        {
            using (WallPostWorker poster = new WallPostWorker())
            {
                string text = "test";
                int groupId = 137750954;
                poster.WriteTextOnGroupWall(text, groupId);
                poster.WriteTextOnGroupWallTest();
                return "готово";
            }
        }

        public async Task<ActionResult> Test()
        {
            var model = await _vkContext.VkKeyWords
                .Include(x => x.VkFoundPosts).ToListAsync();
            return View(viewName: "NewIndex", model: model);
        }
        #endregion

        #region GetToken methods
        public ActionResult Token()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult GetToken(string code)
        {
            UpdateVkToken(code);
            return View();
        }

        [AllowAnonymous]
        public ActionResult GetAppToken(string code)
        {
            UpdateVkAppToken(code);
            return View(viewName: "GetToken");
        }
        #endregion

        #region FoundPosts methods
        // GET: VkOrders
        public async Task<ActionResult> Index()
        {            
            using (MyDbContext context = new MyDbContext())
            {
                List<VkFoundPost> model = await context.VkFoundPosts.Include(g => g.InGroup)
                    .OrderByDescending(x => x.PublishDate)
                    .ToListAsync();

                return View(model);
            };
            
        }

        

        #region Refresh Methods
        [HttpGet]
        public async Task<ActionResult> Refresh()
        {
            List<VkGroup> model = await NewVkPostParseWorker.GetUpdateVkGroupsModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Refresh(List<VkGroup> model)
        {
            WorkerResult result = await NewVkPostParseWorker.UpdateSomeGroupsAsync(model);

            return RedirectToAction("Refresh");
        }
        #endregion

        #region Delete methods

        #region Delete Many Posts

        #region DeleteMany Methods
        [HttpGet]
        public ActionResult DeleteMany(string deletion)
        {
            DeleteManyModel model = new DeleteManyModel
            {
                Deletion = deletion
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteMany(DeleteManyModel model)
        {
            WorkerResult result = await VkPostWorker.DeleteManyAsync(model);
            if(result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
        #endregion

        #region Delete Old Posts Methods
        [HttpGet]
        public async Task<ActionResult> DeleteOldPosts(int days = 7)
        {
            DeleteManyModel model = await VkPostWorker.GetDeleteOldPostsModel(days);
            
            return View(viewName: "DeleteMany", model : model);
        }


        public ActionResult DeleteOldPosts(DeleteManyModel model)
        {
            WorkerResult result = VkPostWorker.DeleteMany(model);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View(model);
            
        }
        #endregion

        #endregion

        #region Delete One Post
        [HttpGet]
        public ActionResult DeleteFoundPost(int id)
        {
            VkFoundPost model = VkPostWorker.GetVkPost(id);
            if(model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
        
        [HttpPost]
        public ActionResult DeleteFoundPost(VkFoundPost model)
        {
            WorkerResult result = VkPostWorker.DeletePost(model);
            if (!result.Succeeded)
            {
                foreach(string error in result.ErrorsList)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #endregion

        #endregion

        #region KeyWords Methods
        public ActionResult KeyWords()
        {
            List<VkKeyWord> model = KeyWordsWorker.GetVkKeyWords().ToList();

            return View(model);
        }

        #region Update Methods
        [HttpGet]
        public ActionResult Update(int id)
        {
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
            VkKeyWord model = KeyWordsWorker.GetVkKeyWord(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(VkKeyWord model)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            WorkerResult result = KeyWordsWorker.UpdateKeyWord(model);

            if(!result.Succeeded)
            {
                foreach(string err in  result.ErrorsList)
                {
                    ModelState.AddModelError("", err);
                }
                return View(model);
            }
            return RedirectToAction("KeyWords");
        }
        #endregion

        #region Create Methods
        [HttpGet]
        public ActionResult CreateKeyWord()
        {
            if (!UserHasRightsToBeThere)
            {
                return RedirectToAction("Index");
            }
            return View(new VkKeyWord());
        }

        [HttpPost]
        public async Task<ActionResult> CreateKeyWord(VkKeyWord model)
        {
            if (!UserHasRightsToBeThere)
            {
                return RedirectToAction("Index");
            }
            WorkerResult result = await KeyWordsWorker.CreateKeyWordAsync(model);
            if(!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction("KeyWords");
        }
        #endregion

        #region Delete methods
        [HttpGet]
        public async Task<ActionResult> DeleteKeyWord(int id)
        {
            if(!UserHasRightsToBeThere)
            {
                return RedirectToAction("Index");
            }
            VkKeyWord model = await _vkContext.VkKeyWords
                .FirstOrDefaultAsync(x => x.Id == id);

            return View(model);
        }

        public async Task<ActionResult> DeleteKeyWord(VkKeyWord model)
        {
            if (!UserHasRightsToBeThere)
            {
                return RedirectToAction("Index");
            }
            _vkContext.VkKeyWords.Attach(model);
            _vkContext.VkKeyWords.Remove(model);
            await _vkContext.SaveChangesAsync();

            return RedirectToAction("KeyWords");
        }
        #endregion
        #endregion

        #region Groups Methods
        public ActionResult Groups()
        {
            List<VkGroup> model = VkGroupWorker.GetGroups()
                .OrderBy(x => x.IsMember).ToList();
            return View(model);
        }

        #region AddGroup methods
        [HttpGet]
        public ActionResult AddGroup()
        {
            
            return View(new VkAddGroupModel());
        }

        [HttpPost]
        public ActionResult AddGroup(VkAddGroupModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            WorkerResult result = VkGroupWorker.AddNewGroup(model);
            if(!result.Succeeded)
            {
                foreach(string error in result.ErrorsList)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }

            return RedirectToAction("Groups");
        }
        #endregion

        #endregion

        #region Recovery methods
        [HttpGet]
        public async Task<string> GroupsRecovery(string pass = null)
        {
            if(pass == "566762332")
            {
                await VkGroupWorker.DeleteAllGroupsAndGetFromVk();
            }
            return "готово";
        }
        #endregion

        #region Help Methods

        #region Token methods
        void UpdateVkToken(string code)
        {
            string filesDir = HostingEnvironment.MapPath("~/Files");

            string tokenFilePath = $"{filesDir}/vk_token.txt";
            
            //если файл существует то удаляем его
            if(System.IO.File.Exists(tokenFilePath))
            {
                System.IO.File.Delete(tokenFilePath);
            }

            string fileContents = GetVkToken(code);

            //заполняем новым содержимым
            System.IO.File.AppendAllText(tokenFilePath, fileContents);

        }

        void UpdateVkAppToken(string code)
        {
            string filesDir = HostingEnvironment.MapPath("~/Files");

            string tokenFilePath = $"{filesDir}/vkApp_token.txt";

            //если файл существует то удаляем его
            if (System.IO.File.Exists(tokenFilePath))
            {
                System.IO.File.Delete(tokenFilePath);
            }

            string fileContents = GetVkAppToken(code);
            
            

            //заполняем новым содержимым
            System.IO.File.AppendAllText(tokenFilePath, fileContents);

        }


        

        #region Методы получающие токены через code от сервера
        string GetVkAppToken(string code)
        {
            int client_id = VkConstants.ClientId;
            string client_secret = VkConstants.ClientSecret;

            string redirectUri = @"https://runtasker.ru/VkOrders/GetAppToken";
            string url = @"https://oauth.vk.com/access_token"
                    + $"?client_id={client_id}&client_secret={client_secret}" +
                    $"&redirect_uri={redirectUri}&code={code}";

            JObject response = JsonRequest(url);

            int runtaskerGroupId = VkConstants.RuntaskerGroupId;

            if (!response[$"access_token_{runtaskerGroupId}"].IsNullOrEmpty())
            {
                return response[$"access_token_{runtaskerGroupId}"].ToString();
            }

            return null;

        }


        string GetVkToken(string code)
        {
            int client_id = VkConstants.ClientId;
            string client_secret = VkConstants.ClientSecret;
            string uri = @"https://runtasker.ru/VkOrders/GetToken";

            string resp = string.Empty;

            string url = @"https://oauth.vk.com/access_token"
        + $"?client_id={client_id}&client_secret={client_secret}&redirect_uri={uri}&code={code}";


            JObject response = JsonRequest(url);

            if(!response["access_token"].IsNullOrEmpty())
            {
                return response["access_token"].ToString();                
            }

            return null;
            
        }

        #endregion

        JObject JsonRequest(string url)
        {
            

            string resp = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UseDefaultCredentials = true;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                resp = reader.ReadToEnd();
            }

            JObject json = JObject.Parse(resp);
            return json;
        }

        
        #endregion

        #region AddErrors methods
        public void AddErrors(WorkerResult result)
        {
            foreach(string error in result.ErrorsList)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion

        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IDisposable[] toDisposes = new IDisposable[]
                {
                    //_context,
                    _vkContext,
                    _newVkPostParseWorker, _keyWordWorker,
                    _vkGroupWorker, _vkPostWorker,
                };
                for(int i = 0; i < toDisposes.Length; i++)
                {
                    if(toDisposes[i] != null)
                    {
                        toDisposes[i].Dispose();
                        toDisposes[i] = null;
                    }
                }
                
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}