using Extensions.Decimal;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Email.OrderPerformer;

namespace Runtasker.LocaleBuilders.Email.CallToAction
{
    public class PerformerEmailCallToActionModelBuilder : UICultureSwitcher
    {

        public ForEmailCallToAction EstimatedOrder(string customerName, int orderId, decimal orderSum, string roubleSign)
        {
            switch(UICultureName)
            {
                case "ru-RU":
                    return new ForEmailCallToAction
                    {
                        Header = $"{PerformerOrderEmRes.Greeting} {customerName}!",
                        BigText = $"{PerformerOrderEmRes.Dear} {customerName}, {PerformerOrderEmRes.EstimatedText1}{orderId} " +
                         $"{PerformerOrderEmRes.EstimatedText2} {orderSum.ToMoney()}{roubleSign}. " +
                         $"{PerformerOrderEmRes.EstimatedText3} {PerformerOrderEmRes.RuntaskerWish}",
                        ActionBtnText = PerformerOrderEmRes.EstimatedBtnText
                    };
                    //english checked
                default:
                    return new ForEmailCallToAction
                    {
                        Header = $"{PerformerOrderEmRes.Greeting} {customerName}!",
                        BigText = $"{PerformerOrderEmRes.Dear} {customerName}, " +
                        $"{PerformerOrderEmRes.EstimatedText1}{orderId} " +
                        $"{PerformerOrderEmRes.EstimatedText2} {orderSum.ToMoney()}{roubleSign}. " +
                        $"{PerformerOrderEmRes.EstimatedText3} {PerformerOrderEmRes.RuntaskerWish}",
                        ActionBtnText = PerformerOrderEmRes.EstimatedBtnText
                    };
            }
        }

        public ForEmailCallToAction FoundError(string customerName, int orderId)
        {
            switch(UICultureName)
            {
                case "ru-RU":
                    return new ForEmailCallToAction
                    {
                        Header = $"{PerformerOrderEmRes.Greeting} {customerName}!",
                        BigText = $"{PerformerOrderEmRes.Dear} {customerName}, {PerformerOrderEmRes.ErrorFoundText1}{orderId}. " +
                        $"{PerformerOrderEmRes.ErrorFoundText2} {PerformerOrderEmRes.RuntaskerWish}",
                        ActionBtnText = PerformerOrderEmRes.ErrorFoundButtonText
                    };

                default:
                    return new ForEmailCallToAction
                    {
                        Header = $"{PerformerOrderEmRes.Greeting} {customerName}!",
                        BigText = $"{PerformerOrderEmRes.Dear} {customerName}, {PerformerOrderEmRes.ErrorFoundText1}{orderId}. " +
                        $"{PerformerOrderEmRes.ErrorFoundText2} {PerformerOrderEmRes.RuntaskerWish}",
                        ActionBtnText = PerformerOrderEmRes.ErrorFoundButtonText
                    };
            }
            
        }

        public ForEmailCallToAction ExecutedOrder(string customerName, int orderId, decimal leftSum, string roubleSign)
        {
            switch (UICultureName)
            {
                case "ru-RU":
                    return new ForEmailCallToAction
                    {
                        Header = $"{PerformerOrderEmRes.Greeting} {customerName}!",
                        BigText = $"{PerformerOrderEmRes.Dear} {customerName}, " +
                            $"{PerformerOrderEmRes.ExecutedText1}{orderId}. " +
                            $"{PerformerOrderEmRes.ExecutedText2}{leftSum.ToMoney()}{roubleSign}. " +
                            PerformerOrderEmRes.RuntaskerWish,
                        ActionBtnText = PerformerOrderEmRes.ExecutedButtonText
                    };

                default:
                    return new ForEmailCallToAction
                    {
                        Header = $"{PerformerOrderEmRes.Greeting} {customerName}!",
                        BigText = $"{PerformerOrderEmRes.Dear} {customerName}, " +
                        $"{PerformerOrderEmRes.ExecutedText1}{orderId}. " +
                        $"{PerformerOrderEmRes.ExecutedText2}{leftSum.ToMoney()}{roubleSign}. " +
                        PerformerOrderEmRes.RuntaskerWish,
                        ActionBtnText = PerformerOrderEmRes.ExecutedButtonText
                    };
            }
        }
    }
}
