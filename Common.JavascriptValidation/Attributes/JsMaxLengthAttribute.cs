using Common.JavascriptValidation.Statics;
using Extensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.JavascriptValidation.Attributes
{
    public class JsMaxLengthAttribute : CustomResourceAttribute
    {
        public JsMaxLengthAttribute(int maxLength, string errorText) : base(errorText)
        {
            MaxLength = maxLength;
        }

        public JsMaxLengthAttribute(int maxLength, Type resourceType, string resourceName) : base(resourceType, resourceName)
        {
            MaxLength = maxLength;
        }

        public int MaxLength { get; set; }

        public string ErrorText
        {
            get
            {
                return Text;
            }
        }

        public static string GetJsCheckFunction(string propName, JsMaxLengthAttribute attr)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" { ")
            .Append($" if (document.getElementById('{PropertyNameHelper.GetIdForInput(propName)}').value.length > {attr.MaxLength})  ")
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
