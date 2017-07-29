using System;
using System.Collections.Generic;
using System.Linq;

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
    }

    


    #region Не реалзованное
    //public class JsOnValuesAttribute : Attribute
    //{
    //    public JsOnValuesAttribute(JsOnValueScriptPair pair)
    //    {
            
    //    }

    //    public JsOnValuesAttribute(params JsOnValueScriptPair[] values)
    //    {
    //        this.ValueScriptPairs = values.ToList();
    //    }
    //    public List<JsOnValueScriptPair> ValueScriptPairs { get; set; }

    //}

    //public struct JsOnValueScriptPair
    //{
    //    public string Value { get; set; }

    //    public string Script { get; set; }
    //}
    #endregion
}
