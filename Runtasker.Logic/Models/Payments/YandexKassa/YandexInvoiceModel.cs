using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtasker.Logic.Models.Payments.YandexKassa
{
    public class YandexInvoiceModel
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public int Amount { get; set; }
    }
}
