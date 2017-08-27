using Common.JavascriptValidation.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.JavascriptValidation.Entities
{
    /// <summary>
    /// Свойство со списком 
    /// </summary>
    public class JsProperty
    {
        /// <summary>
        /// Имя свойства к которому принадлежат проверочные функции
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Проверочные джаваскрипт функции для свойства
        /// </summary>
        public List<string> CheckingScripts { get; set; }

        /// <summary>
        /// Исходные скрипты
        /// </summary>
        public List<string> DefaultScripts { get; set; }

        public JsHtmlDataType HtmlDataType { get; set; }

        public string OnValueChangedHandler { get; set; }
    }
}
