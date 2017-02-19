using Microsoft.AspNet.Identity;
using Runtasker.Logic.Models;
using Runtasker.Logic.Workers.Email;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Runtasker.Logic.Workers.Admin.Email
{
    public class AdminEmailSender
    {
        #region Fields
        
        EmailSettings _contactSettings = new EmailSettings
        {
            FromAddress = "contact@runtasker.ru",
            IsBodyHtml = true,
            SmtpClient = "smtp-18.1gb.ru",
            SmtpPort = 465,
            Credentials = new NetworkCredential("u458649", "ae6956e7ty")
        };

        EmailSettings _adminSettings = new EmailSettings
        {
            FromAddress = "admin@runtasker.ru",
            IsBodyHtml = true,
            SmtpClient = "smtp-18.1gb.ru",
            SmtpPort = 465,
            Credentials = new NetworkCredential("u458650", "2ae470d23lzx")
        };


        #endregion

        #region Public Methods
        public void SendContactEmail(ActionEmailToCustomer model)
        {
            IdentityMessage mes = new IdentityMessage
            {
                Body = model.Text,
                Subject = model.Subject,
                Destination = model.Email
            };
            try
            {
                SendContactEmail(mes);
            }
            catch(Exception ex)
            {
                string rootDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
                string filePath = $"{rootDirectory}/admin_email.txt";
                string fileContents = $"{DateTime.Now} {ex.Message}\n";

                File.AppendAllText(filePath, fileContents);
            }
            
        }
        #endregion

        #region Help Methods
        protected void SendContactEmail(IdentityMessage message)
        {
            using (SmtpClient smtpMail = new SmtpClient(_contactSettings.SmtpClient)
            {
                Port = _contactSettings.SmtpPort,
                EnableSsl = false,
                Credentials = _contactSettings.Credentials,
            })
            {
                MailMessage mail = new MailMessage(new MailAddress(_contactSettings.FromAddress), new MailAddress(message.Destination));

                mail.Subject = message.Subject;
                mail.Body = message.Body;
                mail.IsBodyHtml = _contactSettings.IsBodyHtml;
                smtpMail.Send(mail);
            }
        }
        #endregion
    }
}
