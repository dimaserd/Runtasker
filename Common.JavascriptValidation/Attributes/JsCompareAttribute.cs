using Common.JavascriptValidation.Statics;
using Extensions.Attributes;
using System;
using System.Text;

namespace Common.JavascriptValidation.Attributes
{
    public class JsCompareAttribute : CustomResourceAttribute
    {
        #region Конструкторы
        public JsCompareAttribute(string comparingPropName, string ErrorText) : base(ErrorText)
        {
            ComparingPropertyName = comparingPropName;
        }

        public JsCompareAttribute(string comparingPropName, string resourceName, Type resourceType) : base(resourceType, resourceName)
        {
            ComparingPropertyName = comparingPropName;
        }
        #endregion

        #region Свойства

        public string ComparingPropertyName { get; set; }

        public string ErrorText
        {
            get
            {
                return Text;
            }
        }
        #endregion

        public static string GetJsCompareCheckingScript(string propName, JsCompareAttribute attr)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" { ")
            .Append($" if (document.getElementById('{PropertyNameHelper.GetIdForInput(propName)}').value.toString() !== document.getElementById(\"{PropertyNameHelper.GetIdForInput(attr.ComparingPropertyName)}\").value.toString())  ")
            .Append("{")
            .Append(JavaScriptHelper.WriteError(propName, attr.ErrorText))
            .Append(JavaScriptHelper.ReturnFalse)
            .Append("}")
            .Append(" else ")
            .Append(JavaScriptHelper.HideError(propName))
            .Append(JavaScriptHelper.ReturnTrue)
            .Append("}");
            return sb.ToString();
        }
    }
}
