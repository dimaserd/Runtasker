using Logic.Extensions.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using VkParser.Contexts;
using VkParser.Entities;
using VkParser.Enumerations;
using VkParser.Workers.Api;

namespace VkParser.Workers.Post
{
    //класс который работает с базой данных и результатами запросов к 
    //апи вк добавляет новые посты к базе и устанавливает значения 
    //последней проверки в сущности группы
    public class VkPostParseWorker : IDisposable
    {
        #region Constructor
        public VkPostParseWorker()
        {
            Construct();
        }

        void Construct()
        {
            Requester = new VkPostRequestWorker();
            KeyWordChecker = new VkKeyWordsChecker();
        }
        #endregion

        #region Properties
        VkPostRequestWorker Requester { get; set; }

        VkKeyWordsChecker KeyWordChecker { get; set; }
        #endregion

        #region Public Methods

        public async Task<List<VkGroup>> GetUpdateVkGroupsModel()
        {
            using(VkParseContext db = new VkParseContext())
            {
                DateTime dateNow = DateTime.Now;
                int minutesSetting = 15;

                return await (from g in db.VkGroups
                  where SqlFunctions.DateDiff("second", g.LastCheckDate, dateNow) > 60 * minutesSetting
                 select g).ToListAsync();
            }
            
        }

        public void UpdateSomeGroups(List<VkGroup> vkGroups)
        {
            int countOfPostsSetting = 10;
            int countOfPosts = 0;

            List<VkFoundPost> postsToAddToDataBase = new List<VkFoundPost>();

            foreach(VkGroup vkGroup in vkGroups)
            {
                if(vkGroup.IsMember)
                {
                    countOfPosts++;
                    postsToAddToDataBase.AddRange(GetNeededPostsFromGroup(vkGroup));
                }

                if(countOfPosts == countOfPostsSetting)
                {
                    break;
                }
            }
            //сохранение всех постов в базе
            SavePostsChanges(postsToAddToDataBase);
        }

        public async Task<WorkerResult> UpdateSomeGroupsAsync(List<VkGroup> vkGroups)
        {
            
            int countOfPostsSetting = 10;
            int countOfPosts = 0;

            List<VkFoundPost> postsToAddToDataBase = new List<VkFoundPost>();

            foreach (VkGroup vkGroup in vkGroups)
            {
                if (vkGroup.IsMember)
                {
                    countOfPosts++;
                    postsToAddToDataBase.AddRange(await GetNeededPostsFromGroupAsync(vkGroup));
                }

                if (countOfPosts == countOfPostsSetting)
                {
                    break;
                }
            }
            //сохранение всех постов в базе
            return await SavePostsChangesAsync(postsToAddToDataBase);
        }

        #endregion

        #region Help Methods
        //метод должен возвращать посты из группы и производить обновление 
        //в группе
        List<VkFoundPost> GetNeededPostsFromGroup(VkGroup vkGroup)
        {
            List<VkFoundPost> postsFromRequest = Requester.GetHundredPostsFromGroup(vkGroup, 0).ToList();

            //посты которые будут добавлены к базе
            List<VkFoundPost> neededPosts = new List<VkFoundPost>();

            //установка первого поста
            VkFoundPost firstPost = null;

            if(postsFromRequest.Count() > 0)
            {
                firstPost = postsFromRequest.First();
            }

            foreach (VkFoundPost maybeNeededPost in postsFromRequest)
            {
                if (vkGroup.LastCheckedPostId < maybeNeededPost.PostIdInGroup)
                {
                    WordType foundSubjectFromKeyWord;

                    string word = "";

                    if (KeyWordChecker.CheckPostForWords(maybeNeededPost, out foundSubjectFromKeyWord, out word))
                    {
                        //ссылка для целостности в базе
                        maybeNeededPost.VkGroupId = vkGroup.Id;
                        maybeNeededPost.FoundKeyWords = word;
                        //установка идентификатора предмета
                        maybeNeededPost.Subject = foundSubjectFromKeyWord;

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

            SaveGroupChanges(vkGroup, firstPost);

            return neededPosts;
        }

        async Task<List<VkFoundPost>> GetNeededPostsFromGroupAsync(VkGroup vkGroup)
        {
            List<VkFoundPost> postsFromRequest = (await Requester.GetHundredPostsFromGroupAsync(vkGroup, 0)).ToList();

            //посты которые будут добавлены к базе
            List<VkFoundPost> neededPosts = new List<VkFoundPost>();

            //установка первого поста
            VkFoundPost firstPost = null;

            if (postsFromRequest.Count() > 0)
            {
                firstPost = postsFromRequest.First();
            }

            foreach (VkFoundPost maybeNeededPost in postsFromRequest)
            {
                if (vkGroup.LastCheckedPostId < maybeNeededPost.PostIdInGroup)
                {
                    WordType foundSubjectFromKeyWord;

                    string word = "";

                    if (KeyWordChecker.CheckPostForWords(maybeNeededPost, out foundSubjectFromKeyWord, out word))
                    {
                        //ссылка для целостности в базе
                        maybeNeededPost.VkGroupId = vkGroup.Id;
                        maybeNeededPost.FoundKeyWords = word;
                        //установка идентификатора предмета
                        maybeNeededPost.Subject = foundSubjectFromKeyWord;

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

            SaveGroupChanges(vkGroup, firstPost);

            return neededPosts;
        }

        #region SaveChanges Methods
        void SaveGroupChanges(VkGroup vkGroup, VkFoundPost firstPost)
        {
            using (VkParseContext context = new VkParseContext())
            {
                vkGroup.LastCheckDate = DateTime.Now;

                if(firstPost != null)
                {
                    vkGroup.LastCheckedPostId = firstPost.PostIdInGroup;
                }
                
                
                context.Entry(vkGroup).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        async Task<WorkerResult> SaveGroupChangesAsync(VkGroup vkGroup, VkFoundPost firstPost)
        {
            using (VkParseContext context = new VkParseContext())
            {
                vkGroup.LastCheckDate = DateTime.Now;

                if (firstPost != null)
                {
                    vkGroup.LastCheckedPostId = firstPost.PostIdInGroup;
                }


                context.Entry(vkGroup).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return new WorkerResult
                {
                    Succeeded = true
                };
            }
        }


        void SavePostsChanges(List<VkFoundPost> posts)
        {
            using (VkParseContext db = new VkParseContext())
            {
                db.VkFoundPosts.AddRange(posts);
                db.SaveChanges();
            }
        }

        async Task<WorkerResult> SavePostsChangesAsync(List<VkFoundPost> posts)
        {
            using (VkParseContext db = new VkParseContext())
            {
                db.VkFoundPosts.AddRange(posts);
                await db.SaveChangesAsync();

                return new WorkerResult
                {
                    Succeeded = true
                };
            }
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
                    // TODO: освободить управляемое состояние (управляемые объекты).
                    if(Requester != null)
                    {
                        Requester.Dispose();
                    }
                    if(KeyWordChecker != null)
                    {
                        KeyWordChecker.Dispose();
                    }
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.
                Requester = null;
                KeyWordChecker = null;

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~VkPostParseWorker() {
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
