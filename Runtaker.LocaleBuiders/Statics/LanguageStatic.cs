using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Runtaker.LocaleBuiders.Statics
{
    public static class LanguageStatic
    {
        public static string LanguageCode
        {
            
            get
            {
                string langText = Thread.CurrentThread.CurrentCulture.DisplayName;
                switch(langText)
                {
                    case "Русский (Россия)":
                        return "ru-RU";

                    default:
                        return "en-GB";
                }
            }
        }

        public static string JSLangCode
        {
            get
            {
                string langCode = LanguageCode;
                switch(langCode)
                {
                    case "ru-RU":
                        return "ru";

                    default:
                        return "en";
                }
            }
        }
        
    }
}
