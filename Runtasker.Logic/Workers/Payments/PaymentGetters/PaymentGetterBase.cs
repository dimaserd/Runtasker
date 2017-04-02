using System.Security.Cryptography;
using System.Text;

namespace Runtasker.Logic.Workers.Payments.PaymentGetters
{
    public class PaymentGetterBase
    {
        protected string GetHash(string val)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] data = sha.ComputeHash(Encoding.Default.GetBytes(val));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
