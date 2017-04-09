using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;
using Runtasker.Logic;
using Runtasker.Logic.Workers.Developer;
using Runtasker.Logic.Workers.Files;
using Runtasker.Settings;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VkParser.Entities;
using VkParser.Workers.Api;

namespace Runtasker.Controllers
{
    [Authorize(Roles = "Admin,Customer,Performer,Dev")]
    public class DevController : Controller
    {
        #region Поля
        DeveloperWorker Dev = new DeveloperWorker();

        MyDbContext context = new MyDbContext();

        UserManager<ApplicationUser> usermanager =
            new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MyDbContext()));
        #endregion

        #region Константы
        const string Password = "566762332";
        #endregion

        #region Private Methods
        bool CheckForPassword(string pass)
        {
            return Password == pass;
        }
        #endregion

        #region Http Методы

        #region Save and Restore Data

        #region Some Constants
        static string filesDir = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
        string restoreDir = $"{filesDir}/Restore";
        #endregion

        public async Task<string> SaveData()
        {
            using (MyDbContext db = new MyDbContext())
            {
                List<VkKeyWord> keyWords = await db.VkKeyWords.ToListAsync();
                List<VkGroup> vkGroups = await db.VkGroups.ToListAsync();

                string vkKeyWordsJSON = Newtonsoft.Json.JsonConvert.SerializeObject(keyWords);
                string vkGroupsJSON = Newtonsoft.Json.JsonConvert.SerializeObject(vkGroups);

                

                //если на сервере не существует директории то создаем ее
                
                if (!System.IO.Directory.Exists(restoreDir))
                {
                    System.IO.Directory.CreateDirectory(restoreDir);
                }


                System.IO.File.AppendAllText(path: $"{restoreDir}/VkGroups.txt", contents: vkGroupsJSON);
                System.IO.File.AppendAllText(path: $"{restoreDir}/VkKeyWords.txt", contents: vkKeyWordsJSON);

            }

            return "скопировано";

        }

        public async Task<string> RestoreData()
        {
            string vkKeyWordsFileContents = System.IO.File.ReadAllText($"{restoreDir}/VkKeyWords.txt");
            List<VkKeyWord> keyWords = Newtonsoft.Json.JsonConvert
                .DeserializeObject<List<VkKeyWord>>(vkKeyWordsFileContents);
            return "готово";
        }

        
        #endregion

        #region Reset Methods
        [AllowAnonymous]
        public string ResetMethod(string pass = null)
        {
            if (CheckForPassword(pass))
            {
                string connection = "LocalTestConnection";

                using (MyDbContext localDb = new MyDbContext(connection))
                {
                    localDb.PaymentTransactions.RemoveRange(localDb.PaymentTransactions);
                    localDb.SaveChanges();

                    localDb.Messages.RemoveRange(localDb.Messages);
                    localDb.SaveChanges();

                    localDb.Notifications.RemoveRange(localDb.Notifications);
                    localDb.SaveChanges();

                    localDb.Payments.RemoveRange(localDb.Payments);
                    localDb.SaveChanges();

                    localDb.Orders.RemoveRange(localDb.Orders);
                    localDb.SaveChanges();

                    List<ApplicationUser> users = localDb.Users.ToList();
                    foreach(ApplicationUser user in users)
                    {
                        localDb.Users.Remove(user);
                    }
                    localDb.SaveChanges();

                    
                }

            }
            return "готово";
        }

        [AllowAnonymous]
        public ActionResult ResetTestUser(string pass)
        {
            if (CheckForPassword(pass))
            {
                string model = Dev.DataBase.ResetTestUser();
                return View(model: model);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Build Methods
        /// <summary>
        /// Метод создает дополнительные информации по каждому
        /// из пользователей
        /// </summary>
        public async Task<string> CheckOtherInfos()
        {
            using (MyDbContext db = new MyDbContext())
            {
                //List<IdentityRole> roles = await db.Roles.ToListAsync();

                List<ApplicationUser> users = await db.Users.ToListAsync();

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                var roles = await roleManager.Roles.ToListAsync();

                foreach(var role in roles)
                {
                    var usersInRole = users
                        .Where(u => u.Roles.Select(r => r.RoleId)
                        .Contains(role.Id)).ToList();

                    foreach(ApplicationUser user in usersInRole)
                    {
                        if(role.Name == "Admin")
                        {
                            //если не существует инфы то создаем ее
                            if(!db.OtherUserInfos.Any(info => info.Id == user.Id))
                            {
                                db.OtherUserInfos.Add(new Logic.Entities.OtherUserInfo
                                {
                                    Id = user.Id,
                                    Specialization = "0,1,2,3,4,5,6,7,8,9",

                                });
                            }
                        }
                        else if(role.Name == "Performer")
                        {
                            if (!db.OtherUserInfos.Any(info => info.Id == user.Id))
                            {
                                db.OtherUserInfos.Add(new Logic.Entities.OtherUserInfo
                                {
                                    Id = user.Id,
                                    Specialization = "0",
                                });
                            }
                        }
                    }

                }

                await db.SaveChangesAsync();
                

                return "готово";
            }
        }

        public string DeleteAllFiles(string pass = null)
        {
            if(CheckForPassword(pass))
            {
                string dirPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
                DirectoryInfo d = new DirectoryInfo(dirPath);
                d.Clear();
                
            }
            return "готово";
            
        }
        #endregion

        #region Методы тестового пользователя
        public async Task<string> AddMoneyToTestUser(int money = 1000)
        {
            ApplicationUser testUser = await usermanager.FindByEmailAsync(DevSettings.TestCustomerEmail);

            if(testUser == null)
            {
                return "Пользователя не существует!";
            }

            testUser.Balance += money;
            await usermanager.UpdateAsync(testUser);

            return "Готово";
        }
        #endregion

        // GET: Dev
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logs(string log = null)
        {
            ViewData["log"] = log;
            string model = "";
            if(log == null)
            {
                log = "";
            }
            if(log.ToLower() == "payments")
            {
                model = Dev.Logs.Payments;
            }
            else if(log.ToLower() == "notifications")
            {
                model = Dev.Logs.Notifications;
            }
            return View(model: model);
        }

        public ActionResult Test()
        {
            return View();
        }
        
        

        public FileResult LoadCurrentState()
        {
            string orders = Dev.DataBase.GetCurrentOrderTableStateInJson();
            return File(Encoding.ASCII.GetBytes(orders), "text/plain", "orders.txt");
        }

        public ActionResult DownloadFile(string filename)
        {
            string rootDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
            string filePath = $"{rootDirectory}/{filename}";

            if(!System.IO.File.Exists(filePath))
            {
                return HttpNotFound();
            }

            return File(filePath, MimeMapping.GetMimeMapping(filePath), filename);
        }
       

        /// <summary>
        /// Метод который удаляет все файлы придумай ему нормальное применение
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        public string DeleteFolders(string pass = "")
        {
            //на данный момент пароль не пройдет
            if(CheckForPassword(pass + "437658o236475"))
            {
                string filesDir = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
                new System.IO.DirectoryInfo($"{filesDir}/Attachments").Clear();
                new System.IO.DirectoryInfo($"{filesDir}/Orders").Clear();
            }
            return "Готово";
        }
        #endregion
    }
}