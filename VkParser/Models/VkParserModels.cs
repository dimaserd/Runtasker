using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VkParser.Entities;

namespace VkParser.Models
{
    public class VkAPIFoundPostModel
    {
        public int Id { get; set; }

        public int FromId { get; set; }

        public int ToId { get; set; }

        public int PostIdInGroup { get; set; }
        //теперь посты не удаляются а становятся не активными
        public bool IsActive { get; set; }

        public DateTime PublishDate { get; set; }

        public string Text { get; set; }

        public string VkLink { get; set; }

        public string PostOwnerId { get; set; }

        public string FoundKeyWords { get; set; }
    }

    public class VkGroupPostsFromAPI
    {
        #region Constructor
        public VkGroupPostsFromAPI()
        {
            Posts = new List<VkAPIFoundPostModel>();
        }
        #endregion

        public int VkGroupId { get; set; }

        public List<VkAPIFoundPostModel> Posts { get; set; }
    }

    public class VkGroupToRefresh
    {
        #region Constructors
        
        #endregion

        public VkGroup vkGroup { get; set; }
        public bool Checked { get; set; }
    }

    public class VkAddGroupModel
    {
        [Required]
        [Display(Name = "Краткое имя группы")]
        public string GroupName { get; set; }
    }


    public class DeleteManyModel
    {
        public string Deletion { get; set; }
    }

    public class VkGroupInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ScreenName { get; set; }

        public bool IsClosed { get; set; }

        public bool IsMember { get; set; }
    }
}
