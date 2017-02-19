﻿using Logic.Extensions.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using VkParser.Contexts;
using VkParser.Entities;
using VkParser.Models;
using VkParser.Repositories;
using VkParser.Workers.Base;

namespace VkParser.Workers
{
    public class VkFoundPostWorker : BaseVkParseContextWorker
    {
        #region Constructors
        public VkFoundPostWorker(VkParseContext context) : base(context)
        {
            Construct();
        }

        void Construct()
        {
            Repository = new VkFoundPostRepository(Context);           
        }
        #endregion

        #region Properties
        VkFoundPostRepository Repository { get; set; }
        #endregion

        #region Public Methods
        

        public VkFoundPost GetVkPost(int id)
        {
            return Context.VkFoundPosts.FirstOrDefault(p => p.Id == id);
        }

        public WorkerResult DeletePost(VkFoundPost model)
        {
            VkFoundPost postToDelete = Context.VkFoundPosts.FirstOrDefault(p => p.Id == model.Id);

            if(postToDelete == null)
            {
                return new WorkerResult("Пост не найден!");
            }

            Context.VkFoundPosts.Remove(postToDelete);
            Context.SaveChanges();

            return new WorkerResult
            {
                Succeeded = true
            };
        }

        public WorkerResult DeleteMany(DeleteManyModel model)
        {
            if(string.IsNullOrEmpty(model.Deletion))
            {
                return new WorkerResult
                {
                    Succeeded = true
                };
            }

            string[] separators = new string[] { "." };
            List<string> ids = model.Deletion.Split(separator: separators, options: StringSplitOptions.None).ToList();

            foreach (string id in ids)
            {
                int parsedId;
                if(int.TryParse(id, out parsedId))
                {
                    VkFoundPost post = new VkFoundPost() { Id = parsedId };
                    Context.VkFoundPosts.Attach(post);
                    Context.VkFoundPosts.Remove(post);
                }
                
            }
            Context.SaveChanges();

            return new WorkerResult
            {
                Succeeded = true
            };
        }

        public async Task<WorkerResult> DeleteManyAsync(DeleteManyModel model)
        {
            if (string.IsNullOrEmpty(model.Deletion))
            {
                return new WorkerResult
                {
                    Succeeded = true
                };
            }

            string[] separators = new string[] { "." };
            List<string> ids = model.Deletion.Split(separator: separators, options: StringSplitOptions.None).ToList();

            foreach (string id in ids)
            {
                int parsedId;
                if (int.TryParse(id, out parsedId))
                {
                    VkFoundPost post = new VkFoundPost() { Id = parsedId };
                    Context.VkFoundPosts.Attach(post);
                    Context.VkFoundPosts.Remove(post);
                }

            }
            await Context.SaveChangesAsync();

            return new WorkerResult
            {
                Succeeded = true
            };
        }


        public async Task<DeleteManyModel> GetDeleteOldPostsModel(int days)
        {
            DateTime dateNow = DateTime.Now;

       

            List<VkFoundPost> oldPosts = await (from p in Context.VkFoundPosts
                         where SqlFunctions.DateDiff("day", p.PublishDate, dateNow) > days
                         select p).ToListAsync();

            string result = string.Empty;

            foreach(VkFoundPost post in oldPosts)
            {
                result += $"{post.Id}.";
            }

            return new DeleteManyModel
            {
                Deletion = result
            };
        }
        #endregion

        #region Overridden Methods
        protected override void Dispose(bool disposing)
        {
            Repository.Dispose(disposing);

            base.Dispose(disposing);
        }
        #endregion
    }
}
