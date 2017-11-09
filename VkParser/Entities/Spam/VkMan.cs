using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkParser.Entities.Spam
{
    public class VkMan
    {
        #region КОнструкторы
        public VkMan()
        {
            VkGroupMembers = new List<VkGroupMember>();
        }
        #endregion

        public string Id { get; set; }

        public string VkLink { get; set; }
        
        public bool IsInformed { get; set; }

        #region Свойства отношений

        public virtual ICollection<VkGroupMember> VkGroupMembers { get; set; }
        #endregion
    }
}
