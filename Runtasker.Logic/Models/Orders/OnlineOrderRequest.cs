using Runtasker.Logic.Entities;
using System;

namespace Runtasker.Logic.Models.Orders
{
    public class OnlineOrderRequest
    {
        public const OrderWorkType OnlineHelpWorkType = OrderWorkType.OnlineHelp;

        public DateTime StartDate { get; set; }

        public Subject Subject { get; set; }

        public string OtherSubject { get; set; }
    }
}
