using Extensions.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Runtasker.Logic.Models.ManageModels
{
    public class AddVkInfoModel
    {
        [Display(Name = "Ссылка в вк")]
        [Placeholder(text: "https://vk.com/durov")]
        [Tooltip(text: "Скопируйте сюда свою ссылку из вконтакте, например \"https://vk.com/durov\"")]
        public string VkLink { get; set; }

        public bool IsSet { get; set; }

        public string VkDomain { get; set; }

        public string VkId { get; set; }
    }
}
