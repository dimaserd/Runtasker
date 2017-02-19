namespace VkParser.Models.MessageSenderModels
{
    public class VkMessage
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        /// <summary>
        /// короткий адрес пользователя (например, illarionov).
        /// </summary>
        public string UserDomain { get; set; }

        public string Text { get; set; }
    }
}
