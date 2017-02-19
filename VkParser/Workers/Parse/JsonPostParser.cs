using Extensions.Date;
using Extensions.Json;
using Newtonsoft.Json.Linq;
using System;
using VkParser.Models;

namespace VkParser.Workers.Parse
{
    public class JsonPostParser
    {
        public VkAPIFoundPostModel GetPostFromJSON(JToken post)
        {
            if (post.Type == JTokenType.Object)
            {

                if (!post["is_pinned"].IsNullOrEmpty())
                {
                    return null;
                }

                DateTime date = int.Parse(post["date"].ToString()).UNIXTimeToDateTime();

                //проверка на новенькое
                if(post["from_id"].IsNullOrEmpty() || post["to_id"].IsNullOrEmpty())
                {
                    //ошибку выкинет
                }

                return new VkAPIFoundPostModel
                {
                    FromId = int.Parse(post["from_id"].ToString()),
                    ToId = int.Parse(post["to_id"].ToString()),
                    Id = int.Parse(post["id"].ToString()),
                    PostIdInGroup = int.Parse(post["id"].ToString()),
                    PublishDate = date,
                    Text = post["text"].ToString(),
                    FoundKeyWords = "some key words",
                    PostOwnerId = post["signer_id"].IsNullOrEmpty() ? "" : post["signer_id"].ToString(),
                    VkLink = "someLink"
                };
            }
            return null;
        }
    }
}
