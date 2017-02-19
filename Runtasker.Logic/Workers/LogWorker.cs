using System.IO;
using System.Text;

namespace Runtasker.Logic.Workers
{
    public class Logs
    {
        public Logs(string logDirectory)
        {
            LogDirectory = logDirectory;
        }

        private string LogDirectory { get; set; }

        public string PaymentsLog { get { return $"{LogDirectory}/Payments.txt"; } private set { } }

        public string NotificationsLog { get { return $"{LogDirectory}/Notifications.txt"; } private set { } }
    }
    //Just Writes text to logfile and checks whether it exists
    public class LogWriter
    {
        public void WriteTextToLog(string text, string logPath)
        {
            Checking(logPath);
            using (TextWriter sw = new StreamWriter(logPath, true))
            {
                sw.WriteLine(text);
                sw.Close();
            }
        }

        private void Checking(string logPath)
        {
            if(!File.Exists(logPath))
            {
                using (TextWriter tw = File.CreateText(logPath))
                {
                    tw.Close();
                }
            }
        }
    }

    public class DeveloperLogMethods
    {
        #region Constructors
        public DeveloperLogMethods(string logDirectory)
        {
            LogDirectory = logDirectory;
            Logs = new Logs(LogDirectory);
        }
        #endregion

        private string LogDirectory { get; set; }

        public Logs Logs { get; private set; }

        public string GetPaymentsLog()
        {
            string result;
            using (StreamReader sw = new StreamReader(Logs.PaymentsLog, Encoding.UTF8))
            {
                result = sw.ReadToEnd();
                sw.Close();
            }
            return result;
        }

        public string GetNotificationsLog()
        {
            string result;
            using (StreamReader sw = new StreamReader(Logs.NotificationsLog, Encoding.UTF8))
            {
                result = sw.ReadToEnd();
                sw.Close();
            }
            return result;
        }
    }

    public class PaymentLogMethods : LogWriter
    {
        public PaymentLogMethods(string logDirectory)
        {
            LogDirectory = logDirectory;
            PaymentsLog = $"{LogDirectory}/Payments.txt";
            Checking();
        }

        
        private void Checking()
        {
            if (!File.Exists(PaymentsLog))
            {
                using (TextWriter sw = File.CreateText(PaymentsLog))
                {
                    sw.WriteLine("Лог платежей полученный с сайта");
                    sw.Close();
                }
            }
        }

        private string LogDirectory { get; set; }

        private string PaymentsLog { get; set; }

        public void LogPayment(string notification_type = null, string operation_id = null,
            string label = null, string datetime = null, string amount = null, 
            string withdraw_amount = null, string sender = null, string sha1_hash = null,
            string currency = null, bool? codepro = null, string userGuid = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{System.DateTime.Now}")
            .AppendLine($"notification_type = {notification_type},")
            .AppendLine($"operation_id = {operation_id},")
            .AppendLine($"label = {label},")
            .AppendLine($"datetime = {datetime},")
            .AppendLine($"amount = {amount},")
            .AppendLine($"withdraw_amount = {withdraw_amount},")
            .AppendLine($"sender = {sender},")
            .AppendLine($"sha1_hash = {sha1_hash},")
            .AppendLine($"currency = {currency},")
            .AppendLine($"codepro = {codepro}")
            .AppendLine($"userGuid = {userGuid}")
            .AppendLine("-----------------------------------------------");

            WriteTextToLog(sb.ToString(), PaymentsLog);
        }
    }

    public class NotificationLogMethods
    {
        public NotificationLogMethods(string logDirectory)
        {
            LogDirectory = logDirectory;
            NotificationsLog = $"{LogDirectory}/Notifications.txt";
            Checking();
        }

        private void Checking()
        {
            if (!File.Exists(NotificationsLog))
            {
                using (TextWriter sw = File.CreateText(NotificationsLog))
                {
                    sw.WriteLine("Лог уведомлений отправленных с сайта");
                    sw.Close();
                }
            }
        }

        private string LogDirectory { get; set; }
        private string NotificationsLog { get; set; }

        public void LogNotification(string notificationBody, string notificationWay)
        {
            using (StreamWriter sw = new StreamWriter(NotificationsLog, true))
            {
                sw.WriteLine($"{System.DateTime.Now} отправлено уведомление");
                sw.WriteLine($"через {notificationWay}");
                sw.WriteLine("отправлено : ");
                sw.WriteLine(notificationBody);
                sw.WriteLine("-----------------------------------------------");
                sw.Close();
            }
        }
    }

    public class LogWorker
    {
        public LogWorker()
        {
            RootDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
            LogDirectory = $"{RootDirectory}/Logs";
            Checking();
            PaymentMethods = new PaymentLogMethods(LogDirectory);
            NotificationMethods = new NotificationLogMethods(LogDirectory);
            DeveloperMethods = new DeveloperLogMethods(LogDirectory);
        }

        private void Checking()
        {
            if(!Directory.Exists($"{LogDirectory}"))
            {
                Directory.CreateDirectory($"{LogDirectory}");
            }
        }

        private string RootDirectory { get; set; }

        private string LogDirectory { get; set; }

        public PaymentLogMethods PaymentMethods { get;  private set; }
        public NotificationLogMethods NotificationMethods { get; private set; }
        public DeveloperLogMethods DeveloperMethods { get; private set; }
    }

}
