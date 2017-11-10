using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VkParser.Entities.Spam;
using VkParser.Models;

namespace VkParser.Entities
{
    public class VkGroup
    {
        #region Constructors
        public VkGroup()
        {
            Posts = new List<VkFoundPost>();
            VkGroupMembers = new List<VkGroupMember>();
        }
        #endregion

        [Key]
        public int Id { get; set; }

        public string ScreenName { get; set; }

        public string Name { get; set; }

        public DateTime LastCheckDate { get; set; }

        public int LastCheckedPostId { get; set; }

        public int GroupId { get; set; }

        //может быть отправлена заявка на участие в сообществе
        public bool IsMember { get; set; }


        public virtual ICollection<VkFoundPost> Posts { get; set; }

        public virtual ICollection<VkGroupMember> VkGroupMembers { get; set; }
        
    }

    public static class VkGroupExtensions
    {
        public static VkGroup ToVkGroup(this VkGroupInfo info)
        {
            return new VkGroup
            {
                Id = 0,
                GroupId = info.Id,
                ScreenName = info.ScreenName,
                IsMember = info.IsMember,
                Name = info.Name,
                LastCheckDate = DateTime.Now,

                LastCheckedPostId = 0
            };
        }
    }
}
