namespace Runtasker.Logic.Models.Payments.YandexKassa
{
    public class YandexKassaPaymentResponse
    {
        public string action { get; set; }
        public string orderSumAmount { get; set; }
        public string orderSumCurrencyPaycash { get; set; }
        public string orderSumBankPaycash { get; set; }
        public string shopId { get; set; }
        public string invoiceId { get; set; }
        public string customerNumber { get; set; }
        public string MD5 { get; set; }
    }
}
