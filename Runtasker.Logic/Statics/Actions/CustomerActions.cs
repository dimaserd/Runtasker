using Extensions.Decimal;
using HtmlExtensions.StaticRenderers;
using Runtasker.Logic.Entities;
using Runtasker.Logic.Models.Orders;
using Runtasker.Resources.Actions.OrderActions;

namespace Runtasker.Logic.Statics.Actions
{
    public static class CustomerActions
    {
        public static ActionLink GetActionLinkFromOrder(Order order)
        {
            switch (order.Status)
            {
                case OrderStatus.New:
                    return null;

                //исправления ошибок
                case OrderStatus.HasError:
                    switch(order.ErrorType)
                    {
                        case OrderErrorType.NeedDescription:
                            return new ActionLink
                            {
                                Link = $"/Orders/AddDescription/{order.Id}",
                                Text = AddOrderBracketsInfo(OrderActionsRes.AddDescription, order)
                            };

                        case OrderErrorType.NeedFiles:
                            return new ActionLink
                            {
                                Link = $"/Orders/AddFiles/{order.Id}",
                                Text = AddOrderBracketsInfo(OrderActionsRes.AddFiles, order)
                            };

                        default:
                            return null;
                    }

                #region Оплата
                case OrderStatus.Estimated:
                    //выбираем сумму
                    string sumToPayString = (order.WorkType != OrderWorkType.OnlineHelp) ? (order.Sum / 2).ToMoney() : (order.Sum).ToMoney();

                    if (order.WorkType != OrderWorkType.OnlineHelp)
                    {
                        return new ActionLink
                        {
                            Link = $"/Orders/PayHalf/{order.Id}",
                            Text = AddOrderBracketsInfo(string.Format(OrderActionsRes.PayFormat, sumToPayString, HtmlSigns.Rouble), order)
                        };
                    }
                    else
                    {
                        //посмотреть как будет работать
                        return new ActionLink
                        {
                            Link = $"/Orders/PayOnlineHelp/{order.Id}",
                            //OpenInModal = true,
                            Text = AddOrderBracketsInfo(string.Format(OrderActionsRes.PayFormat, sumToPayString, HtmlSigns.Rouble), order)
                        };
                    }



                case OrderStatus.Finished:
                    return new ActionLink
                    {
                        Link = $"/Orders/PayAnotherHalf/{order.Id}",
                        Text = AddOrderBracketsInfo(string.Format(OrderActionsRes.PayFormat, (order.Sum / 2).ToMoney(), HtmlSigns.Rouble), order)
                    };
                #endregion

                case OrderStatus.FullPaid:
                    if (order.WorkType != OrderWorkType.OnlineHelp)
                    {
                        return new ActionLink
                        {
                            Link = $"/Orders/DownloadSolution/{order.Id}",
                            Text = string.Format(OrderActionsRes.DownloadSolutionFormat, order.Id)
                        };
                    }
                    else
                    {
                        return null;
                    }

                case OrderStatus.Downloaded:
                    return new ActionLink
                    {
                        Link = $"/Orders/Rating/{order.Id}",
                        OpenInModal = true,
                        Text = string.Format(OrderActionsRes.RatingFormat, order.Id)
                    };
                    

                default: return null;

            }
        }


        static string AddOrderBracketsInfo(string actionInfo, Order order)
        {
            if(order.WorkType != OrderWorkType.OnlineHelp)
            {
                return $"{actionInfo} ({string.Format(OrderActionsRes.OrderFormat, order.Id)})";
            }
            else
            {
                return $"{actionInfo} ({string.Format(OrderActionsRes.OnlineHelpFormat, order.Id)})";
            }
            
        }
    }
}