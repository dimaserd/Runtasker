using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace Runtasker.Logic.Workers.Email
{
    //Base class that has settings, credentials
    //and just can send emails via protected method
    public class EmailWorkerBase : IDisposable
    {
        #region Constants
        protected const string Host = "https://runtasker.ru";
        #endregion

        #region Constructors
        public EmailWorkerBase()
        {
            //Settings Parameters
            FromAddress = "info@runtasker.ru";
            IsBodyHtml = true;
            SmtpClientString = "smtp-18.1gb.ru";
            SmtpPort = 465;
            Credentials = new NetworkCredential("u457762", "f0c839a76io");

            Logger = new LogWorker();
            
        }
        #endregion

        #region Private Settings Fields
        private string FromAddress { get; set; }
        private bool IsBodyHtml { get; set; }
        private string SmtpClientString { get; set; }
        private int SmtpPort { get; set; }
        #endregion

        #region Fields
        private NetworkCredential Credentials { get; set; }

        private LogWorker Logger { get; set; }

        public PaymentEmailMethods Payment { get; private set; }
        #endregion

        #region Protected Methods
        //Temporary method
        public void SendEmail(IdentityMessage message)
        {
            using (SmtpClient smtpMail = new SmtpClient(SmtpClientString)
            {
                Port = SmtpPort,
                EnableSsl = false,
                Credentials = Credentials,
            })
            {
                MailMessage mail = new MailMessage(new MailAddress(FromAddress), new MailAddress(message.Destination));

                mail.Subject = message.Subject;
                mail.Body = message.Body;
                mail.IsBodyHtml = IsBodyHtml;
                // and then send the mail
                try { smtpMail.Send(mail); }
                catch (Exception ex)
                {
                    string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
                    path = $"{path}/Logs/TempEmailLog.txt";
                    new LogWriter().WriteTextToLog(ex.Message, path);
                }
            }
        }

        public void SendEmails(IEnumerable<IdentityMessage> messages)
        {
            using (SmtpClient smtpMail = new SmtpClient(SmtpClientString)
            {
                Port = SmtpPort,
                EnableSsl = false,
                Credentials = Credentials,
            })
            {
                foreach(IdentityMessage message in messages)
                {
                    MailMessage mail = new MailMessage(new MailAddress(FromAddress), new MailAddress(message.Destination));

                    mail.Subject = message.Subject;
                    mail.Body = message.Body;
                    mail.IsBodyHtml = IsBodyHtml;
                    // and then send the mail
                    try { smtpMail.Send(mail); }
                    catch (Exception ex)
                    {
                        string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
                        path = $"{path}/Logs/TempEmailLog.txt";
                        new LogWriter().WriteTextToLog(ex.Message, path);
                    }
                }
                
            }
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.
                Credentials = null;
                Logger = null;
                Payment = null;

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~EmailWorkerBase() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
