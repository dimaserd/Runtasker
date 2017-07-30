using System;

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
    }
}
