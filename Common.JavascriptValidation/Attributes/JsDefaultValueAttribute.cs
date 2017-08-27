using Common.JavascriptValidation.Statics;
using System;
using System.Text;

namespace Common.JavascriptValidation.Attributes
{
    /// <summary>
    /// Данный атрибут предназначен для установки значения по умолчанию
    /// для свойства
    /// </summary>
    public class JsDefaultValueAttribute : Attribute
    {
        /// <summary>
        /// Значение которое будет установлено
        /// </summary>
        public string DefaultValue { get; set; }

        #region Генереция скриптов
        public static string GetJsDefaultValueScript(string propName, JsDefaultValueAttribute attr)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" { ")
            .Append($" document.getElementById(\"{PropertyNameHelper.GetIdForInput(propName)}\").value = {attr.DefaultValue} ")

            .Append("}");

            return sb.ToString();
        }
        #endregion
    }
}
