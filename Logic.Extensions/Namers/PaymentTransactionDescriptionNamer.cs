
using Runtasker.Resources.Namers.Payment;
using System;

namespace Logic.Extensions.Namers
{
    public class PaymentTransactionDescriptionNamer
    {
        public string GetForPayOnlineHelp(int orderId)
        {
            return string.Format(PaymentNamerRes.OnlineOrderPaidFormat, orderId);
        }

        public string GetForPayFirstHalfOfAnOrder(int orderId)
        {
            return string.Format(PaymentNamerRes.OrderFirstHalfPaidFormat, orderId);
        }

        public string GetForPayAnotherHalfOfAnOrder(int orderId)
        {
            return string.Format(PaymentNamerRes.OrderSecondHalfPaidFormat, orderId);
        }

        public string GetForInvitedUser(string email)
        {
            return string.Format(PaymentNamerRes.ForInvitedUserFormat, email);
        }

        public string PaymentWithRobokassaService()
        {
            return string.Format(PaymentNamerRes.BalanceRechargingViaServiceFormat, "RoboKassa");
        }

        public string PaymentWithYandexMoneyFirstService()
        {
            return string.Format(PaymentNamerRes.BalanceRechargingViaServiceFormat, "YandexMoney");
        }

        public string GetForRegisterBonus()
        {
            return PaymentNamerRes.RegistrationBonus;
        }

        public string GetForFeatureRevealed(string featureName)
        {
            return string.Format(PaymentNamerRes.RevealedFeatureFormat, featureName);
        }

        public string GetForBugBounty()
        {
            throw new NotImplementedException();
        }
    }
}
