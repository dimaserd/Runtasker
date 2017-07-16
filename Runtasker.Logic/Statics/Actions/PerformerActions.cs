using Runtasker.Logic.Entities;
using Runtasker.Logic.Models.Orders;
using Runtasker.Resources.Actions.OrderActions;

namespace Runtasker.Logic.Statics.Actions
{
    public static class AdminActions
    {
        public static ActionLink GetActionLinkFromOrder(Order order)
        {
            switch (order.Status)
            {
                case OrderStatus.New:
                    return new ActionLink
                    {
                        Link = $"/Performer/ValueOrder/{order.Id}",
                        Text = AddOrderBracketsInfo("Оценить стоимость", order)
                    };

                //исправления ошибок
                case OrderStatus.HasError:
                     return null;
                    
                
                #region Оплата
                case OrderStatus.Estimated:
                    return null;



                case OrderStatus.Finished:
                    return null;


                case OrderStatus.HalfPaid:
                    return new ActionLink
                    {
                        Link = $"/Performer/Solve/{order.Id}",
                        Text = AddOrderBracketsInfo("Решить", order)
                    };
                #endregion

                case OrderStatus.FullPaid:
                    if (order.WorkType == OrderWorkType.OnlineHelp)
                    {
                        return new ActionLink
                        {
                            Link = $"#",
                            Text = AddOrderBracketsInfo($"Онлайн ({order.FinishDate.ToString("G")})", order)
                        };
                    }
                    else
                    {
                        return null;
                    }

                case OrderStatus.Downloaded:
                    return null;
                    

                default: return null;

            }
        }

        /// <summary>
        /// Возвращает строку содержащую название действия и в скобках номер заказа.
        /// Например Оплатить 300Р (заказ №31)
        /// </summary>
        /// <param name="actionInfo"></param>
        /// <param name="order"></param>
        /// <returns></returns>
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