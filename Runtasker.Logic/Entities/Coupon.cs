using System.Collections.Generic;

namespace Runtasker.Logic.Entities
{
    /// <summary>
    /// Пока купоны будут только на бонусные деньги
    /// </summary>
    public class Coupon
    {
        #region Свойства
        public string Id { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

        public CouponType Type { get; set; }

        public decimal BonusMoney { get; set; }
        #endregion

        #region Отношения к коллекциям
        public virtual ICollection<ApplicationUser> Users { get; set; }
        #endregion
    }

    public enum CouponType
    {
        PaymentDiscount,
    } 
}
