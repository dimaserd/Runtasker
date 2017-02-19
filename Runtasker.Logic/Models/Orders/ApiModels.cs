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
        public int Id { get; set; }

        public int UnreadCount { get; set; }
    }
}
