using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers.Email
{
    public class EmailSettings
    {
        public string FromAddress { get; set; }

        public bool IsBodyHtml { get; set; }

        public string SmtpClient { get; set; }

        public int SmtpPort { get; set; }

        public NetworkCredential Credentials { get; set; }
        
    }
}
