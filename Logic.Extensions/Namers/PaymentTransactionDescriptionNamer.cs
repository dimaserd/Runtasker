
using System;

namespace Logic.Extensions.Namers
{
    public class PaymentTransactionDescriptionNamer
    {
        public string GetForPayFirstHalfOfAnOrder(int orderId)
        {
            return $"First half of an Order №{orderId} paid";
        }

        public string GetForPayAnotherHalfOfAnOrder(int orderId)
        {
            return $"Second half of an Order №{orderId} paid";
        }

        public string GetForInvitedUser(string email)
        {
            return $"For invited user {email}";
        }

        public string PaymentWithRobokassaService()
        {
            return $"Balance recharging via RoboKassa";
        }

        public string PaymentWithYandexMoneyFirstService()
        {
            return $"Balance recharging via YandexMoney";
        }

        public string GetForRegisterBonus()
        {
            return "Registration Bonus";
        }

        public string GetForFeatureRevealed(string featureName)
        {
            return $"Revealed feature : [{featureName}]";
        }

        public string GetForBugBounty()
        {
            throw new NotImplementedException();
        }
    }
}
