using System.Collections.Generic;

namespace Runtasker.Logic.Models.Orders
{
    public class UserOrdersInfo
    {
        public bool HasOrders { get; set; }

        public int UnreadCount { get; set; }

        public IEnumerable<OrderMessagesInfo> OrderMessageInfos { get; set; }
    }

    public class OrderMessagesInfo
    {
        public ActionLink ActionLink { get; set; }

        public int Id { get; set; }

        public int UnreadCount { get; set; }
    }

    public class ActionLink
    {
        public string Link { get; set; }

        public string FaIconClass { get; set; }

        public string Text { get; set; }

        public bool OpenInModal { get; set; }
    }
}
