using Common.JavascriptValidation.Statics;
using System;
using System.Text;

namespace Common.JavascriptValidation.Attributes
{
    /// <summary>
    /// Атрибут для джаваскриптовой валидации (который скроет форму данного свойства)
    /// </summary>
    public class JsHideByDefaultAttribute : Attribute
    {
        public static string GetHideByDefaultScript(string propName, JsHideByDefaultAttribute attr)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            sb.Append($" HideObject(\"{PropertyNameHelper.GetIdForForm(propName)}\"); ")

            .Append("}");

            return sb.ToString();
        }
    }
}
