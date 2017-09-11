using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runtasker.Logic.Entities
{
    public enum PaymentViaType
    {
        YandexMoney, Robokassa, YandexKassa, Administration
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

        /// <summary>
        /// Хеш рассчитаный платежными системами
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// Идентификатор платежа в эквайринговой системе
        /// </summary>
        public string PaymentServiceId { get; set; }

        /// <summary>
        /// Метка учета данного платежа в моей системе
        /// </summary>
        public bool Confirmed { get; set; }
    }

    
}
