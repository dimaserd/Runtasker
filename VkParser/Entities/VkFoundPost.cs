using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VkParser.Enumerations;

namespace VkParser.Entities
{
    public class VkFoundPost
    {
        #region Constructor
        public VkFoundPost()
        {
            LookUps = new List<VkPostLookUp>();
        }
        #endregion

        public int Id { get; set; }

        public int PostIdInGroup { get; set; }
        //теперь посты не удаляются а становятся не активными
        public bool IsActive { get; set; }

        public DateTime PublishDate { get; set; }

        [Required]
        [ForeignKey("InGroup")]
        public int VkGroupId { get; set; }
        [JsonIgnore]
        public virtual VkGroup InGroup { get; set; }

        public string Text { get; set; }

        public string VkLink { get; set; }

        public string PostOwnerId { get; set; }

        public string FoundKeyWords { get; set; }

        /// <summary>
        /// Далее убери его и замени на что из вк
        /// </summary>
        public WordType Subject { get; set; }

        #region Virtual Collections
        public virtual ICollection<VkPostLookUp> LookUps { get; set; }

        public virtual ICollection<VkKeyWord> VkKeyWords { get; set; }
        #endregion
    }
}
