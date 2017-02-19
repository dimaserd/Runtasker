using Logic.Extensions.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using VkParser.Contexts;
using VkParser.Entities;
using VkParser.Repositories.Base;

namespace Runtasker.Logic.Repositories
{
    public class VkGroupRepository : BaseVkRepository, IRepository<VkGroup>
    {
        #region Constructors
        public VkGroupRepository(VkParseContext context) : base(context) { }
        #endregion

        #region IRepository Methods
        public void Add(VkGroup item)
        {
            db.VkGroups.Add(item);
        }

        public void Remove(int id)
        {
            VkGroup group = db.VkGroups.Find(id);

            if(group != null)
            {
                db.VkGroups.Remove(group);
            }
        }

        public VkGroup GetItem(int id)
        {
            return db.VkGroups.Find(id);
        }

        public IEnumerable<VkGroup> GetList()
        {
            return db.VkGroups;
        }

        public void Update(VkGroup group)
        {
            db.Entry(group).State = EntityState.Modified;
        }
        #endregion
    }
}
