using System.Collections.Generic;

namespace Runtasker.Logic.Models
{
    public class MessageIndexViewModel
    {
        public IEnumerable<int> AboutOrderChatIds { get; set; }

        public IEnumerable<string> ChatGuids { get; set; }
    }

    public class PanelViewModel
    {
        public int InboxCount { get; set; }

        public IEnumerable<OrderChatLink> OrderChatLinks { get; set; }
    }

    public class OrderChatLink
    {
        public int OrderId { get; set; }

        public int UnreadMessages { get; set; }
    }
}
