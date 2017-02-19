using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VkParser.Entities
{
    public class VkPostLookUp
    {
        [Key]
        public int Id { get; set; }

        public int Count { get; set; }
        
        [Required]
        [ForeignKey("VkPost")]
        public int VkFoundPostId { get; set; }
        [JsonIgnore]
        public virtual VkFoundPost VkPost { get; set; }


        [Required]
        //[StringLength(128)]
        //[ForeignKey("VkPerformer")]
        public string VkPerformerGuid { get; set; }
        //[JsonIgnore]
        //public virtual ApplicationUser VkPerformer { get; set; }
    }
}
