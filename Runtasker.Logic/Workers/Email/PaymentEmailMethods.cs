using Microsoft.AspNet.Identity;
using Runtasker.Logic.Entities;

namespace Runtasker.Logic.Workers.Email
{
    public class PaymentEmailMethods : EmailWorkerBase
    {
        public PaymentEmailMethods()
        {

            //Logger = new PaymentLogMethods();
            //берем только его так как записываться здесь будут только оплаты
        }


        private PaymentLogMethods Logger { get; set; }

        public void OnPaymentReceived(string toEmailAddress, Payment payment)
        {
            IdentityMessage message = new IdentityMessage
            {
                Subject = "Payment received!",
                Body = $"<p>We have received your payment!</p>" +
                $"{payment.Amount}&#8381; added to your account</p>",
                Destination = toEmailAddress,

            };
            SendEmail(message);
            Log(message.Body, "via Email");
        }

        private void Log(string notificationBody, string notificationWay)
        {

        }
    }
}
