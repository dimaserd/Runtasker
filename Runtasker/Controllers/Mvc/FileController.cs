using Extensions.String;
using Logic.Extensions.Models;
using Microsoft.AspNet.Identity;
using Runtasker.Controllers.Base;
using Runtasker.Logic;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Workers;
using Runtasker.Logic.Workers.Files;
using Runtasker.Settings.Files;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace Runtasker.Controllers
{

    public class FileController : BaseMvcController
    {
        #region Конструкторы
        public FileController()
        {
            Construct();
        }

        void Construct()
        {
            if(!Directory.Exists(SiteFilesDirectory))
            {
                Directory.CreateDirectory(SiteFilesDirectory);
            }
        }
        #endregion

        #region Поля
        
        AvatarWorker _avatarer;

        FileControllerMethods _filer;

        #endregion

        #region Свойства
        

        FileControllerMethods Filer
        {
            get
            {
                if(_filer == null)
                {
                    _filer = new FileControllerMethods(UserGuid, Db);
                }
                return _filer;
            } 
        }


        #region Directories
        string FilesDir
        {
            get { return System.Web.Hosting.HostingEnvironment.MapPath("~/Files"); }
        }

        string AssetsDir
        {
            get
            {
                return System.Web.Hosting.HostingEnvironment.MapPath("~/assets");
            }
        }

        string SiteFilesDirectory
        {
            get { return $"{FilesDir}/Site"; }
        }
        #endregion

        #endregion

        #region Public Properties

        public AvatarWorker Avatarer
        {
            get
            {
                if (_avatarer == null)
                {
                    _avatarer = new AvatarWorker(UserGuid);
                }
                return _avatarer;
            }
        }
        #endregion

        #region Http обработчики

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Upload()
        {
            string directory = FilesDir + "/Temporary";


            StringBuilder sb = new StringBuilder();
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];


                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = Path.GetFileName(upload.FileName).LeftJustOneDot().RemoveSymbols('&', ',', '!');
                    if (System.IO.File.Exists(directory + "/" + fileName))
                    {
                        List<string> files = Directory.GetFiles(Server.MapPath("~/Files/Temporary")).ToList();

                        fileName = fileName.MakeFileNameUniqueAtList(files);
                    }
                    // сохраняем файл в папку Files в проекте
                    sb.Append(fileName + "&");
                    upload.SaveAs(directory + "/" + fileName);
                }
            }
            string result = sb.ToString();

            return Json(result.Remove(result.LastIndexOf('&')));
        }

        [HttpPost]
        public async Task<JsonResult> UploadFiles()
        {
            List<HttpPostedFileBase> files = new List<HttpPostedFileBase>();

            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];

                files.Add(file);
            }


            using (MyDbContext db = new MyDbContext())
            {
                Attachment at = files.LeftOnlyGoodFiles().GetAttachmentFromFiles();

                if(at == null)
                {
                    return null;
                }

                db.Attachments.Add(at);

                await db.SaveChangesAsync();

                return Json(at.Id);
            }
        }

        #region Методы скачивания
        [HttpGet]
        [Authorize(Roles = "Admin,Performer,Customer")]
        public async Task<ActionResult> Download(string id)
        {
            using (MyDbContext context = new MyDbContext())
            {
                Attachment a = await context.Attachments.FirstOrDefaultAsync(t => t.Id == id);
                if (a == null && a.FileData != null)
                {
                    return new HttpStatusCodeResult(404);
                }

                return File(a.FileData, MimeMapping.GetMimeMapping(a.FileName), a.FileName);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Performer,Customer")]
        public ActionResult DownloadByKey(string key)
        {
            using (MyDbContext context = new MyDbContext())
            {
                Attachment a = context.Attachments.FirstOrDefault(t => t.Id == key);
                if (a == null || !System.IO.File.Exists(a.FilePath))
                {
                    return new HttpStatusCodeResult(404);
                }

                return File(a.FilePath, MimeMapping.GetMimeMapping(a.FilePath), a.FileName);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Performer,Customer")]
        public async Task<ActionResult> GetCustomerFiles(int orderId)
        {
            using (MyDbContext context = new MyDbContext())
            {
                Attachment a = await context.Attachments.FirstOrDefaultAsync(t => t.OrderId == orderId);
                if (a == null && a.FileData != null)
                {
                    return new HttpStatusCodeResult(404);
                }

                return File(a.FileData, MimeMapping.GetMimeMapping(a.FileName), a.FileName);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Performer,Customer")]
        public ActionResult DownloadSolution(int id)
        {

            WorkerResult result; 

            

            if (Request.IsAuthenticated && User.IsInRole("Admin"))
            {
                result = new WorkerResult
                {
                    Succeeded = true
                };
            }
            else
            {
                result = Filer.GetDownloadSolutionResult(id);
            }

            if (!result.Succeeded)
            {
                return RedirectToAction("Index", "Orders");
            }

            Attachment solution = Filer.GetOrderSolutionAttachment(id);

            if(solution == null)
            {
                return HttpNotFound();
            }

            return File(solution.FileData, MimeMapping.GetMimeMapping(solution.FileName), solution.FileName);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetFileFromAssets(string path)
        {
            string filePath = $"{AssetsDir}/{path}";
            string fileName = path.Split(separator: new string[] { "/" }, options: StringSplitOptions.RemoveEmptyEntries).Last();
            if(System.IO.File.Exists(filePath))
            {
                return File(filePath, MimeMapping.GetMimeMapping(filePath), fileName);

            }
            
            return new HttpStatusCodeResult(404);
        }
        #endregion

        

       

        #region Avatars Methods
        [Authorize]
        public FileResult GetAvatar(string userGuid)
        {
            string filePath = Avatarer.GetAvatarPathByGuid(userGuid);

            string ext = Path.GetExtension(filePath);

            string filename = $"avatar.{ext}";

            return File(filePath, MimeMapping.GetMimeMapping(filePath), filename);
        }


        [Authorize]
        public FileResult GetMyAvatar()
        {
            string filePath = Avatarer.GetAvatarPath();

            string ext = Path.GetExtension(filePath);

            string filename = $"myAvatar.{ext}";

            return File(filePath, MimeMapping.GetMimeMapping(filePath), filename);
        }

        [HttpPost]
        [Authorize]
        public JsonResult UploadAvatar(HttpPostedFileBase file)
        {
            Avatarer.ChangeAvatar(file);
            //Sending Image Source
            return Json("Uploaded");
        }

        #endregion
        
        
        [HttpGet]
        public ActionResult GetFileToSite(string filename)
        {
            string filePath = $"{SiteFilesDirectory}/{filename}";
            if (System.IO.File.Exists(filePath))
            {
                return File(filePath, MimeMapping.GetMimeMapping(filePath), filename);
            }

            return new HttpStatusCodeResult(404);
        }

        [HttpGet]
        [AllowAnonymous]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ActionResult GetFileFromSite(string filename)
        {
            string filePath = $"{SiteFilesDirectory}/{filename}";
            if (System.IO.File.Exists(filePath))
            {
                return File(filePath, MimeMapping.GetMimeMapping(filePath), filename);
            }

            return new HttpStatusCodeResult(404);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            
            if(_filer != null)
            {
                _filer.Dispose();
                _filer = null;
            }

            base.Dispose(disposing);
        }
    }
}