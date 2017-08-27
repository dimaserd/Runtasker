using Common.JavascriptValidation.Statics;
using Extensions.Attributes;
using System;
using System.Text;

namespace Common.JavascriptValidation.Attributes
{
    public class JsMinLengthAttribute : CustomResourceAttribute
    {
        public JsMinLengthAttribute(int minLength, string errorText) : base(errorText)
        {
            MinLength = minLength;
        }

        public JsMinLengthAttribute(int minLength, Type resourceType, string resourceName) : base(resourceType, resourceName)
        {
            MinLength = minLength;
        }

        public int MinLength { get; set; }

        public string ErrorText
        {
            get
            {
                return Text;
            }
        }

        public static string GetJsCheckFunction(string propName, JsMinLengthAttribute attr)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" { ")
            .Append($" if (document.getElementById('{PropertyNameHelper.GetIdForInput(propName)}').value > {attr.MinLength})  ")
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
