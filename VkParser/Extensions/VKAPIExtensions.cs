using VkParser.Entities;
using VkParser.Models;

namespace VkParser.Extensions
{
    public static class VKAPIExtensions
    {
        public static VkFoundPost ToVkFoundPost(this VkAPIFoundPostModel model)
        {
            return new VkFoundPost
            {
                Id = model.Id,
                FoundKeyWords = model.FoundKeyWords,
                IsActive = model.IsActive,
                PostIdInGroup = model.PostIdInGroup,
                PostOwnerId = model.PostOwnerId,
                Text = model.Text,
                PublishDate = model.PublishDate,
                VkLink = model.VkLink,

            };
        }
    }
}
