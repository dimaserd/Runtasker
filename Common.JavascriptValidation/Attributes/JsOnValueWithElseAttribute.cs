using System;
using System.Text;

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

        public static string GetJsOnValueWithElseHalfScript(string propName, JsOnValueWithElseAttribute attr)
        {
            StringBuilder sb = new StringBuilder();


            sb.Append($" if(value === \"{attr.Value}\")  ")
            .Append("{")
            .Append(" console.log(\"Попадание в условие!\"); ")
            .Append($" {attr.OnValueScript} ")
            .Append("}")
            .Append("else")
            .Append("{")
            .Append($" {attr.OnElseScript} ")
            .Append("}");


            return sb.ToString();
        }

    }
}
