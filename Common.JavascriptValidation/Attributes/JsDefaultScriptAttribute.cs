using System;

namespace Common.JavascriptValidation.Attributes
{
    /// <summary>
    /// Аттрибут который выполнит скрипт сразу при загрузке страницы 
    /// </summary>
    public class JsDefaultScriptAttribute : Attribute
    {
        public string Script { get; set; }

        public static string GetDefaulScript(JsDefaultScriptAttribute attr)
        {
            return attr.Script;
        }
    }
}
