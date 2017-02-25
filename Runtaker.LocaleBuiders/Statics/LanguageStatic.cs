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
