using Runtasker.Logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtasker.Logic.Models.VkNotificater
{
    public class VkUserInfo
    {
        public string Name { get; set; }

        public string VkDomain { get; set; }

        public string VkId { get; set; }

        

    }

    #region Extension Methods
    public static class VkUserInfoExtensions
    {
        public static IEnumerable<VkUserInfo> ToVkUserInfoList(this IEnumerable<ApplicationUser> users, IEnumerable<OtherUserInfo> infos)
        {
            List<VkUserInfo> result = new List<VkUserInfo>();

            foreach(ApplicationUser user in users)
            {
                OtherUserInfo info = infos.FirstOrDefault(x => x.Id == user.Id);
                if (info != null)
                {
                    result.Add(new VkUserInfo
                    {
                        Name = user.Name,
                        VkDomain = info.VkDomain,
                        VkId = info.VkId
                    });
                }

                
            }

            return result;
        }
    }
    #endregion
}
