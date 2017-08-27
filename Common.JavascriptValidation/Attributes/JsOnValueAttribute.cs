using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.JavascriptValidation.Attributes
{

    public class JsOnValueForCheckBox
    {

    }

    public class JsOnValue : JsOnValueBaseAttribute
    {

    }

    public class JsOnValue1 : JsOnValueBaseAttribute
    {

    }

    public class JsOnValue2 : JsOnValueBaseAttribute
    {

    }

    /// <summary>
    /// Атрибут джаваскрипт валидации. При изменении значения на значение Value
    /// выполняется код в свойстве Value
    /// </summary>
    public class JsOnValueBaseAttribute : Attribute
    {
        public string Value { get; set; }

        public string Script { get; set; }

        /// <summary>
        /// Часть джаваскрипт функции 
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string GetJsOnValueHalfScript(string propName, JsOnValueBaseAttribute attr)
        {
            StringBuilder sb = new StringBuilder();


            sb.Append($" if(value === \"{attr.Value}\")  ")
            .Append("{")
            .Append(" console.log(\"Попадание в условие!\"); ")
            .Append($" {attr.Script} ")
            .Append("}");


            return sb.ToString();
        }



    }

}
