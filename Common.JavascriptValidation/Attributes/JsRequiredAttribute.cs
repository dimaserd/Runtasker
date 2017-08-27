using Common.JavascriptValidation.Statics;
using Extensions.Attributes;
using System;
using System.Text;

namespace Common.JavascriptValidation.Attributes
{
    public class JsRequiredAttribute : CustomResourceAttribute
    {
        public JsRequiredAttribute(string ErrorText) : base(ErrorText)
        {

        }

        public JsRequiredAttribute(string resourceName, Type resourceType) : base(resourceType, resourceName)
        {

        }

        public string ErrorText
        {
            get
            {
                return Text;
            }
        }

        public static string GetRequiredValidationFunc(string propName, JsRequiredAttribute attr)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            sb.Append($" if (document.getElementById('{PropertyNameHelper.GetIdForInput(propName)}').value.length == 0) ")
            .Append(" { ")
            .Append(JavaScriptHelper.WriteError(propName, attr.ErrorText))

            .Append(JavaScriptHelper.ReturnFalse)
            .Append(" } ")
            .Append(" else ")
            .Append(" { ")
            .Append(JavaScriptHelper.HideError(propName))
            .Append(JavaScriptHelper.ReturnTrue)
            .Append(" } ")
            .Append("}");

            return sb.ToString();
        }

    }
}
