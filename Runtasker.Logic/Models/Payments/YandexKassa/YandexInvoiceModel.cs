using Common.JavascriptValidation.Attributes;
using Extensions.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Runtasker.Logic.Models.Payments.YandexKassa
{
    public class YandexInvoiceModel
    {
        public string UserId { get; set; }

        [Display(Name = "Email")]
        [Placeholder(text: "ivan@mail.com")]
        [JsEmail(Text = "Данное поле не является email адресом!")]
        [Tooltip("На данный email будет выслан счет")]
        public string Email { get; set; }

        [Display(Name = "Сумма")]
        [Placeholder(text: "100 рублей")]
        [Tooltip(text: "Сумма в рублях, на которую вы хотите пополнить свой баланс.")]
        [JsDefaultScript(Script = " document.getElementById('AmountInput').type='number'; ")]
        public int Amount { get; set; }
    }
}
