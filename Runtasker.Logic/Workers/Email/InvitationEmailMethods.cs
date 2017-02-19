using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers.Email
{
    public class InvitationEmailMethods : EmailWorkerBase
    {
        public void OnUserSentAnInvitation(string invitedEmail, string invitatorEmail, string invitationId)
        {
            IdentityMessage m = new IdentityMessage
            {
                Destination = invitedEmail,
                Subject = "You were invited to Runtasker",
                Body = $"<p>User {invitatorEmail} invited you to Runtasker</p>"
                + "<p>Runtasker - is a web application that helps foreign students who study in Russia </p>"
                + "<p>with their homework! Click the link to register easily!</p>"
                + "<p>After registration you will get 300 bonus roubles and lots of opportunities </p>"
                + "<p>to get more bonus roubles! See you on Runtasker!</p>" + 

                $"<a href='{Host}/Account/RegisterByInvitation?id={invitationId}'>Join us!</a>"
            };
            SendEmail(m);
        }
    }
}
