using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runtasker.Logic.Entities
{
    

    public enum TransactionType
    {
        Spending, Recharging, RuntaskerBounty
    }

    public class PaymentTransaction
    {
        #region Constructors
        public PaymentTransaction()
        {
            Date = DateTime.Now;
        }
        #endregion

        [Key]
        public int Id { get; set; }

        public TransactionType Type { get; set; }

        [ForeignKey("User")]
        public string UserGuid { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }

        public decimal Sum { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }
 
    }

    public static class PaymentTransactionsExtensions
    {
        public static string ToBootTableClass(this TransactionType val)
        {
            switch(val)
            {
                case TransactionType.Spending:
                    return "danger";

                case TransactionType.Recharging:
                    return "success";

                case TransactionType.RuntaskerBounty:
                    return "info";

                default:
                    return "";
            }
        }
    }

}
