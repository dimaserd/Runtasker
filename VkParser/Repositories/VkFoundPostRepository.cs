using Logic.Extensions.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using VkParser.Contexts;
using VkParser.Entities;
using VkParser.Repositories.Base;

namespace VkParser.Repositories
{
    public class VkFoundPostRepository : BaseVkRepository, IRepository<VkFoundPost>, IRepositoryExtensive<VkFoundPost>
    {
        #region Constructors
        public VkFoundPostRepository(VkParseContext context) : base(context) { }

        
        #endregion

        #region IRepostory Methods
        public void Add(VkFoundPost item)
        {
            db.VkFoundPosts.Add(item);
        }

        public void Remove(int id)
        {
            VkFoundPost post = db.VkFoundPosts.Find(id);
            if (post != null)
            {
                db.VkFoundPosts.Remove(post);
            }   
        }

        public VkFoundPost GetItem(int id)
        {
            return db.VkFoundPosts.Find(id);
        }

        public IEnumerable<VkFoundPost> GetList()
        {
            return db.VkFoundPosts;
        }

        public void Update(VkFoundPost post)
        {
            db.Entry(post).State = EntityState.Modified;
        }
        #endregion

        #region IRepositoryExtensive Methods
        public void AddRange(IEnumerable<VkFoundPost> items)
        {
            db.VkFoundPosts.AddRange(items);
        }

        public void RemoveRange(IEnumerable<VkFoundPost> items)
        {
            db.VkFoundPosts.RemoveRange(items);
        }
        #endregion
    }
}
