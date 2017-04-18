using Extensions.Decimal;
using HtmlExtensions.StaticRenderers;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models.Orders;
using Runtasker.Resources.Actions.OrderActions;

namespace Runtasker.Statics.Actions
{
    public static class CustomerActions
    {
        public static ActionLink GetActionLinkFromOrder(Order order)
        {
            switch (order.Status)
            {
                case OrderStatus.New:
                    return null;

                case OrderStatus.Estimated:
                    //выбираем сумму
                    string sumToPayString = (order.WorkType != OrderWorkType.OnlineHelp) ? (order.Sum / 2).ToMoney() : (order.Sum).ToMoney();

                    if (order.WorkType != OrderWorkType.OnlineHelp)
                    {
                        return new ActionLink
                        {
                            Link = $"/Orders/PayHalf/{order.Id}",
                            Text = string.Format(OrderActionsRes.PayFormat, sumToPayString, HtmlSigns.Rouble)
                        };
                    }
                    else
                    {
                        return new ActionLink
                        {
                            Link = $"/Orders/PayOnlineHelp/{order.Id}",
                            Text = string.Format(OrderActionsRes.PayFormat, sumToPayString, HtmlSigns.Rouble)
                        };
                    }



                case OrderStatus.Finished:
                    return new ActionLink
                    {
                        Link = $"/Orders/PayAnotherHalf/{order.Id}",
                        Text = string.Format(OrderActionsRes.PayFormat, (order.Sum / 2).ToMoney(), HtmlSigns.Rouble)
                    };

                case OrderStatus.FullPaid:
                    return new ActionLink
                    {
                        Link = $"/Orders/DownloadSolution/{order.Id}",
                        Text = string.Format(OrderActionsRes.DownloadSolutionFormat, order.Id)
                    };

                default: return null;

            }
        }
    }
}