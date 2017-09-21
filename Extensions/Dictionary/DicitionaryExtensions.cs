using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Dictionary
{
    public static class DicitionaryExtensions
    {
        public static string GetPropertiesString(this Dictionary<string, string> dict)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string key in dict.Keys)
            {
                sb.Append($"{key}=\"{dict[key]}\" ");
            }

            return sb.ToString();
        }

        public static string GetPropertiesString(this Dictionary<string, object> dict)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string key in dict.Keys)
            {
                sb.Append($"{key}=\"{dict[key].ToString()}\" ");
            }

            return sb.ToString();
        }
    }
}
