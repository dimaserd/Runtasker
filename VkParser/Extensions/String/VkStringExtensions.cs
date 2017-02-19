using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkParser.Entities;

namespace VkParser.Extensions.String
{
    public static class VkStringExtensions
    {
        public static string GetLinkToPost(VkFoundPost post)
        {
            string linkToPost = @"https://vk.com/" + $"{post.InGroup.ScreenName}?w=wall-{post.InGroup.GroupId}_{post.PostIdInGroup}";

            if(linkToPost.Contains("public"))
            {

            }

            return linkToPost;
        }
    }
}
