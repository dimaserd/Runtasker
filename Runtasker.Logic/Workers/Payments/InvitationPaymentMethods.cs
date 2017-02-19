
using Logic.Extensions.Namers;
using Runtasker.Logic.Entities;

namespace Runtasker.Logic.Workers.Payments
{
    public class InvitationPaymentMethods
    {
        #region Constructors
        public InvitationPaymentMethods(MyDbContext context)
        {
            Construct(context);
        }

        void Construct(MyDbContext context)
        {
            Context = context;
            DescNamer = new PaymentTransactionDescriptionNamer();
        }
        #endregion

        #region Properties
        MyDbContext Context { get; set; }

        PaymentTransactionDescriptionNamer DescNamer { get; set; }
        #endregion

        #region Public Methods
        public void OnInvitedFeatureRevealed(decimal sum, string userGuid)
        {
            string featureName = "self-invitation";

            PaymentTransaction pt = new PaymentTransaction
            {
                UserGuid = userGuid,
                Description = DescNamer.GetForFeatureRevealed(featureName),
                Sum = sum,
                Type = TransactionType.Recharging
            };

            Context.PaymentTransactions.Add(pt);
            Context.SaveChanges();
        }
        #endregion
    }
}
