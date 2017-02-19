using Microsoft.AspNet.Identity;
using Runtasker.Logic.Models;

namespace Runtasker.Logic.Workers.Email
{
    public class ContactEmailMethods : EmailWorkerBase
    {
        #region Constants
        string contactEmail
        {
            get { return "dimaserd84@gmail.com"; }
        }
        #endregion

        public void Contact(ContactViewModel model, string attachmentsLink)
        {
            IdentityMessage m = new IdentityMessage
            {
                Subject = "Сообщение с сайта Runtasker",
                Body = $"<p>От пользователя {model.Email}</p>" +                       
                       $"<p>Номер телефона {model.PhoneNumber}</p>" +
                       $"<p>Тема сообщения {model.Subject}<p>" +
                       $"<p>{model.Message}</p>" +
                       $"<p>{attachmentsLink}</p>",
                Destination = contactEmail
            };
            SendEmail(m);
        }
    }
}
