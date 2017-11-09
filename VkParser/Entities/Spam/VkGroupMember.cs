namespace VkParser.Entities.Spam
{
    public class VkGroupMember
    {
        public string VkManId { get; set; }

        public virtual VkMan Man { get; set; }

        public string VkGroupId { get; set; }

        public VkGroup Group { get; set; }
    }
    
}
