using Logic.Extensions.Namers;
using Runtasker.Logic.Entities;

namespace Runtasker.Logic.Workers.Payments
{
    public class AccountPaymentMethods
    {
        #region Constructors
        public AccountPaymentMethods(MyDbContext context)
        {
            Construct(context);
        }

        void Construct(MyDbContext context)
        {
            Context = context;
            //Usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MyDbContext()));
            DescNamer = new PaymentTransactionDescriptionNamer();
        }
        #endregion

        #region Properties
        MyDbContext Context { get; set; }
        
        PaymentTransactionDescriptionNamer DescNamer { get; set; }
        #endregion

        #region Public methods
        public void OnUserRegistered(ApplicationUser user)
        {
            PaymentTransaction pt = new PaymentTransaction
            {
                Description = DescNamer.GetForRegisterBonus(),
                UserGuid = user.Id,
                Sum = user.Balance,
                Type = TransactionType.Recharging
            };

            Context.PaymentTransactions.Add(pt);
            Context.SaveChanges();
        }
        #endregion
    }
}
