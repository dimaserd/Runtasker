
namespace Runtasker.Logic.Workers.Developer
{
    public class DeveloperLogs
    {
        public DeveloperLogs()
        {
            Logger = new LogWorker();
        }

        private LogWorker Logger { get; set; }

        public string Payments
        {
            get
            {
                return Logger.DeveloperMethods.GetPaymentsLog();
            }
        }

        public string Notifications
        {
            get
            {
                return Logger.DeveloperMethods.GetNotificationsLog();
            }
        }

        
    }
}
