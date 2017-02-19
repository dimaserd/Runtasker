using VkParser.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using VkParser.Contexts;
using Logic.Extensions.Models;
using VkParser.PostFinders;
using VkParser.Models;
using VkParser.Enumerations;
using VkParser.Extensions;
using VkParser.Workers.KeyWords;

namespace VkParser.Workers.Post
{
    //сделай одно сохранение групп для можно это сделать через лист
    //и один контекст сэйв чейнджз
    public class NewVkPostParseWorker : IDisposable
    {
        #region Constructor
        public NewVkPostParseWorker(VkParseContext db)
        {
            Db = db;
            Construct();
        }

        void Construct()
        {

            NewKeyWordsChecker = new NewVkKeyWordsChecker(Db.VkKeyWords.ToList());


            KeyWordChecker = new VkKeyWordsChecker();
            UpdatedGroups = new List<VkGroup>();
        }
        #endregion

        #region Properties
        VkParseContext Db { get; set; }

        NewVkKeyWordsChecker NewKeyWordsChecker { get; set; }

        VkKeyWordsChecker KeyWordChecker { get; set; }

        List<VkGroup> UpdatedGroups { get; set; }
        #endregion

        #region Public Methods
        public async Task<List<VkGroup>> GetUpdateVkGroupsModel()
        {

            DateTime dateNow = DateTime.Now;
            int minutesSetting = 15;

            return await (from g in Db.VkGroups
                          where SqlFunctions.DateDiff("second", g.LastCheckDate, dateNow) > 60 * minutesSetting
                          where g.IsMember == true
                          select g).ToListAsync();


        }

        public async Task<WorkerResult> UpdateSomeGroupsAsync(List<VkGroup> vkGroups)
        {
            //по этой переменной определяется сколько постов будет взято из апи
            int countSetting = 23;

            VkPostFinder finder = new VkPostFinder();
            //берем столько групп сколько нужно для запроса
            List<VkGroup> groupsToUpdate = vkGroups.Take(countSetting).ToList();
            //находим их с помощью поисковика
            List<VkGroupPostsFromAPI> groupPosts = finder.FindMethod(groupsToUpdate);

            IEnumerable<VkGroupPostsFromAPI> normalizedModel = Normalize(groupPosts, groupsToUpdate).ToList();

            List<VkFoundPost> postsToAddToDataBase = new List<VkFoundPost>();

            foreach (VkGroup vkGroup in groupsToUpdate)
            {
                VkGroupPostsFromAPI postsAPI = normalizedModel
                    .FirstOrDefault(g => g.VkGroupId == vkGroup.Id);

                if (postsAPI == null)
                {
                    //данные в группе должны сохраняться в любом случае
                    SaveGroupChanges(vkGroup, null);
                    continue;
                }
                List<VkFoundPost> neededPosts = GetNeededPostsFromGroup(vkGroup, postsAPI).ToList();

                //добавление найденных постов
                postsToAddToDataBase.AddRange(neededPosts);
            }

            //сохранение всех постов в базе
            return await SaveAllChangesAsync(postsToAddToDataBase);
        }
        #endregion

        #region Help Methods
        IEnumerable<VkGroupPostsFromAPI> Normalize(List<VkGroupPostsFromAPI> groupPosts, List<VkGroup> vkGroups)
        {


            return groupPosts.Select(
                x =>
                {
                    //проверка
                    if (x.Posts.Count == 0)
                    {
                        return x;
                    }

                    VkGroup vkGroup = vkGroups.FirstOrDefault(o => o.GroupId == -x.Posts.First().ToId);
                    x.VkGroupId = vkGroup.Id;
                    return x;
                });

        }

        /// <summary>
        /// этот метод возвращает только те посты которые нужно добавить
        /// в базу так как в них были обнаружены какие то ключевые слова
        /// </summary>
        IEnumerable<VkFoundPost> GetNeededPostsFromGroup(VkGroup vkGroup, VkGroupPostsFromAPI postsAPI)
        {
            List<VkFoundPost> posts = postsAPI.Posts.Select(x => x.ToVkFoundPost()).ToList();

            //посты которые будут добавлены к базе
            List<VkFoundPost> neededPosts = new List<VkFoundPost>();

            //установка первого поста
            VkFoundPost firstPost = null;

            if (posts.Count() > 0)
            {
                firstPost = posts.First();
            }

            foreach (VkFoundPost maybeNeededPost in posts)
            {
                if (vkGroup.LastCheckedPostId < maybeNeededPost.PostIdInGroup)
                {
                    if (NewKeyWordsChecker.CheckPostForKeyWords(maybeNeededPost))
                    {
                        //ссылка для целостности в базе
                        maybeNeededPost.VkGroupId = vkGroup.Id;
                        
                        
                        //в дальнейшем нужно выносить четкий вердикт посту
                        //по найденным в нем ключевым словам
                        maybeNeededPost.Subject = maybeNeededPost.VkKeyWords.First().Subject;

                        neededPosts.Add(maybeNeededPost);
                    }

                }
                else
                {
                    //как только мы встречаем пост который имеет тот же айдишник
                    //что и последний прочеканный что сидит в группе
                    //значит на этом нужно закончить поиски
                    break;
                }
            }
            //нужно заменить
            SaveGroupChanges(vkGroup, firstPost);

            return neededPosts;

        }

        #region SaveChanges Methods
        /// <summary>
        /// Ничего не записывает в базу просто пишет в лист
        /// чтобы в конце все решить в один запрос
        /// </summary>
        /// <param name="vkGroup"></param>
        /// <param name="firstPost">параметр мой</param>
        void SaveGroupChanges(VkGroup vkGroup, VkFoundPost firstPost)
        {
            //все добавляем в один список чтобы сэкономить на запросах
            //в базу данных
            vkGroup.LastCheckDate = DateTime.Now;
            if (firstPost != null)
            {
                vkGroup.LastCheckedPostId = firstPost.PostIdInGroup;
            }

            UpdatedGroups.Add(vkGroup);

        }


        /// <summary>
        /// Сохраняет все изменения в базе данных как по группам, так и по постам
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        async Task<WorkerResult> SaveAllChangesAsync(List<VkFoundPost> posts)
        {

            Db.VkFoundPosts.AddRange(posts);
            foreach (VkGroup vkGroup in UpdatedGroups)
            {
                // Указать, что запись изменилась
                Db.VkGroups.Attach(vkGroup);
                Db.Entry(vkGroup)
                .Property(c => c.LastCheckDate).IsModified = true;
                Db.Entry(vkGroup).Property(c => c.LastCheckedPostId).IsModified = true;
            }

            await Db.SaveChangesAsync();

            return new WorkerResult
            {
                Succeeded = true
            };
        }



        #endregion

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        public virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    IDisposable[] toDisposes = new IDisposable[]
                    {
                        KeyWordChecker, Db
                    };
                    for(int i = 0; i < toDisposes.Length; i++)
                    {
                        if (toDisposes[i] != null)
                        {
                            toDisposes[i].Dispose();
                            toDisposes[i] = null;
                        }
                    }
                    
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~NewVkPostParseWorker() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        void IDisposable.Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
