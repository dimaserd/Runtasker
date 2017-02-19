using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Runtasker.Logic.Workers.Admin.Email
{
    public class AdminEmailGetter
    {
        public void Read()
        {
            var client = new Pop3Client();
            client.Connect("pop.gmail.com", 995, true);
            client.Authenticate("admin@bendytree.com", "YourPasswordHere");
            var count = client.GetMessageCount();
            Message message = client.GetMessage(count);
            Console.WriteLine(message.Headers.Subject);
        }
    }
}
