using System.ComponentModel;

namespace Runtasker.LocaleBuilders.Enumerations
{
    public enum Lang
    {
        [Description("en-GB")]
        English,
        [Description("ru-RU")]
        Russian,
        [Description("zh-CN")]
        Chinese
    }

    public static class LangExtensions
    {
        public static string GetFileEnding(this Lang lang)
        {
            string resx = ".resx";

            switch(lang)
            {
                case Lang.Russian:
                    return $".ru-RU{resx}";

                case Lang.Chinese:
                    return $".zh-CN{resx}";

                default:
                    return resx;
            }
        }
    }
   
}
