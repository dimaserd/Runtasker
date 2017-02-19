using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Randoms
{
    public class RandomPassword
    {
        public char[] alphabet =
        {
            'q','w','e','r','t','y','u','i','o','p',
            'a','s','d','f','g','h','j','k','l',
            'z','x','c','v','b','n','m'
        };

        public char[] symbols =
        {
            '!', '$', '%', '&', '*'
        };
        //3 буквы 4 цифры 2 буквы 1 цифра
        public string GetRandomPass()
        {
            StringBuilder sb = new StringBuilder();
            Random rng = new Random();
            sb.Append(alphabet[rng.Next(0, alphabet.Length - 1)]);
            sb.Append(symbols[rng.Next(0, symbols.Length - 1)]);
            sb.Append(alphabet[rng.Next(0, alphabet.Length - 1)]);
            sb.Append(rng.Next(0, 9).ToString());
            sb.Append(rng.Next(0, 9).ToString());
            sb.Append(rng.Next(0, 9).ToString());
            sb.Append(alphabet[rng.Next(0, alphabet.Length - 1)].ToString().ToUpper());
            sb.Append(rng.Next(0, 9).ToString());
            sb.Append(rng.Next(0, 9).ToString());
            sb.Append(symbols[rng.Next(0, symbols.Length - 1)]);


            return sb.ToString();
        }
    }
}
