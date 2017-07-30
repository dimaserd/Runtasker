using System;

namespace Common.JavascriptValidation.Attributes
{
    public class JsOnValueWithElseAttribute : Attribute
    {
        /// <summary>
        /// Значение свойства от которого будет построена [if/else] ветвь кода
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Скрипт который будет выполнен при значении, указанном в Value
        /// </summary>
        public string OnValueScript { get; set; }

        /// <summary>
        /// Скрипт который будет выполнен в остальных случаях,
        /// когда значение не будет равно указанному в Value
        /// </summary>
        public string OnElseScript { get; set; }
    }
}
