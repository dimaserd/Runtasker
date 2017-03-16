using Runtaker.LocaleBuiders.Enumerations;
using System.Threading;

namespace Runtasker.LocaleBuilders.Statics
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

                    case "Russian (Russia)":
                        return "ru-RU";

                    case "Chinese (Simplified, PRC)":
                        return "zh-CN";

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
        
        public static Lang Language
        {
            get
            {
                string langText = Thread.CurrentThread.CurrentCulture.DisplayName;
                switch (langText)
                {

                    case "Русский (Россия)":
                        return Lang.Russian;

                    case "Russian (Russia)":
                        return Lang.Russian;

                    case "Chinese (Simplified, PRC)":
                        return Lang.Chinese;

                    default:
                        return Lang.English;
                }
            }  
        }
    }
}
