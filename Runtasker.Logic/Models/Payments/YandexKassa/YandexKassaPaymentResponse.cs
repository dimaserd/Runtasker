namespace Runtasker.Logic.Models.Payments.YandexKassa
{
    /// <summary>
    /// 
    /// </summary>
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
    //orderSumAmount=10.00
    //cdd_exp_date=1121
    //shopArticleId=427088
    //paymentPayerCode=4100322062290
    //paymentDatetime=2017-04-05T17%3A23%3A53.519%2B03%3A00
    //cdd_rrn=483282312041
    //external_id=deposit
    //paymentType=AC
    //requestDatetime=2017-04-05T17%3A24%3A03.238%2B03%3A00
    //depositNumber=Yi6X9n-8YV15wMH9ugwQpVYY_kYZ.001f.201704
    //submit-button=Pay
    //nst_eplPayment=true
    //cdd_response_code=00
    //cps_user_country_code=PL
    //orderCreatedDatetime=2017-04-05T17%3A23%3A52.922%2B03%3A00
    //sk=u7f1de39b6e6243122047c902ae1deb94
    //shopId=130768
    //scid=551474
    //rebillingOn=false
    //orderSumBankPaycash=1003
    //cps_region_id=213
    //orderSumCurrencyPaycash=10643
    //merchant_order_id=dimaserd96%40yandex.ru_050417172332_00000_
    //cdd_pan_mask=444444%7C4448
    //customerNumber=dimaserd96%40yandex.ru
    //yandexPaymentId=2570083110257
    //invoiceId=2000001134313
}
