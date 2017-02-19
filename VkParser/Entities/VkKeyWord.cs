using System.Collections.Generic;
using VkParser.Enumerations;

namespace VkParser.Entities
{
    public class VkKeyWord
    {
        public int Id { get; set; }

        public string MainForm { get; set; }

        public string OtherWordForms { get; set; }

        public WordType Subject { get; set; }

        public virtual ICollection<VkFoundPost> VkFoundPosts { get; set; }
    }
}
