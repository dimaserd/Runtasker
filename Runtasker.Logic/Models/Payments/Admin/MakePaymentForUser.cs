namespace Runtasker.Logic.Models.Payments.Admin
{
    public class MakePaymentForUser
    {
        public string UserId { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }
    }
}
