using Newtonsoft.Json.Linq;
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

        public int VkId { get; set; }

        public string VkLink { get; set; }
        
        public bool IsInformed { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        #region Свойства отношений
        public virtual ICollection<VkGroupMember> VkGroupMembers { get; set; }
        #endregion
    }

    public static class VkManExtensions
    {
        public static VkMan ToVkMan(this JToken model)
        {
            return new VkMan
            {
                Id = Guid.NewGuid().ToString(),
                VkId = (int)model["uid"],
                FirstName = model["first_name"].ToString(),
                LastName = model["last_name"].ToString(),
                IsInformed = false,
                VkLink = "https://vk.com/" + model["domain"]
            };
        }
    }
}
