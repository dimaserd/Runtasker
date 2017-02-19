using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runtasker.Logic.Entities
{
    public enum PaymentViaType
    {
        YandexMoney, Robokassa
    }

    public class Payment
    {
        #region Constructors
        public Payment()
        {
            Date = DateTime.Now;
        }
        #endregion

        [Key]
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public PaymentViaType ViaType { get; set; }

        [ForeignKey("User")]
        public string UserGuid { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser User { get; set; } 

        public DateTime Date { get; set; }

        public string Description { get; set; }
    }

    
}
